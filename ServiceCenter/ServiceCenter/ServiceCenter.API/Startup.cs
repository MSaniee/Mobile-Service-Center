using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServiceCenter.Domain.Core.Settings.Site;
using ServiceCenter.WebFramework.API.StartupClassConfigurations;
using ServiceCenter.WebFramework.API.StartupClassConfigurations.BackgroundServicesAndJobs;
using ServiceCenter.WebFramework.API.StartupClassConfigurations.Identity;
using ServiceCenter.WebFramework.API.StartupClassConfigurations.Jwt;
using ServiceCenter.WebFramework.API.StartupClassConfigurations.MediatR;
using ServiceCenter.WebFramework.API.StartupClassConfigurations.Middlewares;
using ServiceCenter.WebFramework.API.StartupClassConfigurations.Swagger;

namespace ServiceCenter.API;

public class Startup
{
    private readonly SiteSettings _siteSettings;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _siteSettings = Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConfigureSettings(Configuration);

        services.AddDBContext(Configuration);

        services.AddCustomIdentity(_siteSettings.IdentitySettings);

        services.AddMinimalMvc();

        services.AddControllers();

        services.AddCustomMediatR();

        services.AddJwtAuthentication(_siteSettings.JwtSettings);

        services.AddBackgroundServices();

        services.AddCustomApiVersioning();

        services.AddSwagger();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.IntializeDatabase();

        app.UseCustomExceptionHandler();

        app.UseHsts(env);

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseCors();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        //app.UseElmah();

        app.UseSwaggerAndUI();

        app.UseCustomRouting();
    }
}

