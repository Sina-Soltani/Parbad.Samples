using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parbad.Builder;
using Parbad.Gateway.ParbadVirtual;
using Parbad.Storage.EntityFrameworkCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddParbad()
       .ConfigureGateways(gateways =>
                          {
                              gateways.AddParbadVirtual()
                                      .WithOptions(options => options.GatewayPath = "/virtual");
                          })
       .ConfigureHttpContext(httpContext => httpContext.UseDefaultAspNetCore())
       .ConfigureStorage(storage => storage.UseEfCore(options =>
                                                      {
                                                          const string connectionString = "Connection String";

                                                          // An Assembly where your migrations files are in it. In this sample the files are in the same project.
                                                          var migrationsAssemblyName = typeof(Program).Assembly
                                                                                                      .GetName()
                                                                                                      .Name;

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

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.MapDefaultControllerRoute().WithStaticAssets();
app.UseParbadVirtualGateway();

await app.RunAsync();
