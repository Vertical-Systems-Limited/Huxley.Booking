using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Bookings.Commands;

public class ConfirmBooking(
    ILogger<ConfirmBooking> logger, 
    IValidator<ConfirmBookingRequest> validator,
    IBookingRepository bookingRepository
    )
    : IRequestHandler<ConfirmBookingRequest, Result<ConfirmBookingResponse>>
{
    public async Task<Result<ConfirmBookingResponse>> Handle(ConfirmBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return ConfirmBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            var result = await bookingRepository.ConfirmBooking(request.FileNumber);
            return !result 
                ? ConfirmBookingResponse.CreateFailure($"Failed to confirm booking with FileNumber: {request.FileNumber}") 
                : ConfirmBookingResponse.CreateSuccess();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed Confirming Booking with FileNumber: {FileNumber}", request.FileNumber);
            return ConfirmBookingResponse.CreateFailure(e);
        }
    }
}

public class ConfirmBookingResponse
{
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private ConfirmBookingResponse(bool successful, string[] failureReasons)
    {
        Successful = successful;
        FailureReasons = failureReasons;
    }

    public static ConfirmBookingResponse CreateSuccess() =>
        new ConfirmBookingResponse(true, []);

    public static ConfirmBookingResponse CreateFailure(string[] reasons) =>
        new ConfirmBookingResponse(false, reasons);

    public static ConfirmBookingResponse CreateFailure(string reason) =>
        new ConfirmBookingResponse(false, [reason]);

    public static ConfirmBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static ConfirmBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class ConfirmBookingRequest : IRequest<Result<ConfirmBookingResponse>>
{
    public string FileNumber { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global
    public ConfirmBookingRequest()
    {
        //for serialization
    }
    public static ConfirmBookingRequest Create(string fileNumber) =>
        new ConfirmBookingRequest() { FileNumber = fileNumber };
}

public class ConfirmBookingRequestValidator : AbstractValidator<ConfirmBookingRequest>
{
    public ConfirmBookingRequestValidator()
    {
        RuleFor(x => x.FileNumber)
            .NotEmpty().WithMessage("FileNumber is required.");

    }
}