using AsyncProductAPI.Data;
using AsyncProductAPI.Dtos;
using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=AsyncRequestReply.db"));

var app = builder.Build();

app.UseHttpsRedirection();

// Start Endpoint
app.MapPost("api/v1/products", async (AppDbContext context, HeavyProcessingRequest processingRequest) =>
{
    if (processingRequest == null)
        return Results.BadRequest();

    processingRequest.RequestStatus = "ACCEPT";
    processingRequest.EstimatedCompetionTime = DateTime.Now.AddMinutes(1).ToString();

    await context.HeavyProcessingRequests.AddAsync(processingRequest);
    await context.SaveChangesAsync();

    ProcessingStatus processingStatus = new ProcessingStatus
    {
        EstimatedCompetionTime = DateTime.Now.AddMinutes(1).ToString(),
        RequestId = processingRequest.RequestId,
        RequestStatus = "ACCEPT"
    };

    return Results.Accepted($"api/v1/productstatus/{processingRequest.RequestId}", processingStatus);

});

// Status endpoint

app.MapGet("api/v1/productstatus/{requestId}", async (AppDbContext context, string requestId) =>
{
    var processingRequest = context.HeavyProcessingRequests.FirstOrDefault(lr => lr.RequestId == requestId);

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
        context.HeavyProcessingRequests.Update(processingRequest);
        await context.SaveChangesAsync();
    }

    if (string.Equals(processingRequest.RequestStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase))
    {
        processingStatus.RedirectURL = $"api/v1/products/{processingRequest.RequestId}";
        //return Results.Ok(listingStatus);

        return Results.Redirect("https://localhost:7201/" + processingStatus.RedirectURL);
    }
    //assign new estimatted time if required
    processingStatus.EstimatedCompetionTime = DateTime.Now.AddMinutes(2).ToString();
    return Results.Ok(processingStatus);
});

// Final Endpoint
app.MapGet("api/v1/products/{requestId}", (AppDbContext context, string requestId) =>
{
    var processingRequest = context.HeavyProcessingRequests.FirstOrDefault(lr => lr.RequestId == requestId);
    if (processingRequest == null)
    {
        return Results.NotFound();
    }

    if (!string.Equals(processingRequest.RequestStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase))
    {
        return Results.NotFound();
    }

    return Results.Ok(processingRequest);



});

app.Run();
