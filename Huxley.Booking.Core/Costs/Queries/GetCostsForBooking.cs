using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Costs.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Costs.Queries;

public class GetCostsForBooking(
    ILogger<GetCostsForBooking> logger, 
    IValidator<GetCostsForBookingRequest> validator,
    ICostRepository costRepository
    )
    : IRequestHandler<GetCostsForBookingRequest, Result<GetCostsForBookingResponse>>
{
    public async Task<Result<GetCostsForBookingResponse>> Handle(GetCostsForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetCostsForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the costs
            var costs = await costRepository.GetCostsForFileNo(request.FileNumber);

            // return
            return GetCostsForBookingResponse.CreateSuccess(costs);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed Getting Costs for Booking with File Number {FileNo}", request.FileNumber);
            return GetCostsForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetCostsForBookingResponse
{
    public Cost[] Costs { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetCostsForBookingResponse(bool successful, string[] failureReasons, Cost[] costs)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Costs = costs;
    }

    public static GetCostsForBookingResponse CreateSuccess(IEnumerable<Cost> costs) =>
        new GetCostsForBookingResponse(true, [], costs.ToArray());

    public static GetCostsForBookingResponse CreateFailure(string[] reasons) =>
        new GetCostsForBookingResponse(false, reasons, []);

    public static GetCostsForBookingResponse CreateFailure(string reason) =>
        new GetCostsForBookingResponse(false, [reason], []);

    public static GetCostsForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetCostsForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetCostsForBookingRequest : IRequest<Result<GetCostsForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetCostsForBookingRequestValidator : AbstractValidator<GetCostsForBookingRequest>
{
    public GetCostsForBookingRequestValidator()
    {
    }
}