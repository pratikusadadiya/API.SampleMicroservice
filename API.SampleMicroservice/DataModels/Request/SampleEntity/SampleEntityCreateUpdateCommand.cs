namespace API.SampleMicroservice.DataModels.Request
{
    public class SampleEntityCreateUpdateCommand
    {
        public string Name { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
        public string? AlternatePhoneNo { get; set; }
        public string Address { get; set; } = null!;
        public string? Comments { get; set; }
        public bool IsActive { get; set; }
    }
}
