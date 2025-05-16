using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calendar.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        // Foreign key for User
        public int UserId { get; set; }
        
        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        public Appointment()
        {
        }
        
        public Appointment(string name, string location, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Location = location;
            StartTime = startTime;
            EndTime = endTime;
        }
        
        // Kiểm tra tính hợp lệ của lịch hẹn
        public bool CheckValid()
        {
            return EndTime > StartTime;
        }
        
        // Kiểm tra trùng lặp với lịch hẹn khác
        public bool CheckOverlap(Appointment other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
        }
    }
} 
