using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Parbad.Builder;
using Parbad.Gateway.IdPay;
using Parbad.Gateway.ParbadVirtual;
using Parbad.Gateway.ZarinPal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddParbad()
       .ConfigureGateways(gateways =>
       {
           gateways
               .AddZarinPal()
               .WithAccounts(accounts =>
               {
                   accounts.AddInMemory(account =>
                   {
                       account.MerchantId = "f024bc76-f262-456e-8e9b-d59d32e12cd5";
                       account.IsSandbox = true;
                   });
               });

           gateways
               .AddIdPay()
               .WithAccounts(accounts =>
               {
                   accounts.AddInMemory(account =>
                   {
                       account.Api = "API";
                       account.IsTestAccount = true;
                   });
               });

           gateways
               .AddParbadVirtual()
               .WithOptions(options => options.GatewayPath = "/MyVirtualGateway");
       })
       .ConfigureHttpContext(httpContextBuilder => httpContextBuilder.UseDefaultAspNetCore())
       .ConfigureStorage(storageBuilder => storageBuilder.UseMemoryCache());

var host = builder.Build();

host.UseStaticFiles();
host.MapDefaultControllerRoute();

host.UseParbadVirtualGateway();

await host.RunAsync();
