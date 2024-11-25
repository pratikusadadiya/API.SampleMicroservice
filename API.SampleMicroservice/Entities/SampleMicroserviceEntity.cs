namespace API.SampleMicroservice.Entities;

public partial class SampleMicroserviceEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Comments { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ModifiedBy { get; set; }

    public string? AlternatePhoneNo { get; set; }
}
