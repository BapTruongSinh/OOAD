using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarApp.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime Time { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Note { get; set; }
        
        // Foreign keys
        public int RelatedAppointmentId { get; set; }
        public int OwnerId { get; set; }

        // Navigation properties
        [ForeignKey("RelatedAppointmentId")]
        public virtual Appointment RelatedAppointment { get; set; }
        
        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        // Constructors
        public Reminder() { }

        public Reminder(DateTime time, string note, Appointment relatedAppointment, User owner)
        {
            Time = time;
            Note = note;
            RelatedAppointment = relatedAppointment;
            RelatedAppointmentId = relatedAppointment?.Id ?? 0;
            Owner = owner;
            OwnerId = owner?.Id ?? 0;
        }

        public override string ToString()
        {
            return $"Reminder: {Time:g} - {Note} for {RelatedAppointment?.Name ?? "Unknown"}";
        }
    }
} 