using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Itinerary.Accommodations.Data;
using Huxley.Booking.Core.Itinerary.CarHires.Data;
using Huxley.Booking.Core.Itinerary.Cruises;
using Huxley.Booking.Core.Itinerary.Excursions.Data;
using Huxley.Booking.Core.Itinerary.ExtraNotes.Data;
using Huxley.Booking.Core.Itinerary.Ferries.Data;
using Huxley.Booking.Core.Itinerary.Flights.Data;
using Huxley.Booking.Core.Itinerary.Parkings.Data;
using Huxley.Booking.Core.Itinerary.Rails.Data;
using Huxley.Booking.Core.Itinerary.Restaurants.Data;
using Huxley.Booking.Core.Itinerary.SurfaceSectors.Data;
using Huxley.Booking.Core.Itinerary.Transfers.Data;
using Huxley.Booking.Core.Itinerary.Transports.Data;
using Huxley.Core;
using Huxley.Itinerary.Domain;
using Huxley.Itinerary.Domain.CarHires.Contracts;
using Huxley.Itinerary.Domain.Contracts;
using Huxley.Itinerary.Domain.Cruises.Contracts;
using Huxley.Itinerary.Domain.Excursions.Contracts;
using Huxley.Itinerary.Domain.ExtraNotes.Contracts;
using Huxley.Itinerary.Domain.Ferries.Contracts;
using Huxley.Itinerary.Domain.Flights.Contracts;
using Huxley.Itinerary.Domain.Parkings.Contracts;
using Huxley.Itinerary.Domain.Rails.Contracts;
using Huxley.Itinerary.Domain.Restaurants.Contracts;
using Huxley.Itinerary.Domain.SurfaceSectors.Contracts;
using Huxley.Itinerary.Domain.Transfers.Contracts;
using Huxley.Itinerary.Domain.Transports.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Itinerary.BookingItineraries.Queries;

public class GetItineraryForBooking(
    ILogger<GetItineraryForBooking> logger,
    IValidator<GetItineraryForBookingRequest> validator,
    IAccommodationRepository accommodationRepository,
    ICarHireRepository carHireRepository,
    ICruiseService cruiseService,
    IExcursionRepository excursionRepository,
    IExtraNoteRepository extraNoteRepository,
    IFerryRepository ferryRepository,
    IFlightRepository flightRepository,
    IParkingRepository parkingRepository,
    IRailRepository railRepository,
    IRestaurantRepository restaurantRepository,
    ISurfaceSectorRepository surfaceSectorRepository,
    ITransferRepository transferRepository,
    ITransportRepository transportRepository
    )
    : IRequestHandler<GetItineraryForBookingRequest, Result<GetItineraryForBookingResponse>>
{
    public async Task<Result<GetItineraryForBookingResponse>> Handle(GetItineraryForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetItineraryForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // fetch all itinerary items in parallel
            var accommodationsTask = accommodationRepository.GetAccommodationsByFileNo(request.FileNumber);
            var carHiresTask = carHireRepository.GetCarHiresForFileNo(request.FileNumber);
            var cruisesTask = cruiseService.GetCruisesForFileNo(request.FileNumber);
            var excursionsTask = excursionRepository.GetExcursionsForFileNo(request.FileNumber);
            var extraNotesTask = extraNoteRepository.GetExtraNotesForFileNo(request.FileNumber);
            var ferriesTask = ferryRepository.GetFerriesForFileNo(request.FileNumber);
            var flightRecordsTask = flightRepository.GetFlightsByFileNo(request.FileNumber);
            var parkingsTask = parkingRepository.GetParkingForFileNo(request.FileNumber);
            var railsTask = railRepository.GetRailsForFileNo(request.FileNumber);
            var restaurantsTask = restaurantRepository.GetRestaurantsForFileNo(request.FileNumber);
            var surfaceSectorsTask = surfaceSectorRepository.GetSurfaceSectorsForFileNo(request.FileNumber);
            var transferRecordsTask = transferRepository.GetTransfersForFileNo(request.FileNumber);
            var transportsTask = transportRepository.GetTransportsForFileNo(request.FileNumber);

            // wait for all tasks to complete
            await Task.WhenAll(
                accommodationsTask,
                carHiresTask,
                cruisesTask,
                excursionsTask,
                extraNotesTask,
                ferriesTask,
                flightRecordsTask,
                parkingsTask,
                railsTask,
                restaurantsTask,
                surfaceSectorsTask,
                transferRecordsTask,
                transportsTask
            );

            // Get the records and map to domain models
            var flights = flightRecordsTask.Result.ToList();
            var accommodations = accommodationsTask.Result.ToList();
            var transfers = transferRecordsTask.Result.ToList();
            var cruises = cruisesTask.Result.ToList();
            var carHires = carHiresTask.Result.ToList();
            var excursions = excursionsTask.Result.ToList();
            var extraNotes = extraNotesTask.Result.ToList();
            var ferries = ferriesTask.Result.ToList();
            var parkings = parkingsTask.Result.ToList();
            var rails = railsTask.Result.ToList();
            var restaurants = restaurantsTask.Result.ToList();
            var surfaceSectors = surfaceSectorsTask.Result.ToList();
            var transports = transportsTask.Result.ToList();

            // Map domain models to DTOs
            var itinerary = new List<ItineraryItemDto>();

            itinerary.AddRange(flights.Select(f => f.MapToDto()).ToArray());
            itinerary.AddRange(accommodations.Select(a => a.MapToDto()).ToArray());
            itinerary.AddRange(transfers.Select(t => t.MapToDto()).ToArray());
            itinerary.AddRange(cruises.Select(c => c.MapToDto()).ToArray());
            itinerary.AddRange(carHires.Select(c => c.MapToDto()).ToArray());
            itinerary.AddRange(excursions.Select(e => e.MapToDto()).ToArray());
            itinerary.AddRange(extraNotes.Select(e => e.MapToDto()).ToArray());
            itinerary.AddRange(ferries.Select(f => f.MapToDto()).ToArray());
            itinerary.AddRange(parkings.Select(p => p.MapToDto()).ToArray());
            itinerary.AddRange(rails.Select(r => r.MapToDto()).ToArray());
            itinerary.AddRange(restaurants.Select(r => r.MapToDto()).ToArray());
            itinerary.AddRange(surfaceSectors.Select(s => s.MapToDto()).ToArray());
            itinerary.AddRange(transports.Select(t => t.MapToDto()).ToArray());

            return GetItineraryForBookingResponse.CreateSuccess(itinerary.ToArray());
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting itinerary for booking with file number {FileNo}", request.FileNumber);
            return GetItineraryForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetItineraryForBookingResponse
{
    public ItineraryItemDto[] Itinerary { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetItineraryForBookingResponse(bool successful, string[] failureReasons, ItineraryItemDto[] itinerary)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Itinerary = itinerary;
    }

    public static GetItineraryForBookingResponse CreateSuccess(ItineraryItemDto[] itinerary) =>
        new GetItineraryForBookingResponse(true, [], itinerary);

    public static GetItineraryForBookingResponse CreateFailure(string[] reasons) =>
        new GetItineraryForBookingResponse(false, reasons, null);

    public static GetItineraryForBookingResponse CreateFailure(string reason) =>
        new GetItineraryForBookingResponse(false, [reason], null);

    public static GetItineraryForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetItineraryForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());
}

public class GetItineraryForBookingRequest : IRequest<Result<GetItineraryForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetItineraryForBookingRequestValidator : AbstractValidator<GetItineraryForBookingRequest>
{
    public GetItineraryForBookingRequestValidator()
    {
        RuleFor(x => x.FileNumber).NotEmpty().WithMessage("File number must be provided.");
    }
}