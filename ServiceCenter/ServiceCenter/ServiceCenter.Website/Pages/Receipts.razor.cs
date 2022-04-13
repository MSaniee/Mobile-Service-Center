using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class Receipts
{
    public List<ReceiptDto> ReceiptList { get; set; } = new();
    public MetaData MetaData { get; set; } = new MetaData();
    private Pagable Pagable = new() { Page = 1, PageSize = 10 , Search = null};

    [Inject]
    public IReceiptService ReceiptService { get; set; }

    protected async override Task OnInitializedAsync()
    {
        var pagingResponse  = await ReceiptService.GetReceipts(Pagable);

        ReceiptList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
