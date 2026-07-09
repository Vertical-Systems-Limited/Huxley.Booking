using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Bookings.Contracts;
using Huxley.Booking.Core.Bookings.Data;
using Huxley.Booking.Core.Costs.Data;
using Huxley.Booking.Core.Passengers.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Bookings.Commands;

public class CreateBooking(
    ILogger<CreateBooking> logger, 
    IValidator<CreateBookingRequest> validator,
    IBookingRepository bookingRepository,
    ICostRepository costRepository,
    IPassengerRepository passengerRepository
    )
    : IRequestHandler<CreateBookingRequest, Result<CreateBookingResponse>>
{
    public async Task<Result<CreateBookingResponse>> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return CreateBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // insert the booking first
            var newFileNumber = await bookingRepository.InsertBooking(request.Booking);
            // insert the costs
            await costRepository.InsertCosts(request.Booking, newFileNumber);
            // insert the passengers
            await passengerRepository.InsertPassengers(request.Booking, newFileNumber);
            // return
            return CreateBookingResponse.CreateSuccess(newFileNumber);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed Creating Booking");
            return CreateBookingResponse.CreateFailure(e);
        }
    }
}

public class CreateBookingResponse
{
    public string FileNumber { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private CreateBookingResponse(bool successful, string[] failureReasons, string fileNumber)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        FileNumber = fileNumber;
    }

    public static CreateBookingResponse CreateSuccess(string fileNumber) =>
        new CreateBookingResponse(true, [], fileNumber);

    public static CreateBookingResponse CreateFailure(string[] reasons) =>
        new CreateBookingResponse(false, reasons, null);

    public static CreateBookingResponse CreateFailure(string reason) =>
        new CreateBookingResponse(false, [reason], null);

    public static CreateBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static CreateBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class CreateBookingRequest : IRequest<Result<CreateBookingResponse>>
{
    public CreateBookingDto Booking { get; set; }
}

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
}