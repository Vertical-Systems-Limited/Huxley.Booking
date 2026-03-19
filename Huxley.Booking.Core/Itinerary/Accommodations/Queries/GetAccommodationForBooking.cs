using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Accommodations.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Accommodations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Accommodations.Queries;

public class GetAccommodationForBooking(
    ILogger<GetAccommodationForBooking> logger,
    IValidator<GetAccommodationForBookingRequest> validator,
    IAccommodationRepository repository
    )
    : IRequestHandler<GetAccommodationForBookingRequest, Result<GetAccommodationForBookingResponse>>
{
    public async Task<Result<GetAccommodationForBookingResponse>> Handle(GetAccommodationForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetAccommodationForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetAccommodationsByFileNo(request.FileNumber);
            // return
            return GetAccommodationForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting passengers for booking with file number {FileNo}", request.FileNumber);
            return GetAccommodationForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetAccommodationForBookingResponse
{
    public Accommodation[] Accommodations { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetAccommodationForBookingResponse(bool successful, string[] failureReasons, Accommodation[] accommodations)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Accommodations = accommodations;
    }

    public static GetAccommodationForBookingResponse CreateSuccess(IEnumerable<Accommodation> accommodations) =>
        new GetAccommodationForBookingResponse(true, [], accommodations.ToArray());

    public static GetAccommodationForBookingResponse CreateFailure(string[] reasons) =>
        new GetAccommodationForBookingResponse(false, reasons, []);

    public static GetAccommodationForBookingResponse CreateFailure(string reason) =>
        new GetAccommodationForBookingResponse(false, [reason], []);

    public static GetAccommodationForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetAccommodationForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetAccommodationForBookingRequest : IRequest<Result<GetAccommodationForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetAccommodationForBookingRequestValidator : AbstractValidator<GetAccommodationForBookingRequest>
{
    public GetAccommodationForBookingRequestValidator()
    {
    }
}