using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parbad.Builder;
using Parbad.Gateway.Mellat;
using Parbad.Gateway.ParbadVirtual;
using Parbad.Storage.EntityFrameworkCore.Builder;

namespace Parbad.Sample.EntityFrameworkCore
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
                .ConfigureStorage(builder => builder.UseEfCore(options =>
                {
                    const string connectionString = "Connection String";
                    var migrationsAssemblyName = typeof(Startup).Assembly.GetName().Name; // An Assembly where your migrations files are in it. In this sample the files are in the same project.

                    options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql =>
                    {
                        sql.MigrationsAssembly(migrationsAssemblyName);
                        sql.MigrationsHistoryTable("TABLE NAME");
                    });

                    ///////////////////////////////////////////////////
                    // Optional Settings for Table names and schemas //
                    ///////////////////////////////////////////////////

                    //options.DefaultSchema = "Parbad";

                    //options.PaymentTableOptions.Name = "TABLE NAME";
                    //options.PaymentTableOptions.Schema = "SCHEMA NAME";

                    //options.TransactionTableOptions.Name = "TABLE NAME";
                    //options.TransactionTableOptions.Schema = "SCHEMA NAME";
                }));
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
