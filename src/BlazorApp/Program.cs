using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorApp
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using System.Threading;

    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://mybills-apim.azure-api.net/MyBillsApi" },
                scopes: new[] { "https://mybills.onmicrosoft.com/api/default" });            
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            
            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient("ServerAPI",
                    client => {                        
                        client.BaseAddress = new Uri("https://mybills-apim.azure-api.net/MyBillsApi/");
                    })
                .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://mybills.onmicrosoft.com/api/default");
                // no popup window
                options.ProviderOptions.LoginMode = "redirect";
            });

            await builder.Build().RunAsync();
        }
    }
}