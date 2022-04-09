using ServiceCenter.Domain.Entities.UserAggregate;

namespace ServiceCenter.Application.Interfaces.Repositories.UserAggregate;

public interface IActivationCodeRepository : IRepository<ActivationCode>
{
    Task<ActivationCode> GetCodeByPhoneNumber(string phoneNubmer, CancellationToken cancellationToken);
}
