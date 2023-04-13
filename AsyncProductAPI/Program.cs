using AsyncProductAPI.Data;
using AsyncProductAPI.Dtos;
using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=RequestDB.db"));

var app = builder.Build();

app.UseHttpsRedirection();

// Start Endpoint
app.MapPost("api/v1/products", async (AppDbContext context, ListingRequest listingRequest) =>
{
    if (listingRequest == null)
        return Results.BadRequest();

    listingRequest.RequestStatus = "ACCEPT";
    listingRequest.EstimatedCompetionTime = DateTime.Now.AddMinutes(1).ToString();

    await context.ListingRequests.AddAsync(listingRequest);
    await context.SaveChangesAsync();

    return Results.Accepted($"api/v1/productstatus/{listingRequest.RequestId}", listingRequest);

});

// Status endpoint

app.MapGet("api/v1/productstatus/{requestId}", async (AppDbContext context, string requestId) => {
    var listingRequest = context.ListingRequests.FirstOrDefault(lr => lr.RequestId == requestId);

    if (listingRequest == null)
        return Results.NotFound();

    ListingStatus listingStatus = new ListingStatus
    {
        RequestStatus = listingRequest.RequestStatus,
        ResourceURL = String.Empty
    };
    DateTime estimatedtime;
    if (DateTime.TryParse(listingRequest.EstimatedCompetionTime, out estimatedtime) && estimatedtime < DateTime.Now) 
    {
        listingRequest.RequestStatus = "COMPLETE";
        context.ListingRequests.Update(listingRequest);
        await context.SaveChangesAsync();
    }

    if (string.Equals(listingRequest.RequestStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase))
    {
        listingStatus.ResourceURL = $"api/v1/products/{listingRequest.RequestId}";
        //return Results.Ok(listingStatus);

        return Results.Redirect("https://localhost:7201/" + listingStatus.ResourceURL);
    }

    listingStatus.EstimatedCompetionTime = DateTime.Now.AddMinutes(2).ToString();
    return Results.Ok(listingStatus);
});

// Final Endpoint

app.MapGet("api/v1/products/{requestId}", (AppDbContext context, string requestId) =>
{
    var listingRequest = context.ListingRequests.FirstOrDefault(lr => lr.RequestId == requestId);
    if (listingRequest == null)
    {
        return Results.NotFound();
    }

    if (!string.Equals(listingRequest.RequestStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase))
    {
        return Results.NotFound();
    }

    return Results.Ok(listingRequest);



});

app.Run();
