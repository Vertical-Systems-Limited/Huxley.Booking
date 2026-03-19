using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.SurfaceSectors.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.SurfaceSectors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.SurfaceSectors.Queries;

public class GetSurfaceSectorForBooking(
    ILogger<GetSurfaceSectorForBooking> logger,
    IValidator<GetSurfaceSectorForBookingRequest> validator,
    ISurfaceSectorRepository repository
    )
    : IRequestHandler<GetSurfaceSectorForBookingRequest, Result<GetSurfaceSectorForBookingResponse>>
{
    public async Task<Result<GetSurfaceSectorForBookingResponse>> Handle(GetSurfaceSectorForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetSurfaceSectorForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetSurfaceSectorsForFileNo(request.FileNumber);
            // return
            return GetSurfaceSectorForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting surface sectors for booking with file number {FileNo}", request.FileNumber);
            return GetSurfaceSectorForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetSurfaceSectorForBookingResponse
{
    public SurfaceSector[] SurfaceSectors { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetSurfaceSectorForBookingResponse(bool successful, string[] failureReasons, SurfaceSector[] surfaceSectors)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        SurfaceSectors = surfaceSectors;
    }

    public static GetSurfaceSectorForBookingResponse CreateSuccess(IEnumerable<SurfaceSector> surfaceSectors) =>
        new GetSurfaceSectorForBookingResponse(true, [], surfaceSectors.ToArray());

    public static GetSurfaceSectorForBookingResponse CreateFailure(string[] reasons) =>
        new GetSurfaceSectorForBookingResponse(false, reasons, []);

    public static GetSurfaceSectorForBookingResponse CreateFailure(string reason) =>
        new GetSurfaceSectorForBookingResponse(false, [reason], []);

    public static GetSurfaceSectorForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetSurfaceSectorForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetSurfaceSectorForBookingRequest : IRequest<Result<GetSurfaceSectorForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetSurfaceSectorForBookingRequestValidator : AbstractValidator<GetSurfaceSectorForBookingRequest>
{
    public GetSurfaceSectorForBookingRequestValidator()
    {
    }
}