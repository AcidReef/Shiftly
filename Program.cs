using Shiftly.Repositories;
using Shiftly.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Azure.Storage.Blobs;
using Azure.Messaging.ServiceBus;
using MongoDB.Driver;
using System.Text;
using Shiftly.Models;

using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// === KONFIGURACJA MONGODB ===
builder.Services.Configure<Shiftly.Data.MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<Shiftly.Data.MongoDbContext>();

// Tworzenie połączenia z MongoDB i kolekcją
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDb").Get<Shiftly.Data.MongoDbSettings>()!;
    return new MongoClient(settings.ConnectionString);
});

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

// var mongoClient = app.Services.GetRequiredService<IMongoClient>();
// // var database = mongoClient.GetDatabase("shiftly"); 
// // var collection = database.GetCollection<Shift>("shifts");//

// // Asynchroniczne wstawienie testowych danych
// await collection.InsertManyAsync(shifts);
// Console.WriteLine("Testowe shiftsy zostały dodane do bazy!");

// // Wstawienie testowego użytkownika do bazy
// await collection.InsertOneAsync(user);
// Console.WriteLine("Testowy użytkownik został dodany do bazy!");

// === SWAGGER I UI ===
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;  // Swagger dostępny pod głównym adresem
});

app.UseHttpsRedirection();

// === MAPOWANIE KONTROLERÓW I AUTORYZACJA ===
app.MapControllers();

await app.RunAsync();

app.Run();
