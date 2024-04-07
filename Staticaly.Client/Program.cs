using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Staticaly.Client;
using Staticaly.Client.Data;
using Staticaly.Client.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5233") });
builder.Services.AddScoped<UsuariosClient>();
builder.Services.AddScoped<EmailServiceClient>();
builder.Services.AddScoped<EquiposClient>();
builder.Services.AddScoped<PublicacionesClient>();
builder.Services.AddScoped<ImageServiceClient>();
builder.Services.AddScoped<ComentariosClient>();
builder.Services.AddScoped<CuestionarioClient>();
builder.Services.AddScoped<PreguntasClient>();
builder.Services.AddScoped<OpcionesCuestionarioClient>();

await builder.Build().RunAsync();
