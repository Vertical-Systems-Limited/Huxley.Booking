using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Parkings.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Parkings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Parkings.Queries;

public class GetParkingForBooking(
    ILogger<GetParkingForBooking> logger,
    IValidator<GetParkingForBookingRequest> validator,
    IParkingRepository repository
    )
    : IRequestHandler<GetParkingForBookingRequest, Result<GetParkingForBookingResponse>>
{
    public async Task<Result<GetParkingForBookingResponse>> Handle(GetParkingForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetParkingForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetParkingForFileNo(request.FileNumber);
            // return
            return GetParkingForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting parking for booking with file number {FileNo}", request.FileNumber);
            return GetParkingForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetParkingForBookingResponse
{
    public Parking[] Parking { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetParkingForBookingResponse(bool successful, string[] failureReasons, Parking[] parking)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Parking = parking;
    }

    public static GetParkingForBookingResponse CreateSuccess(IEnumerable<Parking> parking) =>
        new GetParkingForBookingResponse(true, [], parking.ToArray());

    public static GetParkingForBookingResponse CreateFailure(string[] reasons) =>
        new GetParkingForBookingResponse(false, reasons, []);

    public static GetParkingForBookingResponse CreateFailure(string reason) =>
        new GetParkingForBookingResponse(false, [reason], []);

    public static GetParkingForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetParkingForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetParkingForBookingRequest : IRequest<Result<GetParkingForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetParkingForBookingRequestValidator : AbstractValidator<GetParkingForBookingRequest>
{
    public GetParkingForBookingRequestValidator()
    {
    }
}