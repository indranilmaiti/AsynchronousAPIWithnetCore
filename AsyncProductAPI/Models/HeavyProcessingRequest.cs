namespace AsyncProductAPI.Models
{
    public class HeavyProcessingRequest
    {
        public int Id { get; set; }
        public string? RequestBody { get; set; }
        public string? EstimatedCompetionTime { get; set; }
        public string? RequestStatus { get; set; }
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
    }
}