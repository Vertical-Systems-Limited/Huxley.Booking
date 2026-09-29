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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Huxley.Booking.WebApi.Serialization;

// Huxley.Itinerary.Domain is pinned at 1.4.1. That version's own ItineraryContractResolver /
// ItineraryItemDtoConverter only match CanConvert(objectType == typeof(ItineraryItemDto)) - the exact
// abstract base type. Newtonsoft resolves an array element's contract by *runtime* type on write, so
// WriteJson never fires for an actual AccommodationDto/FlightDto/etc. instance and "$type" is never
// emitted. Later versions of the library fix this (matching subtypes via IsAssignableFrom), but upgrading
// past 1.4.1 pulls in unrelated breaking DTO changes we're not ready for. This reimplements just the
// write-side (and matching read-side) fix locally, without touching the package version. Delete this
// file and revert Program.cs once Huxley.Itinerary.Domain is upgraded and its own fix is in use.
public class PatchedItineraryContractResolver(Type? exclude = null) : DefaultContractResolver
{
    protected override JsonContract CreateContract(Type objectType)
    {
        var contract = base.CreateContract(objectType);
        if (objectType != exclude && typeof(ItineraryItemDto).IsAssignableFrom(objectType))
            contract.Converter = new ItineraryItemDtoTypeDiscriminatorConverter();
        return contract;
    }
}

public class ItineraryItemDtoTypeDiscriminatorConverter : JsonConverter
{
    // Mirrors the discriminator strings used by Huxley.Itinerary.Domain 1.4.1's own TypeMap.
    private static readonly Dictionary<string, Type> TypeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["flight"] = typeof(FlightDto),
        ["transfer"] = typeof(TransferDto),
        ["cruise"] = typeof(CruiseDto),
        ["accommodation"] = typeof(AccommodationDto),
        ["carHire"] = typeof(CarHireDto),
        ["excursion"] = typeof(ExcursionDto),
        ["extraNote"] = typeof(ExtraNoteDto),
        ["ferry"] = typeof(FerryDto),
        ["parking"] = typeof(ParkingDto),
        ["rail"] = typeof(RailDto),
        ["restaurant"] = typeof(RestaurantDto),
        ["surfaceSector"] = typeof(SurfaceSectorDto),
        ["transport"] = typeof(TransportDto),
    };

    private static readonly Dictionary<Type, string> ReverseTypeMap =
        TypeMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    // Matches the abstract base AND every concrete subtype - see the note above on why that's required
    // for WriteJson to actually fire.
    public override bool CanConvert(Type objectType) => typeof(ItineraryItemDto).IsAssignableFrom(objectType);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not ItineraryItemDto dto) { writer.WriteNull(); return; }

        var type = dto.GetType();
        var innerSerializer = CreateInnerSerializer(serializer, exclude: type);
        var jo = JObject.FromObject(dto, innerSerializer);
        if (ReverseTypeMap.TryGetValue(type, out var discriminator))
            jo.AddFirst(new JProperty("$type", discriminator));
        jo.WriteTo(writer);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var discriminator = jo["$type"]?.Value<string>();
        if (discriminator == null || !TypeMap.TryGetValue(discriminator, out var concreteType))
            throw new JsonSerializationException($"Unknown or missing itinerary item '$type': '{discriminator}'");

        var innerSerializer = CreateInnerSerializer(serializer, exclude: concreteType);
        return jo.ToObject(concreteType, innerSerializer);
    }

    // Because the converter is attached directly to the type's JsonContract, Newtonsoft calls it
    // unconditionally for that type once matched - it does not re-check CanConvert. So naively reusing
    // the ambient (resolver-carrying) serializer inside ReadJson/WriteJson would recurse forever. Instead
    // build a fresh serializer whose resolver behaves identically except it skips attaching the converter
    // for the one exact type currently being unwrapped, which breaks the self-recursion while still
    // converting everything nested underneath correctly.
    private static JsonSerializer CreateInnerSerializer(JsonSerializer ambient, Type exclude)
    {
        var inner = new JsonSerializer
        {
            ContractResolver = new PatchedItineraryContractResolver(exclude),
            DateFormatHandling = ambient.DateFormatHandling,
            DateTimeZoneHandling = ambient.DateTimeZoneHandling,
            NullValueHandling = ambient.NullValueHandling,
            DefaultValueHandling = ambient.DefaultValueHandling,
            Culture = ambient.Culture
        };
        foreach (var converter in ambient.Converters)
            inner.Converters.Add(converter);
        return inner;
    }
}
