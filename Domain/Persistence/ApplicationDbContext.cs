using Microsoft.EntityFrameworkCore;
using BookingTrain.Domain.Entities;

namespace BookingTrain.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User>      Users     { get; set; }
        public DbSet<Train>     Trains    { get; set; }
        public DbSet<Station>   Stations  { get; set; }
        public DbSet<Route>     Routes    { get; set; }
        public DbSet<Schedule>  Schedules { get; set; }
        public DbSet<SeatType>  SeatTypes { get; set; }
        public DbSet<Seat>      Seats     { get; set; }
        public DbSet<Ticket>    Tickets   { get; set; }
        public DbSet<Payment>   Payments  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng Route (1 Route có 2 Station)
            modelBuilder.Entity<Route>(entity =>
            {
                entity.HasOne(r => r.FromStation)
                      .WithMany()
                      .HasForeignKey(r => r.FromStationId)
                      .OnDelete(DeleteBehavior.Restrict);

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

            // 3. Cấu hình bảng Seat (FK Train + FK SeatType + UQ SeatNumber)
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasOne(s => s.Train)
                      .WithMany(t => t.Seats)
                      .HasForeignKey(s => s.TrainId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.SeatType)
                      .WithMany(st => st.Seats)
                      .HasForeignKey(s => s.SeatTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new { s.TrainId, s.SeatNumber }).IsUnique();
                entity.Property(s => s.SeatNumber).IsRequired().HasMaxLength(20);
            });

            // 4. Ràng buộc User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            });

            // 5. Ràng buộc Station
            modelBuilder.Entity<Station>(entity =>
            {
                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            });

            // 6. Ràng buộc SeatType
            modelBuilder.Entity<SeatType>(entity =>
            {
                entity.Property(st => st.Name).IsRequired().HasMaxLength(50);
                entity.Property(st => st.Description).HasMaxLength(255);
            });

            // 7. Cấu hình Amount trong Payment
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // 8. Chống lỗi xóa dây chuyền cho bảng Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasOne(t => t.Schedule)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(t => t.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Seat)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(t => t.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.User)
                      .WithMany(u => u.Tickets)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}