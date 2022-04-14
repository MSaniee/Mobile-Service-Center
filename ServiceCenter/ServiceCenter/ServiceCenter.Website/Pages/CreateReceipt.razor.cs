using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Website.Components;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class CreateReceipt
{
    private ReceiptDto _receipt = new();
    private SuccessNotification _notification;

    [Inject]
    public IReceiptService ReceiptService { get; set; }

    private async Task Create()
    {
        await ReceiptService.CreateReceipt(_receipt);
        _notification.Show();
    }

    private void AssignImageUrl(string imgUrl) => _receipt.ImageUrl = imgUrl;
}
