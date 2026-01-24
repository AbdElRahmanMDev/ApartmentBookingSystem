using API.Extensions;
using Application;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration) // read from appsettings.json
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UseSerilogRequestLogging(); // logs HTTP requests automatically


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    app.SeedData();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UserRequestContextLogging();



app.Run();

