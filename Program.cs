using Shiftly.Repositories;

var builder = WebApplication.CreateBuilder(args);

// === 1. KONFIGURACJA MONGODB ===
builder.Services.Configure<Shiftly.Data.MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<Shiftly.Data.MongoDbContext>();

// === 2. REPOZYTORIA I SERWISY (dodawać tu młotki!!) ===
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ShiftRepository>();

// === 3. SWAGGER I ENDPOINTY ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === 4. MIDDLEWARE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === 5. MAPOWANIE KONTROLERÓW ===
app.MapControllers();

// (możesz usunąć domyślny endpoint pogodowy, jeśli go nie potrzebujesz)

app.Run();