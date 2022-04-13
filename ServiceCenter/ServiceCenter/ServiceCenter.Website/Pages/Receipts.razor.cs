using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class Receipts
{
    public List<ReceiptDto> ReceiptList { get; set; } = new();

    [Inject]
    public IReceiptService ReceiptService { get; set; }

    protected async override Task OnInitializedAsync()
    {
        ReceiptList = await ReceiptService.GetReceipts();
        //just for testing
        foreach (var receipt in ReceiptList)
        {
            Console.WriteLine(receipt.MobileModel);
        }
    }
}
