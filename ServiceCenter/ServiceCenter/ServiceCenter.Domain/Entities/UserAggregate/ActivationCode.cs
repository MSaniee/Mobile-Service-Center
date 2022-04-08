namespace ServiceCenter.Domain.Entities.UserAggregate;

public class ActivationCode : BaseEntity<long>
{
    public string PhoneNumber { get; set; }
    public int Code { get; set; }
    public DateTime ExpireDate { get; set; }
    public bool Used { get; set; }
    public int Step { get; set; }
}
