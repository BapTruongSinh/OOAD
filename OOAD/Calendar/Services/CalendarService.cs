using System;
using System.Collections.Generic;
using System.Linq;
using Calendar.Data;
using Calendar.Models;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Services
{
    public class CalendarService : ICalendarService, IDisposable
    {
        private readonly CalendarDbContext _dbContext;
        
        public CalendarService()
        {
            _dbContext = new CalendarDbContext();
        }
        
        // Quản lý người dùng
        public User? GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }
        
        public User? GetUserByName(string name)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Name == name);
        }
        
        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
        
        // Quản lý lịch hẹn
        public IEnumerable<Appointment> GetAppointmentsForUser(int userId)
        {
            return _dbContext.Appointments
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.StartTime)
                .ToList();
        }
        
        public IEnumerable<Appointment> GetAppointmentsForDate(int userId, DateTime date)
        {
            DateTime startOfDay = date.Date;
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);
            
            return _dbContext.Appointments
                .Where(a => a.UserId == userId && 
                           (a.StartTime >= startOfDay && a.StartTime <= endOfDay))
                .OrderBy(a => a.StartTime)
                .ToList();
        }
        
        public void AddAppointment(Appointment appointment)
        {
            try
            {
                _dbContext.Appointments.Add(appointment);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi và ném lại ngoại lệ
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception($"Lỗi khi thêm cuộc hẹn: {ex.Message}", ex);
            }
        }
        
        public bool UpdateAppointment(Appointment appointment)
        {
            try
            {
                _dbContext.Entry(appointment).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                System.Diagnostics.Debug.WriteLine($"Error updating appointment: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        
        public bool DeleteAppointment(int appointmentId)
        {
            try
            {
                var appointment = _dbContext.Appointments.Find(appointmentId);
                if (appointment != null)
                {
                    // First, find and delete all reminders associated with this appointment
                    var associatedReminders = _dbContext.Reminders.Where(r => r.AppointmentId == appointmentId).ToList();
                    foreach (var reminder in associatedReminders)
                    {
                        _dbContext.Reminders.Remove(reminder);
                    }
                    
                    // Then delete the appointment
                    _dbContext.Appointments.Remove(appointment);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                System.Diagnostics.Debug.WriteLine($"Error deleting appointment: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        
        public bool CheckOverlap(Appointment appointment)
        {
            return _dbContext.Appointments
                .Where(a => a.UserId == appointment.UserId && a.Id != appointment.Id)
                .Any(a => a.StartTime < appointment.EndTime && a.EndTime > appointment.StartTime);
        }
        
        // Kiểm tra cuộc hẹn trùng thời gian và trả về cuộc hẹn đầu tiên bị trùng
        public Appointment FindOverlappingAppointment(Appointment appointment)
        {
            return _dbContext.Appointments
                .Where(a => a.UserId == appointment.UserId && a.Id != appointment.Id)
                .FirstOrDefault(a => a.StartTime < appointment.EndTime && a.EndTime > appointment.StartTime);
        }
        
        // Kiểm tra cuộc hẹn trùng tên
        public bool CheckDuplicateName(Appointment appointment)
        {
            // Sử dụng ToLower() cho so sánh không phân biệt chữ hoa/thường
            // EF Core có thể dịch ToLower() sang SQL
            var name = appointment.Name.ToLower();
            return _dbContext.Appointments
                .Where(a => a.UserId == appointment.UserId && a.Id != appointment.Id)
                .Any(a => a.Name.ToLower() == name);
        }
        
        // Quản lý cuộc họp nhóm
        public IEnumerable<GroupMeeting> GetGroupMeetingsForUser(int userId)
        {
            return _dbContext.GroupMeetings
                .Include(gm => gm.Users)
                .Where(gm => gm.Users.Any(u => u.Id == userId))
                .OrderBy(gm => gm.StartTime)
                .ToList();
        }
        
        public void AddGroupMeeting(GroupMeeting groupMeeting)
        {
            try
            {
                _dbContext.GroupMeetings.Add(groupMeeting);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi và ném lại ngoại lệ
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception($"Lỗi khi thêm cuộc họp nhóm: {ex.Message}", ex);
            }
        }
        
        public bool AddUserToGroupMeeting(int groupMeetingId, int userId)
        {
            try
            {
                var groupMeeting = _dbContext.GroupMeetings
                    .Include(gm => gm.Users)
                    .FirstOrDefault(gm => gm.Id == groupMeetingId);
                    
                var user = _dbContext.Users.Find(userId);
                
                if (groupMeeting != null && user != null)
                {
                    if (!groupMeeting.Users.Any(u => u.Id == userId))
                    {
                        groupMeeting.Users.Add(user);
                        _dbContext.SaveChanges();
                    }
                    return true;
                }
                
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        // Quản lý nhắc nhở
        public IEnumerable<Reminder> GetRemindersForUser(int userId)
        {
            return _dbContext.Reminders
                .Include(r => r.Appointment)
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.Time)
                .ToList();
        }
        
        public void AddReminder(Reminder reminder)
        {
            try
            {
                // Detach any existing navigation property references to prevent conflicts
                if (reminder.User != null)
                {
                    _dbContext.Entry(reminder.User).State = EntityState.Detached;
                    reminder.User = null;
                }
                
                if (reminder.Appointment != null)
                {
                    _dbContext.Entry(reminder.Appointment).State = EntityState.Detached;
                    reminder.Appointment = null;
                }
                
                _dbContext.Reminders.Add(reminder);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi và ném lại ngoại lệ
                System.Diagnostics.Debug.WriteLine($"Error adding reminder: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new Exception($"Lỗi khi thêm lời nhắc: {ex.Message}", ex);
            }
        }
        
        public bool DeleteReminder(int reminderId)
        {
            try
            {
                var reminder = _dbContext.Reminders.Find(reminderId);
                if (reminder != null)
                {
                    _dbContext.Reminders.Remove(reminder);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        // Tìm các cuộc họp nhóm trùng tên và thời gian
        public IEnumerable<GroupMeeting> FindMatchingGroupMeetings(string name, DateTime startTime, DateTime endTime)
        {
            var lowerName = name.ToLower();
            
            // Tìm tất cả các GroupMeeting trùng cả tên và thời gian
            return _dbContext.GroupMeetings
                .Include(gm => gm.User)  // Include thông tin người tạo
                .Where(gm => 
                    gm.Name.ToLower() == lowerName && 
                    gm.StartTime == startTime && 
                    gm.EndTime == endTime)
                .ToList();
        }
        
        public bool UpdateReminder(Reminder reminder)
        {
            try
            {
                // Detach any existing navigation property references to prevent conflicts
                if (reminder.User != null)
                {
                    _dbContext.Entry(reminder.User).State = EntityState.Detached;
                    reminder.User = null;
                }
                
                if (reminder.Appointment != null)
                {
                    _dbContext.Entry(reminder.Appointment).State = EntityState.Detached;
                    reminder.Appointment = null;
                }
                
                _dbContext.Entry(reminder).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                System.Diagnostics.Debug.WriteLine($"Error updating reminder: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        
        public bool RemoveUserFromGroupMeeting(int groupMeetingId, int userId)
        {
            try
            {
                // Clear any tracked entities to ensure we get fresh data
                _dbContext.ChangeTracker.Clear();
                
                var groupMeeting = _dbContext.GroupMeetings
                    .Include(gm => gm.Users)
                    .FirstOrDefault(gm => gm.Id == groupMeetingId);
                    
                var user = _dbContext.Users.Find(userId);
                
                if (groupMeeting != null && user != null)
                {
                    // Kiểm tra xem người dùng cần xóa có phải là người tạo không
                    if (groupMeeting.UserId == userId)
                    {
                        // Không cho phép xóa người tạo
                        return false;
                    }
                    
                    // Xóa người dùng khỏi danh sách người tham gia
                    if (groupMeeting.Users.Any(u => u.Id == userId))
                    {
                        groupMeeting.Users.Remove(user);
                        _dbContext.SaveChanges();
                        
                        // Detach the entity after saving to prevent caching issues
                        _dbContext.Entry(groupMeeting).State = EntityState.Detached;
                    }
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                System.Diagnostics.Debug.WriteLine($"Error removing user from group meeting: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        
        // Implement IDisposable
        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
} 
