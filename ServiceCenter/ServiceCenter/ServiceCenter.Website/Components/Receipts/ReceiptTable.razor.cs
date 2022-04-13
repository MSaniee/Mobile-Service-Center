using Microsoft.AspNetCore.Components;
using ServiceCenter.Application.Dtos.Receipts;

namespace ServiceCenter.Website.Components.Receipts;

public partial class ReceiptTable
{
    [Parameter]
    public List<ReceiptDto> Receipts { get; set; }
}

