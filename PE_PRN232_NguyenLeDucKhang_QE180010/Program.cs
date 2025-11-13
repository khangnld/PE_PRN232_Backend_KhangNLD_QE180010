using DotNetEnv;
using MongoDB.Driver;
using PE_PRN232_NguyenLeDucKhang_QE180010.Middleware;
using PE_PRN232_NguyenLeDucKhang_QE180010.Repositories;
using PE_PRN232_NguyenLeDucKhang_QE180010.Services;

// Load .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB config
var mongoConnectionString = builder.Configuration["MONGODB_CONNECTION_STRING"]
    ?? Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
    ?? throw new Exception("MONGODB_CONNECTION_STRING is not configured");

var mongoDatabaseName = builder.Configuration["MONGODB_DATABASE_NAME"]
    ?? Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
    ?? throw new Exception("MONGODB_DATABASE_NAME is not configured");

var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

// CORS config
var allowedOrigins = builder.Configuration["ALLOWED_ORIGINS"]
    ?? Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")
    ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins.Split(','))
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ISupabaseService, SupabaseService>();
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// ✅ Luôn bật Swagger (hoặc tùy môi trường)
app.UseSwagger();
app.UseSwaggerUI();

// ❌ Không nên redirect HTTPS trong Render container
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowFrontend");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

// ✅ Dùng PORT động
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
