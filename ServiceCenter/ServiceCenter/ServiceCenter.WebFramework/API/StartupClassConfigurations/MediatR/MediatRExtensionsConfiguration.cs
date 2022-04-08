using Microsoft.Extensions.DependencyInjection;
using ServiceCenter.Application.Dtos;
using MediatR;
using ServiceCenter.Application.Features.Behaviours;

namespace ServiceCenter.WebFramework.API.StartupClassConfigurations.MediatR;

public static class MediatRExtensionsConfiguration
{
    public static void AddCustomMediatR(this IServiceCollection services)
    {
        var assembly = typeof(BaseDto<,,>).Assembly;
        services.AddMediatR(assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
    }
}


