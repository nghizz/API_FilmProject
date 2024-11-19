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

// Cấu hình kết nối MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FilmDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .EnableDetailedErrors()
           .LogTo(Console.WriteLine, LogLevel.Information));

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

// Kiểm tra kết nối MySQL (tuỳ chọn)
try
{
    using var connection = new MySqlConnection(connectionString);
    connection.Open();
    logger.LogInformation("Connection successful!");
}
catch (Exception ex)
{
    logger.LogError($"Error connecting to MySQL: {ex.Message}");
}

// Cấu hình HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Hiển thị lỗi chi tiết trong môi trường phát triển
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
