using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Transports.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Transports;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Transports.Queries;

public class GetTransportForBooking(
    ILogger<GetTransportForBooking> logger,
    IValidator<GetTransportForBookingRequest> validator,
    ITransportRepository repository
    )
    : IRequestHandler<GetTransportForBookingRequest, Result<GetTransportForBookingResponse>>
{
    public async Task<Result<GetTransportForBookingResponse>> Handle(GetTransportForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetTransportForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetTransportsForFileNo(request.FileNumber);
            // return
            return GetTransportForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting transports for booking with file number {FileNo}", request.FileNumber);
            return GetTransportForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetTransportForBookingResponse
{
    public Transport[] Transports { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetTransportForBookingResponse(bool successful, string[] failureReasons, Transport[] transports)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Transports = transports;
    }

    public static GetTransportForBookingResponse CreateSuccess(IEnumerable<Transport> transports) =>
        new GetTransportForBookingResponse(true, [], transports.ToArray());

    public static GetTransportForBookingResponse CreateFailure(string[] reasons) =>
        new GetTransportForBookingResponse(false, reasons, []);

    public static GetTransportForBookingResponse CreateFailure(string reason) =>
        new GetTransportForBookingResponse(false, [reason], []);

    public static GetTransportForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetTransportForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetTransportForBookingRequest : IRequest<Result<GetTransportForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetTransportForBookingRequestValidator : AbstractValidator<GetTransportForBookingRequest>
{
    public GetTransportForBookingRequestValidator()
    {
    }
}