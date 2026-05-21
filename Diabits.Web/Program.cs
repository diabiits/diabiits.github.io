using ApexCharts;

using Diabits.Web;
using Diabits.Web.Infrastructure.Api;
using Diabits.Web.Infrastructure.Services.Auth;
using Diabits.Web.Infrastructure.Services.Dashboard;
using Diabits.Web.Infrastructure.Services.HealthData;
using Diabits.Web.Infrastructure.Services.Invites;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.ClearAfterNavigation = true;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
});

builder.Services.AddApexCharts(e =>
{
    e.GlobalOptions = new ApexChartBaseOptions
    {
        Debug = true,
        //Theme = new Theme
        //{
        //    Mode = Mode.Light,
        //    Palette = PaletteType.Palette1,
        //    Monochrome = new ThemeMonochrome {
        //        Enabled = true,
        //        Color = "#91B3C9",//"#91CA97",//"#e27396",
        //        ShadeTo = Mode.Light,
        //        ShadeIntensity = 1
        //    }
        //},
        States = new States
        {
            Hover = new StatesHover
            {
                Filter = new StatesFilter
                {
                    Type = StatesFilterType.none
                }
            },
            Active = new StatesActive
            {
                Filter = new StatesFilter
                {
                    Type = StatesFilterType.none
                }
            }
        }        
    };
});

builder.Services.AddSingleton<JwtAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthStateProvider>());
builder.Services.AddSingleton<TokenStorage>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<InviteService>();
builder.Services.AddScoped<HealthDataService>();
builder.Services.AddScoped<DashboardService>();

builder.Services.AddScoped<AuthorizationHandler>();

builder.Services.AddHttpClient<ApiClient>(client =>
{
    //TODO Fix hardcoded URL
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://diabits-api-fsgmd3fuh9c9evfe.swedencentral-01.azurewebsites.net");
})
.AddHttpMessageHandler<AuthorizationHandler>(); // Automatically adds JWT to all requests

await builder.Build().RunAsync();