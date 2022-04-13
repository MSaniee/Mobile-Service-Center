﻿using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Domain.Entities.ReceiptAggregate;

namespace ServiceCenter.Application.Interfaces.Repositories.ReceiptAggregate;

public interface IReceiptRepository : IRepository<Receipt>
{
    Task<List<ReceiptDto>> GetReceipts(Guid userId, Pagable pagable, CancellationToken cancellationToken);
}
