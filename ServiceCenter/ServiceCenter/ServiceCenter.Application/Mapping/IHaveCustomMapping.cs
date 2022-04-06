using AutoMapper;

namespace ServiceCenter.Application.Mapping
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile profile);
    }
}
