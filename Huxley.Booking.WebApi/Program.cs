using Huxley.Booking.Core.Configuration;
using Huxley.Hosting.Configuration;
using Huxley.Itinerary.Domain;
using Huxley.Metering.Core.Configuration;


namespace Huxley.Booking.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new ItineraryContractResolver();
            })
            ;
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        
        builder.Services.AddHuxleyWebHosting(builder.Configuration.GetSection("HostingConfiguration"));
        builder.Services.AddHuxleyMetering(builder.Configuration.GetSection("MeteringOptions"));
        builder.Services.AddMeteringMessaging();
      
        builder.Services.AddBookingCore(builder.Configuration);
        builder.Services.AddBookingCoreLogging("Booking.WebApi");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Server-Name", Environment.MachineName);
            await next();
        });
        app.UseHuxleyMetering();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseHuxleyWebHosting();   
        app.MapControllers();
        app.Run();
    }
}