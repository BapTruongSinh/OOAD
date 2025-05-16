using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calendar.Models
{
    public class Reminder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public DateTime Time { get; set; }
        
        [Required]
        public string Note { get; set; } = string.Empty;
        
        // Foreign keys
        public int UserId { get; set; }
        public int? AppointmentId { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }
        
        public Reminder()
        {
        }
        
        public Reminder(DateTime time, string note)
        {
            Time = time;
            Note = note;
        }
    }
} 
