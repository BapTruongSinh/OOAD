using System;
using System.Linq;
using CalendarApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Đảm bảo cơ sở dữ liệu được tạo
                context.Database.EnsureCreated();

                // Kiểm tra xem đã có dữ liệu chưa
                if (context.Users.Any())
                {
                    return; // Đã có dữ liệu rồi, không cần khởi tạo nữa
                }

                // Tạo người dùng mẫu
                var users = new User[]
                {
                    new User { Name = "admin", Password = "admin" },
                    new User { Name = "user1", Password = "password1" },
                    new User { Name = "user2", Password = "password2" }
                };

                foreach (var user in users)
                {
                    context.Users.Add(user);
                }

                context.SaveChanges();

                // Tạo các cuộc hẹn mẫu
                var now = DateTime.Now;
                var appointments = new Appointment[]
                {
                    new Appointment
                    {
                        Name = "Meeting with team",
                        Location = "Conference Room A",
                        StartTime = now.AddDays(1).Date.AddHours(9),
                        EndTime = now.AddDays(1).Date.AddHours(10),
                        OwnerId = users[0].Id
                    },
                    new Appointment
                    {
                        Name = "Lunch",
                        Location = "Cafeteria",
                        StartTime = now.AddDays(1).Date.AddHours(12),
                        EndTime = now.AddDays(1).Date.AddHours(13),
                        OwnerId = users[0].Id
                    }
                };

                foreach (var appointment in appointments)
                {
                    context.Appointments.Add(appointment);
                }

                context.SaveChanges();

                // Tạo cuộc họp nhóm mẫu
                var groupMeeting = new GroupMeeting
                {
                    Name = "Project Planning",
                    Location = "Conference Room B",
                    StartTime = now.AddDays(2).Date.AddHours(14),
                    EndTime = now.AddDays(2).Date.AddHours(15),
                    OwnerId = users[0].Id
                };

                context.GroupMeetings.Add(groupMeeting);
                context.SaveChanges();

                // Thêm thành viên vào cuộc họp nhóm
                var participants = new GroupMeetingParticipant[]
                {
                    new GroupMeetingParticipant
                    {
                        GroupMeetingId = groupMeeting.Id,
                        ParticipantId = users[0].Id
                    },
                    new GroupMeetingParticipant
                    {
                        GroupMeetingId = groupMeeting.Id,
                        ParticipantId = users[1].Id
                    }
                };

                foreach (var participant in participants)
                {
                    context.GroupMeetingParticipants.Add(participant);
                }

                context.SaveChanges();

                // Tạo nhắc nhở mẫu
                var reminders = new Reminder[]
                {
                    new Reminder
                    {
                        Time = appointments[0].StartTime.AddMinutes(-30),
                        Note = "Prepare meeting notes",
                        RelatedAppointmentId = appointments[0].Id,
                        OwnerId = users[0].Id
                    },
                    new Reminder
                    {
                        Time = groupMeeting.StartTime.AddMinutes(-15),
                        Note = "Prepare presentation slides",
                        RelatedAppointmentId = groupMeeting.Id,
                        OwnerId = users[0].Id
                    }
                };

                foreach (var reminder in reminders)
                {
                    context.Reminders.Add(reminder);
                }

                context.SaveChanges();
            }
        }
    }
} 