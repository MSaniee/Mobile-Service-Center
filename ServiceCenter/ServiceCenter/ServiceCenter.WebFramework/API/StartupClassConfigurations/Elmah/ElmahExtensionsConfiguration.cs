using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceCenter.Domain.Core.Settings.Site;

namespace ServiceCenter.WebFramework.API.StartupClassConfigurations.Elmah;

public static class ElmahExtensionsConfiguration
{
    public static void AddElmah(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSetting)
    {
        services.AddElmah<SqlErrorLog>(options =>
        {
            options.Path = siteSetting.ElmahPath;
            options.ConnectionString = configuration.GetConnectionString("Elmah");
            //options.OnPermissionCheck = context => context.User.Identity.IsAuthenticated;
        });
    }
}
