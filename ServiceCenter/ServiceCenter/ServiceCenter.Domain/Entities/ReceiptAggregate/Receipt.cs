using ServiceCenter.Domain.Entities.UserAggregate;
using ServiceCenter.Domain.Enums;

namespace ServiceCenter.Domain.Entities.ReceiptAggregate;

public class Receipt : BaseEntity<long>
{
    public string Imei { get; set; }
    public MobileBrand MobileBrand { get; set; }
    public string MobileModel { get; set; }
    public string FaultDescription { get; set; }
    public string ImageUrl { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid? TechnicianId { get; set; }
    public User Technician { get; set; }

    public ReceiptStatus Status { get; set; }
}

