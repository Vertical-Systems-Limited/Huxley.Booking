using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.CarHires.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.CarHires;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.CarHires.Queries;

public class GetCarHireForBooking(
    ILogger<GetCarHireForBooking> logger,
    IValidator<GetCarHireForBookingRequest> validator,
    ICarHireRepository repository
    )
    : IRequestHandler<GetCarHireForBookingRequest, Result<GetCarHireForBookingResponse>>
{
    public async Task<Result<GetCarHireForBookingResponse>> Handle(GetCarHireForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetCarHireForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetCarHiresForFileNo(request.FileNumber);
            // return
            return GetCarHireForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting car hires for booking with file number {FileNo}", request.FileNumber);
            return GetCarHireForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetCarHireForBookingResponse
{
    public CarHire[] CarHires { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetCarHireForBookingResponse(bool successful, string[] failureReasons, CarHire[] carHires)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        CarHires = carHires;
    }

    public static GetCarHireForBookingResponse CreateSuccess(IEnumerable<CarHire> carHires) =>
        new GetCarHireForBookingResponse(true, [], carHires.ToArray());

    public static GetCarHireForBookingResponse CreateFailure(string[] reasons) =>
        new GetCarHireForBookingResponse(false, reasons, []);

    public static GetCarHireForBookingResponse CreateFailure(string reason) =>
        new GetCarHireForBookingResponse(false, [reason], []);

    public static GetCarHireForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetCarHireForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetCarHireForBookingRequest : IRequest<Result<GetCarHireForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetCarHireForBookingRequestValidator : AbstractValidator<GetCarHireForBookingRequest>
{
    public GetCarHireForBookingRequestValidator()
    {
    }
}