using Huxley.Hosting.Apis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Huxley.Booking.Core.Configuration;

public class BookingApiTenantSettings
{
    public Guid TenantId { get; set; }
    public bool PreLoadAnalysisCodes { get; set; }
}

public interface IBookingApiTenantSettingsProvider
{
    Task<BookingApiTenantSettings> GetSettingsForTenant(Guid tenantId);
    Task<IEnumerable<BookingApiTenantSettings>> GetSettingsForAllTenants();
}

public class BookingApiTenantSettingsProvider(
    IApiSettingsFactory settingsFactory,
    IOptions<BookingOptions> options,
    ILogger<BookingApiTenantSettingsProvider> logger
    )
    : IBookingApiTenantSettingsProvider
{
    public async Task<BookingApiTenantSettings> GetSettingsForTenant(Guid tenantId)
    {
        try
        {
            var properties = await settingsFactory.GetTenantSettings<BookingApiTenantSettings>(options.Value.ApiId, tenantId, (dictionary, settings) =>
            {
                settings.TenantId = tenantId;
                settings.PreLoadAnalysisCodes = dictionary.Get(BookingApiProperties.PreLoadAnalysisCodes).As<bool>();   
            });
            return properties;
        }
        catch (Exception e)
        {
            // need to fail gracefully here as this is called during startup,
            // and we don't want to block the API from starting.
            // ao just log the error and return default settings
            logger.LogError(e, "Error getting Booking API tenant settings for API {ApiId} Tenant {TenantId}", options.Value.ApiId, tenantId);
            return new BookingApiTenantSettings()
            {
                TenantId = tenantId,
                PreLoadAnalysisCodes = false
            };
        }
    }
    
    public async Task<IEnumerable<BookingApiTenantSettings>> GetSettingsForAllTenants()
    {
        var apiId = options.Value.ApiId;
        var settings = await settingsFactory.GetSettings<BookingApiTenantSettings>(apiId, (dict, instance) =>
        {
            instance.TenantId = dict.Get(BookingApiProperties.PreLoadAnalysisCodes).TenantId;
            instance.PreLoadAnalysisCodes = dict.Get(BookingApiProperties.PreLoadAnalysisCodes).As<bool>();
        });
        return settings;
    }

}

public static class BookingApiProperties
{
    public static string TenantId => "TenantId";
    public static string PreLoadAnalysisCodes => "PreLoadAnalysisCodes"; 
}

public static class DictionaryExtensions
{
    public static ApiProperty Get(this IDictionary<string, ApiProperty> dictionary, string key)
    {
        return dictionary.TryGetValue(key, out var property) ? property : null;
    }
}