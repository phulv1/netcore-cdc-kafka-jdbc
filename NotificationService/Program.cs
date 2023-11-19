/**
 * We need to install the NuGet package:
 * Microsoft.EntityFrameworkCore.Design
 * Npgsql.EntityFrameworkCore.PostgreSQL
 * MediatR
 */
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Events;
using NotificationService.Events.Handlers;
using SharedService;
using SharedService.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DBContext Postgres
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NotificationDBContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add Kafka Consumer
builder.Services.AddKafkaConsumer<string, UserCreatedEvent, UserCreatedEventHandler>(p =>
{
    p.Topic = "user_events";
    p.GroupId = "user_events_notification_group";
    p.BootstrapServers = "localhost:9092";
});

builder.Services.AddKafkaConsumer<string, CustomerUpdatedEvent, CustomerUpdatedEventHandler>(p =>
{
    p.Topic = "customer_events";
    p.GroupId = "customer_events_notification_group";
    p.BootstrapServers = "localhost:9092";
});

var app = builder.Build();

// Migrate Database
DbInitilializer.Migrate<NotificationDBContext>(app.Services);

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
