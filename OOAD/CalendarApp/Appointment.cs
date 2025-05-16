using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarApp.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Location { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        // Foreign key
        public int OwnerId { get; set; }

        // Navigation properties
        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
        
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

        // Constructors
        public Appointment() { }

        public Appointment(string name, string location, DateTime startTime, DateTime endTime, User owner)
        {
            Name = name;
            Location = location;
            StartTime = startTime;
            EndTime = endTime;
            Owner = owner;
            OwnerId = owner?.Id ?? 0;
        }

        // Methods
        public virtual bool CheckValid()
        {
            return EndTime > StartTime && !string.IsNullOrEmpty(Name);
        }

        public virtual bool CheckOverlap(Appointment other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
        }

        public override string ToString()
        {
            return $"{Name} ({StartTime:g} - {EndTime:g}) at {Location}";
        }
    }
} 