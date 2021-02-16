using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlazorApp.Shared;
using Microsoft.JSInterop;
using Syncfusion.Blazor;
using System.Globalization;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Authentication.WebAssembly.Msal.Models;

namespace BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigureSyncfusionLicense();
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            RegisterSyncfusion(builder);

            var host = builder.Build();

            SetDefaultApplicationCulture();

            var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
            var result = await jsInterop.InvokeAsync<string>("cultureInfo.get");
            
            ConfigureCultureSwitcher(result);

            builder.Services
                .AddScoped<CustomAuthorizationMessageHandler>();

            builder.Services
                .AddHttpClient("ServerAPI", ConfigureApi())
                .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services
                .AddMsalAuthentication(ConfigureAuthentication(builder));

            await builder.Build().RunAsync();
        }

        private static void ConfigureCultureSwitcher(string result)
        {
            if (result != null)
            {
                // Set the culture from culture switcher
                var culture = new CultureInfo(result);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
        }

        private static void SetDefaultApplicationCulture()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
        }

        private static void RegisterSyncfusion(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
        }

        private static void ConfigureSyncfusionLicense()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider
                            .RegisterLicense("MzU2NzAxQDMxMzgyZTMzMmUzMFdNbUtVakxBTFc5RkVNTTR5WDRvRnBaMkp0VnBoRVlyellpRTV6Nmh3a2s9");
        }

        private static Action<RemoteAuthenticationOptions<MsalProviderOptions>> ConfigureAuthentication(WebAssemblyHostBuilder builder)
        {
            return options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://mybills.onmicrosoft.com/api/default");
                // no popup window
                options.ProviderOptions.LoginMode = "redirect";
            };
        }

        private static Action<HttpClient> ConfigureApi()
        {
            return client =>
            {
                //client.BaseAddress = new Uri("https://mybillsapi.azurewebsites.net/api/");
                client.BaseAddress = new Uri("http://localhost:7071/api/");
                client.DefaultRequestHeaders.Add("x-functions-key", "rqaiKDUmqlaDinVZDSLCvpU8iyab5aB8FkOUuJf90Yygv7CVVMCXIA==");
            };
        }
    }
}