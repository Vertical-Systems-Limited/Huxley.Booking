using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Ferries.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Ferries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Ferries.Queries;

public class GetFerryForBooking(
    ILogger<GetFerryForBooking> logger,
    IValidator<GetFerryForBookingRequest> validator,
    IFerryRepository repository
    )
    : IRequestHandler<GetFerryForBookingRequest, Result<GetFerryForBookingResponse>>
{
    public async Task<Result<GetFerryForBookingResponse>> Handle(GetFerryForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetFerryForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetFerriesForFileNo(request.FileNumber);
            // return
            return GetFerryForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting ferries for booking with file number {FileNo}", request.FileNumber);
            return GetFerryForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetFerryForBookingResponse
{
    public Ferry[] Ferries { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetFerryForBookingResponse(bool successful, string[] failureReasons, Ferry[] ferries)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Ferries = ferries;
    }

    public static GetFerryForBookingResponse CreateSuccess(IEnumerable<Ferry> ferries) =>
        new GetFerryForBookingResponse(true, [], ferries.ToArray());

    public static GetFerryForBookingResponse CreateFailure(string[] reasons) =>
        new GetFerryForBookingResponse(false, reasons, []);

    public static GetFerryForBookingResponse CreateFailure(string reason) =>
        new GetFerryForBookingResponse(false, [reason], []);

    public static GetFerryForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetFerryForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetFerryForBookingRequest : IRequest<Result<GetFerryForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetFerryForBookingRequestValidator : AbstractValidator<GetFerryForBookingRequest>
{
    public GetFerryForBookingRequestValidator()
    {
    }
}