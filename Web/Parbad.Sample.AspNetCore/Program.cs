using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Parbad.Builder;
using Parbad.Gateway.ParbadVirtual;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddParbad()
       .ConfigureGateways(gateways =>
                          {
                              gateways.AddParbadVirtual()
                                      .WithOptions(options => options.GatewayPath = "/virtual");
                          })
       .ConfigureHttpContext(httpContext => httpContext.UseDefaultAspNetCore())
       .ConfigureStorage(storage => storage.UseMemoryCache());

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.MapDefaultControllerRoute().WithStaticAssets();
app.UseParbadVirtualGateway();

await app.RunAsync();
