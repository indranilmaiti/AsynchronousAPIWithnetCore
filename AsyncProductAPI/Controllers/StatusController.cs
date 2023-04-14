using AsyncProductAPI.Data;
using AsyncProductAPI.Dtos;
using AsyncProductAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncProductAPI.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : ControllerBase
    {
        AppDbContext _context;
        public StatusController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("processingstatus/{requestid}")]
        public async Task<IResult> ProcessingStatus(string requestId)
        {
            var processingRequest = _context.HeavyProcessingRequests.FirstOrDefault(lr => lr.RequestId == requestId);

            if (processingRequest == null)
                return Results.NotFound();

            ProcessingStatus processingStatus = new ProcessingStatus
            {
                RequestStatus = processingRequest.RequestStatus,
                RedirectURL = string.Empty
            };
            DateTime estimatedtime;
            if (DateTime.TryParse(processingRequest.EstimatedCompetionTime, out estimatedtime) && estimatedtime < DateTime.Now)
            {
                processingRequest.RequestStatus = "COMPLETE";
                _context.HeavyProcessingRequests.Update(processingRequest);
                await _context.SaveChangesAsync();
            }

            if (string.Equals(processingRequest.RequestStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase))
            {
                processingStatus.RedirectURL = $"final/GetProcessedOutput/{processingRequest.RequestId}";

                return Results.Redirect("https://localhost:7201/" + processingStatus.RedirectURL);
            }
            //assign new estimatted time if required
            processingStatus.EstimatedCompetionTime = DateTime.Now.AddMinutes(2).ToString();
            return Results.Ok(processingStatus);

        }
    }
}
