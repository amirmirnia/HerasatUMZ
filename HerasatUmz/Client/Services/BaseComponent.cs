using Blazored.LocalStorage;
using Domain.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
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


        public ToastType toastType = ToastType.Warning;

        protected HubConnection? hubConnection;

        protected readonly string hubUrl = "https://localhost:7224/hubs/visitors";
        protected readonly string baseUrl = "https://localhost:7224";

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
