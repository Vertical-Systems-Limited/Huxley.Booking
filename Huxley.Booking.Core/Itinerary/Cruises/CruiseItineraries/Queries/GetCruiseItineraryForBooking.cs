using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Cruises.CruiseItineraries.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Cruises.CruiseItineraries.Queries;


public class GetCruiseItineraryForBooking(
    ILogger<GetCruiseItineraryForBooking> logger,
    IValidator<GetCruiseItineraryForBookingRequest> validator,
    ICruiseItineraryRepository repository
    )
    : IRequestHandler<GetCruiseItineraryForBookingRequest, Result<GetCruiseItineraryForBookingResponse>>
{
    public async Task<Result<GetCruiseItineraryForBookingResponse>> Handle(GetCruiseItineraryForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetCruiseItineraryForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetCruiseItineraryRecordsForFileNo(request.FileNumber);
            // return
            return GetCruiseItineraryForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting cruise itineraries for booking with file number {FileNo}", request.FileNumber);
            return GetCruiseItineraryForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetCruiseItineraryForBookingResponse
{
    public CruiseItineraryRecord[] CruiseItineraryRecords { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetCruiseItineraryForBookingResponse(bool successful, string[] failureReasons, CruiseItineraryRecord[] records)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        CruiseItineraryRecords = records;
    }

    public static GetCruiseItineraryForBookingResponse CreateSuccess(IEnumerable<CruiseItineraryRecord> records) =>
        new GetCruiseItineraryForBookingResponse(true, [], records.ToArray());

    public static GetCruiseItineraryForBookingResponse CreateFailure(string[] reasons) =>
        new GetCruiseItineraryForBookingResponse(false, reasons, []);

    public static GetCruiseItineraryForBookingResponse CreateFailure(string reason) =>
        new GetCruiseItineraryForBookingResponse(false, [reason], []);

    public static GetCruiseItineraryForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetCruiseItineraryForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());
}

public class GetCruiseItineraryForBookingRequest : IRequest<Result<GetCruiseItineraryForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetCruiseItineraryForBookingRequestValidator : AbstractValidator<GetCruiseItineraryForBookingRequest>
{
    public GetCruiseItineraryForBookingRequestValidator()
    {
    }
}