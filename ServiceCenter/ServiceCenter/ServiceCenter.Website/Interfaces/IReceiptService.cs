using ServiceCenter.Application.Dtos.Receipts;

namespace ServiceCenter.Website.Interfaces;

public interface IReceiptService
{
    Task<List<ReceiptDto>> GetReceipts();
}

