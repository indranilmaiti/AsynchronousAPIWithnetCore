using AsyncProductAPI.Data;
using AsyncProductAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AsyncProductAPI.Controllers
{
    [ApiController]
    [Route("final")]
    public class FinalController : ControllerBase
    {
        AppDbContext _context;
        public FinalController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("GetProcessedOutput/{requestid}")]
        public IResult GetProcessedOutput(string requestId)
        {
            var processingRequest =  _context.HeavyProcessingRequests.FirstOrDefault(lr => lr.RequestId == requestId);
            if (processingRequest == null)
            {
                return Results.NotFound();
            }

            if (!string.Equals(processingRequest.RequestStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase))
            {
                return Results.NotFound();
            }

            return Results.Ok(processingRequest);

        }
    }
}
