using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CalendarApp.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarApp.Models
{
    public class GroupMeeting : Appointment
    {
        // Navigation properties
        [NotMapped] // Không lưu trong database vì đã có bảng trung gian
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        
        // Quan hệ nhiều-nhiều với User thông qua GroupMeetingParticipant
        public virtual ICollection<GroupMeetingParticipant> Participants { get; set; } = new List<GroupMeetingParticipant>();

        // Constructors
        public GroupMeeting() : base() { }

        public GroupMeeting(string name, string location, DateTime startTime, DateTime endTime, User owner)
            : base(name, location, startTime, endTime, owner) { }

        // Methods
        public void AddToList(User user)
        {
            if (user == null || Id <= 0 || user.Id <= 0)
                return;
                
            // Kiểm tra xem người dùng đã tham gia cuộc họp này chưa
            var dbContext = Program.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            var existingParticipation = dbContext.GroupMeetingParticipants
                .FirstOrDefault(p => p.GroupMeetingId == Id && p.ParticipantId == user.Id);
                
            if (existingParticipation != null)
                return; // Người dùng đã tham gia rồi
                
            try
            {
                // Thêm vào bảng quan hệ nhiều-nhiều
                var participant = new GroupMeetingParticipant
                {
                    GroupMeetingId = Id,
                    ParticipantId = user.Id
                };
                
                dbContext.GroupMeetingParticipants.Add(participant);
                dbContext.SaveChanges();
                
                Console.WriteLine($"Added user {user.Id} to group meeting {Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user to group meeting: {ex.Message}");
            }
        }

        public override string ToString()
        {
            return $"Group Meeting: {Name} ({StartTime:g} - {EndTime:g}) at {Location}";
        }
    }
} 