using Microsoft.AspNetCore.Components.Authorization;
using System.Net;

namespace Client.Services.Auth
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly SemaphoreSlim _refreshLock = new(1, 1);

        public AuthDelegatingHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized ||
                IsAuthEndpoint(request.RequestUri))
            {
                return response;
            }

            response.Dispose();

            var refreshed = await TryRefreshAsync(cancellationToken);
            if (!refreshed)
            {
                NotifyAuthChanged();
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    RequestMessage = request
                };
            }

            var retryRequest = await CloneRequestAsync(request);
            return await base.SendAsync(retryRequest, cancellationToken);
        }

        private async Task<bool> TryRefreshAsync(CancellationToken cancellationToken)
        {
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                using var refreshClient = CreateBareClient();
                using var refreshResponse = await refreshClient.PostAsync(
                    "api/auth/refresh", content: null, cancellationToken);
                return refreshResponse.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private HttpClient CreateBareClient()
        {
            var factory = (IHttpClientFactory)_serviceProvider.GetService(typeof(IHttpClientFactory))!;
            return factory.CreateClient("HerasatUmz.NoAuth");
        }

        private void NotifyAuthChanged()
        {
            var provider = _serviceProvider.GetService(typeof(AuthenticationStateProvider))
                as AuthenticationStateProvider;
            if (provider is JwtAuthenticationStateProvider jwtProvider)
                jwtProvider.NotifyAuthenticationStateChangedExternal();
        }

        private static bool IsAuthEndpoint(Uri? uri)
        {
            if (uri == null) return false;
            var path = uri.AbsolutePath;
            return path.Contains("/api/auth/refresh", StringComparison.OrdinalIgnoreCase)
                || path.Contains("/api/auth/login", StringComparison.OrdinalIgnoreCase)
                || path.Contains("/api/auth/logout", StringComparison.OrdinalIgnoreCase);
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            foreach (var prop in request.Options)
                ((IDictionary<string, object?>)clone.Options)[prop.Key] = prop.Value;

            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);
                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}
