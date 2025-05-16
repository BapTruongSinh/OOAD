using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calendar.Models
{
    public class GroupMeeting : Appointment
    {
        // Danh sách người tham gia
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        
        public GroupMeeting() : base()
        {
        }
        
        public GroupMeeting(string name, string location, DateTime startTime, DateTime endTime)
            : base(name, location, startTime, endTime)
        {
        }
        
        // Thêm người dùng vào cuộc họp nhóm
        public void AddToUser(User user)
        {
            if (!Users.Contains(user))
            {
                Users.Add(user);
            }
        }
    }
} 
