using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Bookings.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Bookings.Queries;

public class GetBookingByFileNo(
    ILogger<GetBookingByFileNo> logger, 
    IValidator<GetBookingByFileNoRequest> validator,
    IBookingRepository bookingRepository
    )
    : IRequestHandler<GetBookingByFileNoRequest, Result<GetBookingByFileNoResponse>>
{
    public async Task<Result<GetBookingByFileNoResponse>> Handle(GetBookingByFileNoRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetBookingByFileNoResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the booking
            var booking = await bookingRepository.GetBookingByFileNo(request.FileNumber);
            
            // return
            return GetBookingByFileNoResponse.CreateSuccess(booking);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed Getting Booking by File Number {FileNo}", request.FileNumber);
            return GetBookingByFileNoResponse.CreateFailure(e);
        }
    }
}

public class GetBookingByFileNoResponse
{
    public Booking Booking { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetBookingByFileNoResponse(bool successful, string[] failureReasons, Booking booking)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Booking = booking;
    }

    public static GetBookingByFileNoResponse CreateSuccess(Booking booking) =>
        new GetBookingByFileNoResponse(true, [], booking);

    public static GetBookingByFileNoResponse CreateFailure(string[] reasons) =>
        new GetBookingByFileNoResponse(false, reasons, null);

    public static GetBookingByFileNoResponse CreateFailure(string reason) =>
        new GetBookingByFileNoResponse(false, [reason], null);

    public static GetBookingByFileNoResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetBookingByFileNoResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetBookingByFileNoRequest : IRequest<Result<GetBookingByFileNoResponse>>
{
    public string FileNumber { get; set; }
}

public class GetBookingByFileNoRequestValidator : AbstractValidator<GetBookingByFileNoRequest>
{
    public GetBookingByFileNoRequestValidator()
    {
        RuleFor(x => x.FileNumber).NotEmpty().WithMessage("File number must be provided.");
    }
}