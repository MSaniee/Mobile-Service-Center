using ServiceCenter.Domain.Enums;

namespace ServiceCenter.Application.Dtos.Receipts;

public class ReceiptDto
{
    public long Id { get; set; }
    public string Imei { get; set; }
    public MobileBrand MobileBrand { get; set; }
    public string MobileModel { get; set; }
    public string FaultDescription { get; set; }

    public Guid? UserId { get; set; }
}

