using Microsoft.EntityFrameworkCore;
using CalendarApp.Models;

namespace CalendarApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<GroupMeeting> GroupMeetings { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<GroupMeetingParticipant> GroupMeetingParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa quan hệ giữa User và Appointment (1 - nhiều)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Owner)
                .WithMany()
                .HasForeignKey(a => a.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Định nghĩa quan hệ giữa GroupMeeting và User thông qua GroupMeetingParticipant (nhiều - nhiều)
            modelBuilder.Entity<GroupMeetingParticipant>()
                .HasKey(gmp => new { gmp.GroupMeetingId, gmp.ParticipantId });

            modelBuilder.Entity<GroupMeetingParticipant>()
                .HasOne(gmp => gmp.GroupMeeting)
                .WithMany(gm => gm.Participants)
                .HasForeignKey(gmp => gmp.GroupMeetingId);

            modelBuilder.Entity<GroupMeetingParticipant>()
                .HasOne(gmp => gmp.Participant)
                .WithMany(u => u.Participations)
                .HasForeignKey(gmp => gmp.ParticipantId);

            // Định nghĩa quan hệ giữa Reminder và Appointment
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.RelatedAppointment)
                .WithMany(a => a.Reminders)
                .HasForeignKey(r => r.RelatedAppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Định nghĩa quan hệ giữa Reminder và User
            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.Reminders)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình bảng-per-hierarchy mapping cho Appointment và GroupMeeting
            modelBuilder.Entity<Appointment>()
                .HasDiscriminator<string>("AppointmentType")
                .HasValue<Appointment>("Appointment")
                .HasValue<GroupMeeting>("GroupMeeting");
        }
    }
} 