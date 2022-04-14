using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Receipts;

namespace ServiceCenter.Website.Components.Receipts;

public partial class ReceiptTable
{
    [Parameter]
    public List<ReceiptDto> Receipts { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    private void RedirectToUpdate(long id)
    {
        var url = Path.Combine("/updateReceipt/", id.ToString());
        NavigationManager.NavigateTo(url);
    }
}

