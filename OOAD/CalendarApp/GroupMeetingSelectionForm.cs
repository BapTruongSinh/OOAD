using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using CalendarApp.Models;

namespace CalendarApp
{
    public partial class GroupMeetingSelectionForm : Form
    {
        private List<GroupMeeting> _meetings;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GroupMeeting SelectedMeeting { get; private set; }
        
        private ListView _meetingsList;
        private ListView _participantsList;
        private Label _lblParticipants;
        private Label _lblSelectedMeeting;

        public GroupMeetingSelectionForm(List<GroupMeeting> meetings)
        {
            _meetings = meetings ?? throw new ArgumentNullException(nameof(meetings));
            InitializeComponent();
            PopulateMeetingsList();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Available Group Meetings";
            this.Size = new System.Drawing.Size(700, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Create controls
            Label lblTitle = new Label
            {
                Text = "Select a Group Meeting to Join",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(400, 30)
            };

            Label lblInstructions = new Label
            {
                Text = "The following group meetings match your appointment details. Select one to join:",
                Location = new Point(20, 60),
                Size = new Size(660, 20)
            };

            // Meetings list
            _meetingsList = new ListView
            {
                Location = new Point(20, 90),
                Size = new Size(660, 150),
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                GridLines = true,
                HideSelection = false
            };

            _meetingsList.Columns.Add("Creator", 150);
            _meetingsList.Columns.Add("Meeting Name", 150);
            _meetingsList.Columns.Add("Location", 120);
            _meetingsList.Columns.Add("Start Time", 120);
            _meetingsList.Columns.Add("End Time", 120);

            // Selected meeting label
            _lblSelectedMeeting = new Label
            {
                Text = "Selected Meeting: None",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(20, 250),
                Size = new Size(660, 20)
            };

            // Participants list
            _lblParticipants = new Label
            {
                Text = "Meeting Participants:",
                Location = new Point(20, 280),
                Size = new Size(200, 20)
            };

            _participantsList = new ListView
            {
                Location = new Point(20, 310),
                Size = new Size(300, 120),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            _participantsList.Columns.Add("ID", 50);
            _participantsList.Columns.Add("Name", 250);

            Button btnJoin = new Button
            {
                Text = "Join Selected Meeting",
                Location = new Point(360, 350),
                Size = new Size(150, 30)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(530, 350),
                Size = new Size(100, 30)
            };

            // Add event handlers
            _meetingsList.SelectedIndexChanged += MeetingsList_SelectedIndexChanged;
            btnJoin.Click += BtnJoin_Click;
            btnCancel.Click += (sender, e) =>
            {
                SelectedMeeting = null;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblInstructions);
            this.Controls.Add(_meetingsList);
            this.Controls.Add(_lblSelectedMeeting);
            this.Controls.Add(_lblParticipants);
            this.Controls.Add(_participantsList);
            this.Controls.Add(btnJoin);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void MeetingsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_meetingsList.SelectedItems.Count > 0)
            {
                var meeting = _meetingsList.SelectedItems[0].Tag as GroupMeeting;
                if (meeting != null)
                {
                    // Update selected meeting display
                    _lblSelectedMeeting.Text = $"Selected Meeting: {meeting.Name} created by {meeting.Owner?.Name ?? "Unknown"} at {meeting.StartTime:g} - {meeting.EndTime:g}";
                    
                    // Update participants list
                    ShowParticipants(meeting);
                }
            }
            else
            {
                _lblSelectedMeeting.Text = "Selected Meeting: None";
                _participantsList.Items.Clear();
            }
        }

        private void ShowParticipants(GroupMeeting meeting)
        {
            _participantsList.Items.Clear();
            
            if (meeting.Users != null)
            {
                foreach (var user in meeting.Users)
                {
                    ListViewItem item = new ListViewItem(user.Id.ToString());
                    item.SubItems.Add(user.Name);
                    _participantsList.Items.Add(item);
                }
            }
        }

        private void PopulateMeetingsList()
        {
            _meetingsList.Items.Clear();
            
            foreach (var meeting in _meetings)
            {
                ListViewItem item = new ListViewItem(meeting.Owner?.Name ?? "Unknown");
                item.SubItems.Add(meeting.Name);
                item.SubItems.Add(meeting.Location);
                item.SubItems.Add(meeting.StartTime.ToString("g"));
                item.SubItems.Add(meeting.EndTime.ToString("g"));
                item.Tag = meeting;
                
                _meetingsList.Items.Add(item);
            }
            
            // Select the first item if available
            if (_meetingsList.Items.Count > 0)
            {
                _meetingsList.Items[0].Selected = true;
            }
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            if (_meetingsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a meeting to join.", 
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SelectedMeeting = _meetingsList.SelectedItems[0].Tag as GroupMeeting;
            
            if (SelectedMeeting != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
} 