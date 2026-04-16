using BookingTrain.Domain.Interfaces;
using BookingTrain.Infrastructure.Persistence;
using BookingTrain.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingTrain.Application.Service;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Lấy chuỗi kết nối từ appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Đăng ký ApplicationDbContext vào hệ thống Dependency Injection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Đăng ký các Repository và UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Đăng ký tất cả Services
            builder.Services.AddScoped<ITrainService,    TrainService>();
            builder.Services.AddScoped<IUserService,     UserService>();
            builder.Services.AddScoped<IStationService,  StationService>();
            builder.Services.AddScoped<ISeatTypeService, SeatTypeService>();
            builder.Services.AddScoped<ISeatService,     SeatService>();
            builder.Services.AddScoped<IRouteService,    RouteService>();
            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddScoped<ITicketService,   TicketService>();
            builder.Services.AddScoped<IPaymentService,  PaymentService>();


            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                // 1. TẮT LỖI POST: Không bắt buộc nhập các trường liên kết (như User, Ticket, Seats...) khi tạo mới
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
               .AddJsonOptions(options =>
              {
               // 2. TẮT LỖI GET: Chống lỗi "vòng lặp vô tận" khi gọi API lấy danh sách (User gọi Ticket, Ticket lại gọi ngược User)
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
             } );

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