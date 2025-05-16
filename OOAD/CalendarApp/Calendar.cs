using System;
using System.Collections.Generic;
using System.Linq;
using CalendarApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp.Models
{
    public class Calendar
    {
        // Static collection for storing all appointments across all users - không cần thiết nữa vì dùng database
        // public static List<Appointment> GlobalAppointments { get; set; } = new List<Appointment>();
        
        // ApplicationDbContext để truy cập database
        private readonly ApplicationDbContext _context;
        
        // Collections for storing data - không cần thiết nữa vì dùng database
        // public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        // public List<Reminder> Reminders { get; set; } = new List<Reminder>();

        // Current user
        private User _currentUser;

        // Constructor
        public Calendar(User currentUser)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _context = Program.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
        }

        // Methods for appointment management
        public bool AddAppointment(Appointment appointment)
        {
            if (appointment == null)
                return false;

            if (!appointment.CheckValid())
                return false;

            // Check for overlapping appointments
            var userAppointments = GetUserAppointments();
            foreach (var existingAppointment in userAppointments.Where(a => a.Owner?.Id == _currentUser.Id))
            {
                if (appointment.CheckOverlap(existingAppointment))
                    return false;
            }

            // Thiết lập owner
            appointment.OwnerId = _currentUser.Id;
            
            // Thêm vào database
            _context.Appointments.Add(appointment);
            
            try {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error saving appointment: {ex.Message}");
                return false;
            }
        }

        public bool AddGroupMeeting(GroupMeeting meeting)
        {
            if (meeting == null)
                return false;

            if (!meeting.CheckValid())
                return false;

            // Check for overlapping appointments for the current user
            var userAppointments = GetUserAppointments();
            foreach (var existingAppointment in userAppointments.Where(a => a.Owner?.Id == _currentUser.Id))
            {
                if (meeting.CheckOverlap(existingAppointment))
                    return false;
            }

            // Add current user to the meeting participants
            meeting.OwnerId = _currentUser.Id;

            try {
                // Thêm cuộc họp vào database
                _context.GroupMeetings.Add(meeting);
                _context.SaveChanges();
                
                // Thêm mối quan hệ Group Meeting - User sau khi đã có ID
                var participant = new GroupMeetingParticipant
                {
                    GroupMeetingId = meeting.Id,
                    ParticipantId = _currentUser.Id
                };
                
                _context.GroupMeetingParticipants.Add(participant);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error saving group meeting: {ex.Message}");
                return false;
            }
        }

        public bool AddReminder(Reminder reminder)
        {
            if (reminder == null)
                return false;

            if (reminder.RelatedAppointment == null || reminder.Time >= reminder.RelatedAppointment.StartTime)
                return false;

            // Thiết lập owner và related appointment
            reminder.OwnerId = _currentUser.Id;
            reminder.RelatedAppointmentId = reminder.RelatedAppointment.Id;
            
            // Thêm vào database
            _context.Reminders.Add(reminder);
            
            try {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error saving reminder: {ex.Message}");
                return false;
            }
        }

        // Get user's appointments and reminders
        public List<Appointment> GetUserAppointments()
        {
            try
            {
                // Lấy tất cả các cuộc hẹn mà người dùng hiện tại tạo ra
                var ownAppointments = _context.Appointments
                    .Where(a => a.OwnerId == _currentUser.Id)
                    .Include(a => a.Owner)
                    .ToList();
                
                // Lấy tất cả các cuộc họp nhóm mà người dùng hiện tại tham gia
                var participatingMeetings = _context.GroupMeetingParticipants
                    .Where(p => p.ParticipantId == _currentUser.Id)
                    .Include(p => p.GroupMeeting)
                    .ThenInclude(gm => gm.Owner)
                    .Select(p => p.GroupMeeting)
                    .ToList();
                
                Console.WriteLine($"Found {ownAppointments.Count} own appointments and {participatingMeetings.Count} participating meetings");
                
                // Kết hợp hai danh sách
                var combined = new List<Appointment>();
                combined.AddRange(ownAppointments);
                combined.AddRange(participatingMeetings);
                    
                return combined;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserAppointments: {ex.Message}");
                return new List<Appointment>();
            }
        }

        public List<Reminder> GetUserReminders()
        {
            return _context.Reminders
                .Where(r => r.OwnerId == _currentUser.Id)
                .Include(r => r.RelatedAppointment)
                .ToList();
        }

        // Get global group meetings
        public static List<GroupMeeting> GetAllGroupMeetings()
        {
            var dbContext = Program.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            return dbContext.GroupMeetings
                .Include(gm => gm.Owner)
                .Include(gm => gm.Participants)
                .ThenInclude(p => p.Participant)
                .ToList();
        }
    }
} 