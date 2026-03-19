using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Excursions.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Excursions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Excursions.Queries;

public class GetExcursionForBooking(
    ILogger<GetExcursionForBooking> logger,
    IValidator<GetExcursionForBookingRequest> validator,
    IExcursionRepository repository
    )
    : IRequestHandler<GetExcursionForBookingRequest, Result<GetExcursionForBookingResponse>>
{
    public async Task<Result<GetExcursionForBookingResponse>> Handle(GetExcursionForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetExcursionForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetExcursionsForFileNo(request.FileNumber);
            // return
            return GetExcursionForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting excursions for booking with file number {FileNo}", request.FileNumber);
            return GetExcursionForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetExcursionForBookingResponse
{
    public Excursion[] Excursions { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetExcursionForBookingResponse(bool successful, string[] failureReasons, Excursion[] excursions)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Excursions = excursions;
    }

    public static GetExcursionForBookingResponse CreateSuccess(IEnumerable<Excursion> excursions) =>
        new GetExcursionForBookingResponse(true, [], excursions.ToArray());

    public static GetExcursionForBookingResponse CreateFailure(string[] reasons) =>
        new GetExcursionForBookingResponse(false, reasons, []);

    public static GetExcursionForBookingResponse CreateFailure(string reason) =>
        new GetExcursionForBookingResponse(false, [reason], []);

    public static GetExcursionForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetExcursionForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetExcursionForBookingRequest : IRequest<Result<GetExcursionForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetExcursionForBookingRequestValidator : AbstractValidator<GetExcursionForBookingRequest>
{
    public GetExcursionForBookingRequestValidator()
    {
    }
}