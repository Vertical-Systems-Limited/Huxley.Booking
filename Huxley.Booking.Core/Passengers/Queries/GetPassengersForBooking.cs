using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Passengers.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Passengers.Queries;

public class GetPassengersForBooking(
    ILogger<GetPassengersForBooking> logger,
    IValidator<GetPassengersForBookingRequest> validator,
    IPassengerRepository passengerRepository
    )
    : IRequestHandler<GetPassengersForBookingRequest, Result<GetPassengersForBookingResponse>>
{
    public async Task<Result<GetPassengersForBookingResponse>> Handle(GetPassengersForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetPassengersForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the pax
            var paxList = await passengerRepository.GetPassengersByFileNo(request.FileNumber);
            // return
            return GetPassengersForBookingResponse.CreateSuccess(paxList);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting passengers for booking with file number {FileNo}", request.FileNumber);
            return GetPassengersForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetPassengersForBookingResponse
{
    public Passenger[] Passengers { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetPassengersForBookingResponse(bool successful, string[] failureReasons, Passenger[] passengers)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Passengers = passengers;
    }

    public static GetPassengersForBookingResponse CreateSuccess(IEnumerable<Passenger> passengers) =>
        new GetPassengersForBookingResponse(true, [], passengers.ToArray());

    public static GetPassengersForBookingResponse CreateFailure(string[] reasons) =>
        new GetPassengersForBookingResponse(false, reasons, []);

    public static GetPassengersForBookingResponse CreateFailure(string reason) =>
        new GetPassengersForBookingResponse(false, [reason], []);

    public static GetPassengersForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetPassengersForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetPassengersForBookingRequest : IRequest<Result<GetPassengersForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetPassengersForBookingRequestValidator : AbstractValidator<GetPassengersForBookingRequest>
{
    public GetPassengersForBookingRequestValidator()
    {
    }
}