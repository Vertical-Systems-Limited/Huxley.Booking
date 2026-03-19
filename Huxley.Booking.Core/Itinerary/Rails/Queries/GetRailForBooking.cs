using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Rails.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Rails;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Rails.Queries;

public class GetRailForBooking(
    ILogger<GetRailForBooking> logger,
    IValidator<GetRailForBookingRequest> validator,
    IRailRepository repository
    )
    : IRequestHandler<GetRailForBookingRequest, Result<GetRailForBookingResponse>>
{
    public async Task<Result<GetRailForBookingResponse>> Handle(GetRailForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetRailForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetRailsForFileNo(request.FileNumber);
            // return
            return GetRailForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting rails for booking with file number {FileNo}", request.FileNumber);
            return GetRailForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetRailForBookingResponse
{
    public Rail[] Rails { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetRailForBookingResponse(bool successful, string[] failureReasons, Rail[] rails)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Rails = rails;
    }

    public static GetRailForBookingResponse CreateSuccess(IEnumerable<Rail> rails) =>
        new GetRailForBookingResponse(true, [], rails.ToArray());

    public static GetRailForBookingResponse CreateFailure(string[] reasons) =>
        new GetRailForBookingResponse(false, reasons, []);

    public static GetRailForBookingResponse CreateFailure(string reason) =>
        new GetRailForBookingResponse(false, [reason], []);

    public static GetRailForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetRailForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetRailForBookingRequest : IRequest<Result<GetRailForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetRailForBookingRequestValidator : AbstractValidator<GetRailForBookingRequest>
{
    public GetRailForBookingRequestValidator()
    {
    }
}