using Shiftly.Repositories;
using Shiftly.Services;

var builder = WebApplication.CreateBuilder(args);

// === KONFIGURACJA MONGODB ===
builder.Services.Configure<Shiftly.Data.MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<Shiftly.Data.MongoDbContext>();

// === REPOZYTORIA I SERWISY ===
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBus"));
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddScoped<LeaveRequestService>();
builder.Services.AddScoped<ShiftService>();
builder.Services.AddScoped<SwapRequestRepository>();
builder.Services.AddScoped<LeaveRequestRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ShiftRepository>();

// === SWAGGER I ENDPOINTY ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === MIDDLEWARE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === MAPOWANIE KONTROLERÓW ===
app.MapControllers();

app.Run();