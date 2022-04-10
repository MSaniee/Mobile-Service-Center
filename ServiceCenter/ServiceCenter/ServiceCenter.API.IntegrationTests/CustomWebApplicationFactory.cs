using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceCenter.Infrastructure.Data.SqlServer.EfCore.Context;
using ServiceCenter.Infrastructure.IoC.Autofac;
using SqlInMemory;
using System;
using System.Linq;

namespace ServiceCenter.API.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IDisposable
       where TStartup : class
{
    public bool isDataInitialized = false;
    public ApplicationDbContext DbContext { set; get; }
    public IDisposable DbDisposable { get; set; }

    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder()
                          .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                          .ConfigureContainer<ContainerBuilder>(builder =>
                          {
                              builder.RegisterModule(new AutofacModule());
                          })
                          //.UseSerilog(LoggingConfiguration.ConfigureLogger)
                          .ConfigureWebHostDefaults(builder =>
                          {
                              builder.UseStartup<TStartup>().UseTestServer();
                              builder.ConfigureServices(service =>
                              {
                                  var descriptor = service.SingleOrDefault(
                                        d => d.ServiceType ==
                                        typeof(DbContextOptions<ApplicationDbContext>));

                                  service.Remove(descriptor);

                                  var connectionString = "Data Source=.;Initial Catalog=TestDb;Integrated Security=true";
                                  DbDisposable = SqlInMemoryDb.Create(connectionString);
                                  service.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));

                                  var serviceProvider = service.BuildServiceProvider();
                                  var appDbContext = serviceProvider.GetService<ApplicationDbContext>();
                                  DbContext = appDbContext;
                                  appDbContext.Database.Migrate();

                                      
                              });
                          });
        return builder;
    }


    public void Dispose()
    {
        DbDisposable.Dispose();
    }
}

