using Autofac;
using ServiceCenter.Application.Dtos;
using ServiceCenter.Application.Interfaces.Repositories;
using ServiceCenter.Domain.Core.DILifeTimesType;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Infrastructure.Data.Repositories;

namespace ServiceCenter.Infrastructure.IoC.Autofac;
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

        var domainAssembly = typeof(IRepository<>).Assembly;
        var domainCoreAssembly = typeof(Pagable).Assembly;
        var applicationAssembly = typeof(BaseDto<,,>).Assembly;
        var infrastructureAssembly = typeof(AutofacModule).Assembly;

        containerBuilder.RegisterAssemblyTypes(domainAssembly, applicationAssembly, infrastructureAssembly)
            .AssignableTo<IScopedDependency>().AsImplementedInterfaces().InstancePerLifetimeScope();

        containerBuilder.RegisterAssemblyTypes(domainAssembly, applicationAssembly, infrastructureAssembly)
            .AssignableTo<ITransientDependency>().AsImplementedInterfaces().InstancePerDependency();

        containerBuilder.RegisterAssemblyTypes(domainAssembly, applicationAssembly, infrastructureAssembly)
            .AssignableTo<ISingletonDependency>().AsImplementedInterfaces().SingleInstance();
    }
}

