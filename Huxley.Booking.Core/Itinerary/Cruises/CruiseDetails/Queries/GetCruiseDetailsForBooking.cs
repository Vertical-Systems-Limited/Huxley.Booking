using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Cruises.CruiseDetails.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Cruises.CruiseDetails.Queries;


public class GetCruiseDetailsForBooking(
    ILogger<GetCruiseDetailsForBooking> logger,
    IValidator<GetCruiseDetailsForBookingRequest> validator,
    ICruiseDetailsRepository repository
    )
    : IRequestHandler<GetCruiseDetailsForBookingRequest, Result<GetCruiseDetailsForBookingResponse>>
{
    public async Task<Result<GetCruiseDetailsForBookingResponse>> Handle(GetCruiseDetailsForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetCruiseDetailsForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetCruiseDetailsRecordsForFileNo(request.FileNumber);
            // return
            return GetCruiseDetailsForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting cruise details for booking with file number {FileNo}", request.FileNumber);
            return GetCruiseDetailsForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetCruiseDetailsForBookingResponse
{
    public CruiseDetailsRecord[] CruiseDetailsRecords { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetCruiseDetailsForBookingResponse(bool successful, string[] failureReasons, CruiseDetailsRecord[] records)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        CruiseDetailsRecords = records;
    }

    public static GetCruiseDetailsForBookingResponse CreateSuccess(IEnumerable<CruiseDetailsRecord> records) =>
        new GetCruiseDetailsForBookingResponse(true, [], records.ToArray());

    public static GetCruiseDetailsForBookingResponse CreateFailure(string[] reasons) =>
        new GetCruiseDetailsForBookingResponse(false, reasons, []);

    public static GetCruiseDetailsForBookingResponse CreateFailure(string reason) =>
        new GetCruiseDetailsForBookingResponse(false, [reason], []);

    public static GetCruiseDetailsForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetCruiseDetailsForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());
}

public class GetCruiseDetailsForBookingRequest : IRequest<Result<GetCruiseDetailsForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetCruiseDetailsForBookingRequestValidator : AbstractValidator<GetCruiseDetailsForBookingRequest>
{
    public GetCruiseDetailsForBookingRequestValidator()
    {
    }
}