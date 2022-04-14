using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ServiceCenter.Application.Dtos.Receipts;

namespace ServiceCenter.Website.Components.Receipts;

public partial class ReceiptTable
{
    [Parameter]
    public List<ReceiptDto> Receipts { get; set; }

    [Parameter]
    public EventCallback<long> OnDeleted { get; set; }

    [Inject]
    public IJSRuntime Js { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }
    private void RedirectToUpdate(long id)
    {
        var url = Path.Combine("/updateReceipt/", id.ToString());
        NavigationManager.NavigateTo(url);
    }

    private async Task Delete(long id)
    {
        var receipt = Receipts.FirstOrDefault(p => p.Id.Equals(id));

        var confirmed = await Js.InvokeAsync<bool>("confirm", $"Are you sure you want to delete {receipt.Imei} receipt?");
        if (confirmed)
        {
            await OnDeleted.InvokeAsync(id);
        }
    }
}

