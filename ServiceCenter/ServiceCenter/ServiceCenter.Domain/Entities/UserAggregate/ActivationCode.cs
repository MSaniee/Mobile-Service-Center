namespace ServiceCenter.Domain.Entities.UserAggregate;

public class ActivationCode : BaseEntity<long>
{
    public string PhoneNumber { get; set; }
    public int Code { get; set; }
    public DateTime ExpireDate { get; set; }
    public bool Used { get; set; }
    public int Step { get; set; }

    
    public void SetExpireDate(int step)
    {
        DateTime dtNow = DateTime.Now;

        ExpireDate = step switch
        {
            0 => dtNow.AddSeconds(60),
            1 => dtNow.AddSeconds(90),
            2 => dtNow.AddSeconds(120),
            3 => dtNow.AddSeconds(300),
            4 => dtNow.AddSeconds(3600),
            _ => dtNow.AddHours(12)
        };
    }
}
