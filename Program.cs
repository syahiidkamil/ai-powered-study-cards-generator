using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudyCardsGenerator;
using StudyCardsGenerator.Services;
using StudyCardsGenerator.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using StudyCardsGenerator.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5000")
    };


    return client;
});

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudyCardService, StudyCardService>();
builder.Services.AddScoped<StudyCardsStateContainer>();

builder.Services.AddScoped<AuthenticationHeaderHandler>();
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5000");
}).AddHttpMessageHandler<AuthenticationHeaderHandler>();

await builder.Build().RunAsync();