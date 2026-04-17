using BookingTrain.Domain.Interfaces;
using BookingTrain.Infrastructure.Persistence;
using BookingTrain.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingTrain.Application.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

            // Đăng ký Auth Service
            builder.Services.AddScoped<IAuthService, AuthService>();

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
             });

            // ── CẤU HÌNH JWT AUTHENTICATION ──────────────────────────────────
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey   = jwtSettings["SecretKey"]!;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = jwtSettings["Issuer"],
                    ValidAudience            = jwtSettings["Audience"],
                    IssuerSigningKey         = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                                                  System.Text.Encoding.UTF8.GetBytes(secretKey))
                };
            });

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Swagger hỗ trợ nhập Bearer token để test các endpoint [Authorize]
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title   = "BookingTrain API",
                    Version = "v1"
                });

                // Thêm nút "Authorize" trên Swagger UI
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name         = "Authorization",
                    Type         = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme       = "Bearer",
                    BearerFormat = "JWT",
                    In           = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description  = "Nhập JWT token vào đây. Ví dụ: eyJhbGci..."
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}