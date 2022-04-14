using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Website.Components;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class UpdateReceipt
{
    private ReceiptDto _receipt;
    private SuccessNotification _notification;

    [Inject]
    IReceiptService ReceiptService { get; set; }

    [Parameter]
    public long Id { get; set; }

    protected async override Task OnInitializedAsync()
    {
        _receipt = await ReceiptService.GetReceipt(Id);
    }

    private async Task Update()
    {
        _receipt.Id = Id;
        await ReceiptService.UpdateReceipt(_receipt);
        _notification.Show();
    }

    private void AssignImageUrl(string imgUrl) => _receipt.ImageUrl = imgUrl;
}
