using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parbad.Builder;
using Parbad.Gateway.Mellat;
using Parbad.Gateway.ParbadVirtual;
using Parbad.Sample.CustomStorage.Storage;

namespace Parbad.Sample.CustomStorage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

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
                .ConfigureStorage(builder => builder.AddStorage<MyStorage>(ServiceLifetime.Transient));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseEndpoints(builder => builder.MapDefaultControllerRoute());

            app.UseParbadVirtualGateway();
        }
    }
}
