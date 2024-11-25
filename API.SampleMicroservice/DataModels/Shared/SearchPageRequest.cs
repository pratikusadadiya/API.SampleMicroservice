namespace API.SampleMicroservice.DataModels.Shared
{
    public class SearchPageRequest : PageRequest
    {
        public string? Search { get; set; }
    }
}
