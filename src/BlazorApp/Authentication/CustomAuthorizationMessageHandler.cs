namespace BlazorApp
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://mybillsapi.azurewebsites.net/api", "http://localhost:7001/api" },
                scopes: new[] { "https://mybills.onmicrosoft.com/api/default" });
        }
    }
}