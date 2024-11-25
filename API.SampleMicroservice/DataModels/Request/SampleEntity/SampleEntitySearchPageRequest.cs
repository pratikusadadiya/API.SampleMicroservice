using API.SampleMicroservice.DataModels.Shared;

namespace API.SampleMicroservice.DataModels.Request
{
    public class SampleEntitySearchPageRequest : SearchPageRequest
    {
        public string? PhoneNo { get; set; }
        public string? AlternatePhoneNo { get; set; }
        public bool? IsActive { get; set; }
    }
}
