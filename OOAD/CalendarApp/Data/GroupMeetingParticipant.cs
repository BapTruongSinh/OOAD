using CalendarApp.Models;

namespace CalendarApp.Data
{
    public class GroupMeetingParticipant
    {
        public int GroupMeetingId { get; set; }
        public int ParticipantId { get; set; }

        public GroupMeeting GroupMeeting { get; set; }
        public User Participant { get; set; }
    }
} 