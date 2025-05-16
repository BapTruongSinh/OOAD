using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CalendarApp.Data;

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
            if (!Users.Contains(user))
            {
                Users.Add(user);
                user.GroupMeetings.Add(this);
                
                // Thêm vào bảng quan hệ nhiều-nhiều khi lưu vào database
                if (Id > 0 && user.Id > 0)
                {
                    GroupMeetingParticipant participant = new GroupMeetingParticipant
                    {
                        GroupMeetingId = Id,
                        ParticipantId = user.Id,
                        GroupMeeting = this,
                        Participant = user
                    };
                    
                    Participants.Add(participant);
                    user.Participations.Add(participant);
                }
            }
        }

        public override string ToString()
        {
            return $"Group Meeting: {Name} ({StartTime:g} - {EndTime:g}) at {Location}";
        }
    }
} 