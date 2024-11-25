namespace API.SampleMicroservice.DataModels.Shared
{
    public class DataQueryResponseModel<TResponseModel> where TResponseModel : class
    {
        public IEnumerable<TResponseModel> Records { get; set; } = Enumerable.Empty<TResponseModel>();
        public int TotalRecords { get; set; }
    }
}
