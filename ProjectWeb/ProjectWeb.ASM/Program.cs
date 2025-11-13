using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProjectWeb.ASM;
using ProjectWeb.ASM.Authentication;
using ProjectWeb.ASM.Repositorio;
using ProjectWeb.ASM.Repositorio.Google;
using ProjectWeb.ASM.Repositorio.Implementacion;
using System.Text.Json;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7135/") }); /// iis_puerto api: https://localhost:44300/  normal puerto api: https://localhost:7135/
// ?? Aquí puedes agregar la configuración del JSON:
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.PropertyNameCaseInsensitive = true; // Ignora mayúsculas/minúsculas en nombres de propiedades
});
builder.Services.AddBlazorBootstrap();
// para regiostrar la utenticacion
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddSweetAlert2();
builder.Services.AddBlazoredModal();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<GoogleAuthService>();

await builder.Build().RunAsync();
