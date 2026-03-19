using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Flights.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Flights;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Flights.Queries;


public class GetFlightsForBooking(
    ILogger<GetFlightsForBooking> logger,
    IValidator<GetFlightsForBookingRequest> validator,
    IFlightRepository repository
    )
    : IRequestHandler<GetFlightsForBookingRequest, Result<GetFlightsForBookingResponse>>
{
    public async Task<Result<GetFlightsForBookingResponse>> Handle(GetFlightsForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetFlightsForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetFlightsByFileNo(request.FileNumber);
            // return
            return GetFlightsForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting passengers for booking with file number {FileNo}", request.FileNumber);
            return GetFlightsForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetFlightsForBookingResponse
{
    public Flight[] Flights { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetFlightsForBookingResponse(bool successful, string[] failureReasons, Flight[] flights)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Flights = flights;
    }

    public static GetFlightsForBookingResponse CreateSuccess(IEnumerable<Flight> flights) =>
        new GetFlightsForBookingResponse(true, [], flights.ToArray());

    public static GetFlightsForBookingResponse CreateFailure(string[] reasons) =>
        new GetFlightsForBookingResponse(false, reasons, []);

    public static GetFlightsForBookingResponse CreateFailure(string reason) =>
        new GetFlightsForBookingResponse(false, [reason], []);

    public static GetFlightsForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetFlightsForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetFlightsForBookingRequest : IRequest<Result<GetFlightsForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetFlightsForBookingRequestValidator : AbstractValidator<GetFlightsForBookingRequest>
{
    public GetFlightsForBookingRequestValidator()
    {
    }
}