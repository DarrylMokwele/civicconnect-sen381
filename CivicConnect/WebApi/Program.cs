using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions;
using Application.Notifications;
using Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CivicConnectDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CivicConnectDatabase")));

builder.Services.AddScoped<
    INotificationService,
    NotificationService>();

builder.Services.AddScoped<
    ServiceRequestStatusChangedHandler>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
