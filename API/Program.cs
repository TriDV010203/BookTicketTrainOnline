using BookingTrain.Domain.Interfaces;
using BookingTrain.Infrastructure.Persistence;
using BookingTrain.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Lấy chuỗi kết nối từ appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // 2. Đăng ký ApplicationDbContext vào hệ thống Dependency Injection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 3. Đăng ký các Repository và UnitOfWork (Để API có thể gọi dùng)
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
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
        }
    }
}
