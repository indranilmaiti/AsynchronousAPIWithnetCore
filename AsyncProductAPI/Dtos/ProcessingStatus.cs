namespace AsyncProductAPI.Dtos
{
    public class ProcessingStatus
    {
        public string? RequestStatus { get; set; }
        public string? EstimatedCompetionTime { get; set; }
        public string? RedirectURL { get; set; }
        public string? RequestId { get; set; }
    }
}