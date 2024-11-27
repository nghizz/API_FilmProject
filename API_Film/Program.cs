using API_Film.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình JSON Serializer để xử lý vòng lặp
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64; // Tăng chiều sâu nếu cần thiết
});

// Thêm dịch vụ vào container
builder.Services.AddControllers();

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Lưu Session trong bộ nhớ
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian sống của Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Domain Angular Frontend
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Cấu hình kết nối MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FilmDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .EnableDetailedErrors()
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging());  // Bật logging dữ liệu nhạy cảm nếu cần

// Cấu hình Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Film Project",
        Version = "v1",
        Description = "API cho hệ thống phim",
    });
});

var app = builder.Build();

// Cấu hình Logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Kiểm tra kết nối MySQL
try
{
    using var connection = new MySqlConnection(connectionString);
    connection.Open();
    logger.LogInformation("Connect suscess!");
}
catch (MySqlException ex)
{
    logger.LogError($"Fail connect MySQL: {ex.Message}");
}
catch (Exception ex)
{
    logger.LogError($"Fail connect MySQL: {ex.Message}");
}

// Cấu hình HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Hiển thị lỗi chi tiết trong môi trường phát triển
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // Sử dụng cấu hình CORS
app.UseHttpsRedirection();

// Sử dụng Session Middleware
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();