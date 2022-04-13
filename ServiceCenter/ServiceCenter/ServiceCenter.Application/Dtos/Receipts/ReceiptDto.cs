using ServiceCenter.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceCenter.Application.Dtos.Receipts;

public class ReceiptDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "IMEI is required field")]
    public string Imei { get; set; }

    [Required(ErrorMessage = "MobileBrand is required field")]
    public MobileBrand MobileBrand { get; set; }

    [Required(ErrorMessage = "MobileModel is required field")]
    public string MobileModel { get; set; }

    [Required(ErrorMessage = "FaultDescription is required field")]
    public string FaultDescription { get; set; }

    [Required(ErrorMessage = "Image is required field")]
    public string Image { get; set; }

    public Guid? UserId { get; set; }
}

