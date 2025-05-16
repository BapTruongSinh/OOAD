using System;
using System.IO;
using Calendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Calendar.Data
{
    public class CalendarDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<GroupMeeting> GroupMeetings { get; set; } = null!;
        public DbSet<Reminder> Reminders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection") 
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                
                optionsBuilder.UseSqlServer(connectionString)
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho mối quan hệ User - GroupMeeting (many-to-many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.GroupMeetings)
                .WithMany(gm => gm.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserGroupMeetings",
                    j => j
                        .HasOne<GroupMeeting>()
                        .WithMany()
                        .HasForeignKey("GroupMeetingId")
                        .OnDelete(DeleteBehavior.NoAction),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction),
                    j => j.HasKey("UserId", "GroupMeetingId")
                );

            // Cấu hình các mối quan hệ khác với DeleteBehavior
            modelBuilder.Entity<User>()
                .HasMany(u => u.Appointments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reminders)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình kế thừa cho Appointment và GroupMeeting
            modelBuilder.Entity<Appointment>()
                .HasDiscriminator<string>("AppointmentType")
                .HasValue<Appointment>("Appointment")
                .HasValue<GroupMeeting>("GroupMeeting");
        }
    }
} 
