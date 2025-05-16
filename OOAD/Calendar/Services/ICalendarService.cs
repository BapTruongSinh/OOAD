using System;
using System.Collections.Generic;
using Calendar.Models;

namespace Calendar.Services
{
    // Interface cho CalendarService (tuân theo nguyên tắc Interface Segregation)
    public interface ICalendarService
    {
        // Quản lý người dùng
        User? GetUserById(int id);
        User? GetUserByName(string name);
        void AddUser(User user);
        
        // Quản lý lịch hẹn
        IEnumerable<Appointment> GetAppointmentsForUser(int userId);
        IEnumerable<Appointment> GetAppointmentsForDate(int userId, DateTime date);
        void AddAppointment(Appointment appointment);
        bool UpdateAppointment(Appointment appointment);
        bool DeleteAppointment(int appointmentId);
        bool CheckOverlap(Appointment appointment);
        Appointment FindOverlappingAppointment(Appointment appointment);
        bool CheckDuplicateName(Appointment appointment);
        
        // Quản lý cuộc họp nhóm
        IEnumerable<GroupMeeting> GetGroupMeetingsForUser(int userId);
        void AddGroupMeeting(GroupMeeting groupMeeting);
        bool AddUserToGroupMeeting(int groupMeetingId, int userId);
        bool RemoveUserFromGroupMeeting(int groupMeetingId, int userId);
        IEnumerable<GroupMeeting> FindMatchingGroupMeetings(string name, DateTime startTime, DateTime endTime);
        
        // Quản lý nhắc nhở
        IEnumerable<Reminder> GetRemindersForUser(int userId);
        void AddReminder(Reminder reminder);
        bool UpdateReminder(Reminder reminder);
        bool DeleteReminder(int reminderId);
    }
} 
