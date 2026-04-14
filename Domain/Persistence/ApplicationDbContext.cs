using Microsoft.EntityFrameworkCore;
using BookingTrain.Domain.Entities;

namespace BookingTrain.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng Route (Quan hệ phức tạp: 1 Route có 2 Station)
            modelBuilder.Entity<Route>(entity =>
            {
                // Cấu hình cho Ga đi
                entity.HasOne(r => r.FromStation)
                      .WithMany()
                      .HasForeignKey(r => r.FromStationId)
                      .OnDelete(DeleteBehavior.Restrict); // Không cho xóa Ga nếu có tuyến đang dùng

                // Cấu hình cho Ga đến
                entity.HasOne(r => r.ToStation)
                      .WithMany()
                      .HasForeignKey(r => r.ToStationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 2. Cấu hình bảng Ticket (Quan hệ 1-1 với Payment)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Payment)
                .WithOne(p => p.Ticket)
                .HasForeignKey<Payment>(p => p.TicketId);

            // 3. Cấu hình bảng Seat (Một tàu có nhiều ghế)
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Train)
                .WithMany(t => t.Seats)
                .HasForeignKey(s => s.TrainId)
                .OnDelete(DeleteBehavior.Cascade); // Nếu xóa tàu thì tự động xóa hết ghế của tàu đó

            // 4. Các ràng buộc dữ liệu (Fluent API)
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique(); // Email không được trùng lặp
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            });

            // 5. Cấu hình cho số tiền trong bảng Payment (Hết cảnh báo vàng)
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // 6. GIẢI QUYẾT LỖI XÓA DÂY CHUYỀN (Multiple Cascade Paths) CHO BẢNG TICKET
            modelBuilder.Entity<Ticket>(entity =>
            {
                // Chặn xóa Lịch trình nếu đã có vé
                entity.HasOne(t => t.Schedule)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(t => t.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Chặn xóa Ghế nếu ghế đã được mua vé
                entity.HasOne(t => t.Seat)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(t => t.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Chặn xóa User nếu User đó có lịch sử mua vé
                entity.HasOne(t => t.User)
                      .WithMany(u => u.Tickets)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}