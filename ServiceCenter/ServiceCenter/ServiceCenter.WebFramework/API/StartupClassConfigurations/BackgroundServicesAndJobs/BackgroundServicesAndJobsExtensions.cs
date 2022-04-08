using Microsoft.Extensions.DependencyInjection;

namespace ServiceCenter.WebFramework.API.StartupClassConfigurations.BackgroundServicesAndJobs;

public static class BackgroundServicesAndJobsExtensions
{
    public static void AddBackgroundServices(this IServiceCollection services)
    {

        //services.AddHostedService<SocialItemsBackgroundService>();
        //services.AddSingleton(_ => Channel.CreateUnbounded<SocialItemMessage>());
    }
}
