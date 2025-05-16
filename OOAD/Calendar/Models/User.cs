using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calendar.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Dùng làm Username

        [Required]
        public string Password { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
        public virtual ICollection<GroupMeeting> GroupMeetings { get; set; } = new List<GroupMeeting>();

        public User()
        {
        }

        public User(string name, string password)
        {
            Name = name; // Name chính là Username
            Password = password;
        }
    }
} 
