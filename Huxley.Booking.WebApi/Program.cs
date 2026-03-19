using Huxley.Booking.Core.Configuration;
using Huxley.Hosting.Configuration;

namespace Huxley.Booking.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddBookingCore(builder.Configuration);
        builder.Services.AddBookingCoreLogging("Booking.WebApi");

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseHuxleyWebHosting();   

        app.MapControllers();

        app.Run();
    }
}