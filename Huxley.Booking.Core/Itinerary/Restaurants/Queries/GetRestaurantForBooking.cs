using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Restaurants.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain.Restaurants;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.Restaurants.Queries;

public class GetRestaurantForBooking(
    ILogger<GetRestaurantForBooking> logger,
    IValidator<GetRestaurantForBookingRequest> validator,
    IRestaurantRepository repository
    )
    : IRequestHandler<GetRestaurantForBookingRequest, Result<GetRestaurantForBookingResponse>>
{
    public async Task<Result<GetRestaurantForBookingResponse>> Handle(GetRestaurantForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetRestaurantForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the list
            var list = await repository.GetRestaurantsForFileNo(request.FileNumber);
            // return
            return GetRestaurantForBookingResponse.CreateSuccess(list);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting restaurants for booking with file number {FileNo}", request.FileNumber);
            return GetRestaurantForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetRestaurantForBookingResponse
{
    public Restaurant[] Restaurants { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetRestaurantForBookingResponse(bool successful, string[] failureReasons, Restaurant[] restaurants)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Restaurants = restaurants;
    }

    public static GetRestaurantForBookingResponse CreateSuccess(IEnumerable<Restaurant> restaurants) =>
        new GetRestaurantForBookingResponse(true, [], restaurants.ToArray());

    public static GetRestaurantForBookingResponse CreateFailure(string[] reasons) =>
        new GetRestaurantForBookingResponse(false, reasons, []);

    public static GetRestaurantForBookingResponse CreateFailure(string reason) =>
        new GetRestaurantForBookingResponse(false, [reason], []);

    public static GetRestaurantForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetRestaurantForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetRestaurantForBookingRequest : IRequest<Result<GetRestaurantForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetRestaurantForBookingRequestValidator : AbstractValidator<GetRestaurantForBookingRequest>
{
    public GetRestaurantForBookingRequestValidator()
    {
    }
}