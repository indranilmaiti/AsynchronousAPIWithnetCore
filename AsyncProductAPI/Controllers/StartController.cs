using AsyncProductAPI.Data;
using AsyncProductAPI.Dtos;
using AsyncProductAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncProductAPI.Controllers
{
    [ApiController]
    [Route("start")]
    public class StartController : ControllerBase
    {
        AppDbContext _context;
        public StartController(AppDbContext context) 
        {
            _context = context;
        }


        [HttpPost]
        [Route("startprocessing")]
        public async Task<IResult> StartProcessing(HeavyProcessingRequest processingRequest)
        {
            if (processingRequest == null)
                return Results.BadRequest();

            processingRequest.RequestStatus = "ACCEPT";
            processingRequest.EstimatedCompetionTime = DateTime.Now.AddMinutes(1).ToString();

            await _context.HeavyProcessingRequests.AddAsync(processingRequest);
            await _context.SaveChangesAsync();

            ProcessingStatus processingStatus = new ProcessingStatus
            {
                EstimatedCompetionTime = DateTime.Now.AddMinutes(1).ToString(),
                RequestId = processingRequest.RequestId,
                RequestStatus = "ACCEPT"
            };

            return Results.Accepted($"api/v1/productstatus/{processingRequest.RequestId}", processingStatus);

        }
    }
}
