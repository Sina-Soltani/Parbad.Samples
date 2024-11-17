using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Parbad.Builder;
using Parbad.Gateway.Mellat;
using Parbad.Gateway.ParbadVirtual;
using Parbad.Sample.Angular.Repositories;

namespace Parbad.Sample.Angular;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews().AddNewtonsoftJson(options =>
                                                             {
                                                                 options.SerializerSettings.Converters.Add(new StringEnumConverter());
                                                             });

        services.AddSpaStaticFiles(configuration =>
                                   {
                                       configuration.RootPath = "ClientApp/dist";
                                   });

        services.AddParbad()
                .ConfigureGateways(gateways =>
                                   {
                                       gateways
                                          .AddMellat()
                                          .WithAccounts(accounts =>
                                                        {
                                                            accounts.AddInMemory(account =>
                                                                                 {
                                                                                     account.Name = "Mellat"; // optional if there is only 1 account for this gateway
                                                                                     account.TerminalId = 123;
                                                                                     account.UserName = "MyId";
                                                                                     account.UserPassword = "MyPassword";
                                                                                 });
                                                        });

                                       gateways
                                          .AddParbadVirtual()
                                          .WithOptions(options => options.GatewayPath = "/MyVirtualGateway");
                                   })
                .ConfigureHttpContext(builder => builder.UseDefaultAspNetCore())
                .ConfigureStorage(builder => builder.UseMemoryCache());

        services.AddSingleton<IOrderRepository, OrderRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
                         {
                             endpoints.MapControllerRoute(
                                                          name: "default",
                                                          pattern: "{controller}/{action=Index}/{id?}");
                         });

        app.UseParbadVirtualGateway();

        app.UseSpa(spa =>
                   {
                       spa.Options.SourcePath = "ClientApp";

                       if (env.IsDevelopment())
                       {
                           spa.UseAngularCliServer(npmScript: "start");
                       }
                   });
    }
}