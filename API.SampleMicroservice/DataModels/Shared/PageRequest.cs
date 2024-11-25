namespace API.SampleMicroservice.DataModels.Shared
{
    public class PageRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
