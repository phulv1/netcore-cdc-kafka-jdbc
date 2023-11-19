/**
 * We need to install the NuGet package:
 * Microsoft.EntityFrameworkCore.Design
 * Npgsql.EntityFrameworkCore.PostgreSQL
 * MediatR
 */
using IdentityService.Data;
using IdentityService.Events;
using IdentityService.Events.Handlers;
using Microsoft.EntityFrameworkCore;
using SharedService;
using SharedService.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Kafka Consumer
builder.Services.AddKafkaConsumer<string, CustomerUpdatedEvent, CustomerUpdatedEventHandler>(p =>
{
    p.Topic = "customer_events";
    p.GroupId = "customer_events_identity_group";
    p.BootstrapServers = "localhost:9092";
});

// DBContext Postgres
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityDBContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Migrate Database
DbInitilializer.Migrate<IdentityDBContext>(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
