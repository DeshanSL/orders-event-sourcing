using Marten;
using Orders.Api.Database.EventSourcing;

namespace Orders.Api;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.CustomSchemaIds(a => a.FullName?.Replace("+", "."));
        });

        builder.Services.AddMarten(options =>
        {
            options.Connection(builder.Configuration.GetConnectionString("DefaultConnection")!);
        });

        builder.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        builder.Services.AddScoped<IAppEventStore, EventStore>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseSliceEndpoints(options =>
        {
            options.RegisterEndpointsFromAssembly(typeof(Program).Assembly);
        });

        app.Run();
    }
}
