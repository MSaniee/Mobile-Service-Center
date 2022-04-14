using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Receipts;
using ServiceCenter.Domain.Core.Utilities.PagesSettings;
using ServiceCenter.Website.Interfaces;

namespace ServiceCenter.Website.Pages;

public partial class Receipts
{
    public List<ReceiptDto> ReceiptList { get; set; } = new();
    public MetaData MetaData { get; set; } = new MetaData();
    private Pagable Pagable = new() { Page = 1, PageSize = 5 , Search = null};

    [Inject]
    public IReceiptService ReceiptService { get; set; }


    protected async override Task OnInitializedAsync()
    {
        await GetReceipts();
    }
    private async Task SelectedPage(int page)
    {
        Pagable.Page = page;
        await GetReceipts();
    }
    private async Task GetReceipts()
    {
        var pagingResponse = await ReceiptService.GetReceipts(Pagable);
        ReceiptList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    private async Task SearchChanged(string searchTerm)
    {
        if(searchTerm.Length > 2)
        {
            Console.WriteLine(searchTerm);

            Pagable.Page = 1;
            Pagable.Search = searchTerm;

            await GetReceipts();
        }
        else if(searchTerm.Length == 0)
        {
            Pagable.Page = 1;
            Pagable.Search = null;

            await GetReceipts();
        }
    }

    private async Task SortChanged(string orderBy)
    {
        Console.WriteLine(orderBy);
        Pagable.OrderBy = orderBy;
        await GetReceipts();
    }

    private async Task DeleteReceipt(long id)
    {
        await ReceiptService.DeleteReceipt(id);
        Pagable.Page = 1;
        await GetReceipts();
    }
}
