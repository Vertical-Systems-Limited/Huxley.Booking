using Huxley.Booking.Core.Bookings.Queries;
using Huxley.Booking.Core.Costs.Queries;
using Huxley.Booking.Core.Documents.Queries;
using Huxley.Booking.Core.Itinerary.BookingItineraries.Queries;
using Huxley.Booking.Core.Passengers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Huxley.Booking.WebApi.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController(
    IMediator mediator

)
{
    [HttpGet]
    [Route("GetBookingByFileNo")]
    public async Task<ActionResult<GetBookingByFileNoResponse>> GetBookingByFileNo(
        [FromQuery] GetBookingByFileNoRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.ToActionResult();
    }

    [HttpGet]
    [Route("GetItineraryByFileNo")]
    public async Task<ActionResult<GetItineraryForBookingResponse>> GetItineraryByFileNo(
        [FromQuery] GetItineraryForBookingRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.ToActionResult();
    }

    [HttpGet]
    [Route("GetPassengersByFileNo")]
    public async Task<ActionResult<GetPassengersForBookingResponse>> GetPassengersByFileNo(
        [FromQuery] GetPassengersForBookingRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.ToActionResult();
    }
    
    [HttpGet]
    [Route("GetDocumentsByFileNo")]
    public async Task<ActionResult<GetDocumentsForBookingResponse>> GetDocumentsByFileNo(
        [FromQuery] GetDocumentsForBookingRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.ToActionResult();
    }
    
    [HttpGet]
    [Route("GetDocumentByLocation")]
    public async Task<ActionResult<GetDocumentByLocationResponse>> GetDocumentByLocation(
        [FromQuery] GetDocumentByLocationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.ToActionResult();
    }
    
    [HttpGet]
    [Route("GetCostsByFileNo")]
    public async Task<ActionResult<GetCostsForBookingResponse>> GetCostsByFileNo(
        [FromQuery] GetCostsForBookingRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return response.ToActionResult();
    }
    
    // TODO: Endpoint for Getting Itineraries
    // TODO: Endpoint for Getting Documents
    // TODO: Endpoint for Importing Booking
}