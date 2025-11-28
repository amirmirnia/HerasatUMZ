using Blazored.LocalStorage;
using Client.Services.Alert;
using Client.Services.Interface;
using Domain.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Globalization;
using System.Security.Claims;

namespace Client.Services
{
    public class BaseComponent : ComponentBase, IAsyncDisposable
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected HttpClient Http { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected ILocalStorageService _localStorage { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] protected UserContextService UserContext { get; set; } = default!;
        [Inject] protected IVisitLogger Logger { get; set; } = default!;
        [Inject] protected AlertService _AlertService { get; set; } = default!;



        protected HubConnection? hubConnection;

        protected string hubUrl => Navigation.ToAbsoluteUri("/hubs/visitors").ToString();
        protected readonly string baseUrl = "https://localhost:7224";
        protected AuthenticationState authState { get; set; }
        public AlertType toastType = AlertType.Warning;
        private string? role;
        private string? name;
        private string? idCode;

        protected string SetImgRoom(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return $"{baseUrl}/images/default-avatar.png";

            return $"{baseUrl}/uploadsImageRoom/{fileName}";
        }

        protected override async Task OnInitializedAsync()
        {
            authState = await AuthStateProvider.GetAuthenticationStateAsync(); // Fixed here

            await TryShowToastFromQueryString();
            await UserContext.InitializeAsync();
            await InitHubAsync();
            await base.OnInitializedAsync();
        }

        protected async Task InitHubAsync()
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .WithAutomaticReconnect()
                    .Build();
            }

            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
        }

        protected async Task TryShowToastFromQueryString()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("toastMessage", out var msg))
            {

                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("toastType", out var type))
                {
                    if (Enum.TryParse<AlertType>(type, true, out var parsedType))
                    {
                        toastType = parsedType;
                    }
                }

                _AlertService.Show(msg, null, toastType);

                // اختیاری: پاک کردن کوئری استرینگ بدون رفرش

            }
        }

        protected async ValueTask DisposeAsync()
        {
            if (hubConnection != null)
                await hubConnection.DisposeAsync();
        }

        protected void GoHome() => Navigation.NavigateTo("/");

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return DisposeAsync();
        }
    }
}
