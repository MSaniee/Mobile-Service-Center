using ServiceCenter.Application.Interfaces.Repositories.UserAggregate;
using ServiceCenter.Domain.Core.DILifeTimesType;
using ServiceCenter.Infrastructure.Data.SqlServer.EfCore.Context;

namespace ServiceCenter.Infrastructure.Data.Repositories.UserAggregate;

public class ActivationCodeRepository : Repository<ActivationCode>, IActivationCodeRepository, IScopedDependency
{
    public ActivationCodeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<ActivationCode> GetCodeByPhoneNumber(string phoneNubmer, CancellationToken cancellationToken)
    {
        return Table.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNubmer, cancellationToken);
    }
}