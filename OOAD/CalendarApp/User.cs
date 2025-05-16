using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CalendarApp.Data;

namespace CalendarApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
        
        [NotMapped] // Không lưu trong database vì đã có bảng trung gian
        public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();
        
        // Quan hệ nhiều-nhiều với GroupMeeting thông qua GroupMeetingParticipant
        public virtual ICollection<GroupMeetingParticipant> Participations { get; set; } = new List<GroupMeetingParticipant>();

        // Constructors
        public User() { }

        public User(string name)
        {
            Name = name;
        }

        // Methods for user management
        public override string ToString()
        {
            return Name ?? $"User {Id}";
        }
    }
} 