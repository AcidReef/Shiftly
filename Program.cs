using Shiftly.Repositories;
using Shiftly.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// === JWT ===
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// === MIDDLEWARE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === MAPOWANIE KONTROLERÓW I AUTORYZACJA ===
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();