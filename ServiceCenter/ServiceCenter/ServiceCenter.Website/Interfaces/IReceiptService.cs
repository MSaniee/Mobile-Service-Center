using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Website.Features;

namespace ServiceCenter.Website.Interfaces;

public interface IReceiptService
{
    Task<PagingResponse<ReceiptDto>> GetReceipts(Pagable pagable);
    Task CreateReceipt(ReceiptDto receipt);
    Task<string> UploadImage(MultipartFormDataContent content);
}

