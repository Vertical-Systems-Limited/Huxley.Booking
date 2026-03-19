using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Transfers.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Transfers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Transfers.Queries;

public class GetTransferForBooking(
    ILogger<GetTransferForBooking> logger,
    IValidator<GetTransferForBookingRequest> validator,
    ITransferRepository repository
    )
    : IRequestHandler<GetTransferForBookingRequest, Result<GetTransferForBookingResponse>>
{
    public async Task<Result<GetTransferForBookingResponse>> Handle(GetTransferForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetTransferForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetTransfersForFileNo(request.FileNumber);
            // return
            return GetTransferForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting transfers for booking with file number {FileNo}", request.FileNumber);
            return GetTransferForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetTransferForBookingResponse
{
    public Transfer[] Transfers { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetTransferForBookingResponse(bool successful, string[] failureReasons, Transfer[] transfers)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Transfers = transfers;
    }

    public static GetTransferForBookingResponse CreateSuccess(IEnumerable<Transfer> transfers) =>
        new GetTransferForBookingResponse(true, [], transfers.ToArray());

    public static GetTransferForBookingResponse CreateFailure(string[] reasons) =>
        new GetTransferForBookingResponse(false, reasons, []);

    public static GetTransferForBookingResponse CreateFailure(string reason) =>
        new GetTransferForBookingResponse(false, [reason], []);

    public static GetTransferForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetTransferForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetTransferForBookingRequest : IRequest<Result<GetTransferForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetTransferForBookingRequestValidator : AbstractValidator<GetTransferForBookingRequest>
{
    public GetTransferForBookingRequestValidator()
    {
    }
}