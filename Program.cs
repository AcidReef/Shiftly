using Shiftly.Repositories;
using Shiftly.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Azure.Storage.Blobs;
using Azure.Messaging.ServiceBus;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// === KONFIGURACJA MONGODB ===
builder.Services.Configure<Shiftly.Data.MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<Shiftly.Data.MongoDbContext>();

// === REPOZYTORIA I SERWISY ===
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBus"));

builder.Services.AddSingleton<NotificationService>();
builder.Services.AddSingleton(sp => new BlobServiceClient(builder.Configuration.GetValue<string>("AzureStorage:ConnectionString")));
builder.Services.AddSingleton(sp => new ServiceBusClient(builder.Configuration.GetValue<string>("ServiceBus:ConnectionString")));
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


app.UseSwagger();
app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;  // Swagger dostępny pod głównym adresem
    });


app.UseHttpsRedirection();

// === MAPOWANIE KONTROLERÓW I AUTORYZACJA ===
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();