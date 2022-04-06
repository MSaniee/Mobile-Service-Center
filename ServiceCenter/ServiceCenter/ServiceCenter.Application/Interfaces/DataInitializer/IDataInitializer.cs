using ServiceCenter.Domain.Core.DILifeTimesType;

namespace ServiceCenter.Application.Interfaces.DataInitializer
{
    public interface IDataInitializer : IScopedDependency
    {
        void InitializeData();
    }
}
