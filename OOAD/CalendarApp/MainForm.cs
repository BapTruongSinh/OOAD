using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CalendarApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp
{
    public partial class MainForm : Form
    {
        private User _currentUser;
        private Calendar _calendar;
        private ListView _appointmentList;
        private ListView _reminderList;

        public MainForm(User currentUser)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _calendar = new Calendar(_currentUser);
            InitializeComponent();
            UpdateLists();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = $"Calendar App - {_currentUser.Name}";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create menu strip            
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem appointmentMenu = new ToolStripMenuItem("Appointments");
            ToolStripMenuItem addAppointmentItem = new ToolStripMenuItem("Add Appointment");
            appointmentMenu.DropDownItems.Add(addAppointmentItem);
            menuStrip.Items.Add(appointmentMenu);                        
            
            // Add logout menu item            
            ToolStripMenuItem logoutMenu = new ToolStripMenuItem("Logout");
            logoutMenu.Click += LogoutMenu_Click;
            menuStrip.Items.Add(logoutMenu);

            // Create splitter container
            SplitContainer splitter = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300
            };

            // Appointments section
            Panel appointmentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label appointmentLabel = new Label
            {
                Text = "Your Appointments",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30
            };

            _appointmentList = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            _appointmentList.Columns.Add("ID", 40);
            _appointmentList.Columns.Add("Type", 100);
            _appointmentList.Columns.Add("Name", 150);
            _appointmentList.Columns.Add("Location", 120);
            _appointmentList.Columns.Add("Start Time", 130);
            _appointmentList.Columns.Add("End Time", 130);
            _appointmentList.Columns.Add("Action", 120); // For the reminder button column
            _appointmentList.Columns.Add("View", 120); // For viewing participants

            // Reminders section
            Panel reminderPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label reminderLabel = new Label
            {
                Text = "Your Reminders",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30
            };

            _reminderList = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            _reminderList.Columns.Add("ID", 40);
            _reminderList.Columns.Add("Time", 150);
            _reminderList.Columns.Add("Note", 200);
            _reminderList.Columns.Add("For Appointment", 150);

            // Event handlers
            addAppointmentItem.Click += AddAppointment_Click;
            
            // Add controls to form
            appointmentPanel.Controls.Add(_appointmentList);
            appointmentPanel.Controls.Add(appointmentLabel);
            
            reminderPanel.Controls.Add(_reminderList);
            reminderPanel.Controls.Add(reminderLabel);
            
            splitter.Panel1.Controls.Add(appointmentPanel);
            splitter.Panel2.Controls.Add(reminderPanel);
            
            this.Controls.Add(splitter);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void AddAppointment_Click(object sender, EventArgs e)
        {
            AddAppointmentForm form = new AddAppointmentForm(_currentUser, false, _calendar);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Get the appointment created by the form
                Appointment appointment = form.CreatedAppointment;
                
                if (appointment != null)
                {
                    // Add to calendar
                    _calendar.AddAppointment(appointment);
                    
                    // Update the list views
                    UpdateLists();
                }
            }
        }

        private void SetReminder_Click(object sender, EventArgs e)
        {
            // Get the selected appointment
            if (_appointmentList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an appointment to set a reminder for.", 
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Appointment appointment = _appointmentList.SelectedItems[0].Tag as Appointment;
            if (appointment == null)
            {
                MessageBox.Show("Could not retrieve the selected appointment.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Show reminder dialog
            ReminderForm reminderForm = new ReminderForm(appointment, _currentUser);
            if (reminderForm.ShowDialog() == DialogResult.OK)
            {
                // Add reminder to calendar
                if (reminderForm.CreatedReminder != null)
                {
                    _calendar.AddReminder(reminderForm.CreatedReminder);
                    UpdateLists();
                }
            }
        }

        private void UpdateLists()
        {
            // Update appointment list
            _appointmentList.Items.Clear();
            foreach (var appointment in _calendar.GetUserAppointments())
            {
                string type = appointment is GroupMeeting ? "Group Meeting" : "Appointment";
                
                ListViewItem item = new ListViewItem(appointment.Id.ToString());
                item.SubItems.Add(type);
                item.SubItems.Add(appointment.Name);
                item.SubItems.Add(appointment.Location);
                item.SubItems.Add(appointment.StartTime.ToString("g"));
                item.SubItems.Add(appointment.EndTime.ToString("g"));
                
                // Add a "Set Reminder" button as a subitems
                item.SubItems.Add("Set Reminder");
                
                // Add a "View Participants" button for group meetings
                if (appointment is GroupMeeting)
                {
                    item.SubItems.Add("View Participants");
                    item.BackColor = Color.LightBlue;
                }
                else
                {
                    item.SubItems.Add("");
                }
                
                item.Tag = appointment;
                
                _appointmentList.Items.Add(item);
            }

            // Add click event for the ListView
            _appointmentList.MouseClick -= AppointmentList_MouseClick;
            _appointmentList.MouseClick += AppointmentList_MouseClick;

            // Update reminder list
            _reminderList.Items.Clear();
            foreach (var reminder in _calendar.GetUserReminders())
            {
                ListViewItem item = new ListViewItem(reminder.Id.ToString());
                item.SubItems.Add(reminder.Time.ToString("g"));
                item.SubItems.Add(reminder.Note);
                item.SubItems.Add(reminder.RelatedAppointment?.Name ?? "Unknown");
                item.Tag = reminder;
                
                _reminderList.Items.Add(item);
            }
        }

        private void AppointmentList_MouseClick(object sender, MouseEventArgs e)
        {
            // Get the clicked item
            ListViewHitTestInfo hitTest = _appointmentList.HitTest(e.X, e.Y);
            if (hitTest.Item != null && hitTest.SubItem != null)
            {
                int subItemIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                Appointment appointment = hitTest.Item.Tag as Appointment;
                
                if (appointment == null)
                    return;
                
                // Check if it's the "Set Reminder" button column (column index 6)
                if (subItemIndex == 6)
                {
                    // Change cursor temporarily to indicate click was registered
                    this.Cursor = Cursors.WaitCursor;
                    
                    ReminderForm reminderForm = new ReminderForm(appointment, _currentUser);
                    reminderForm.ShowDialog();
                    
                    // If reminder was created, update lists
                    if (reminderForm.DialogResult == DialogResult.OK && reminderForm.CreatedReminder != null)
                    {
                        _calendar.AddReminder(reminderForm.CreatedReminder);
                        UpdateLists();
                    }
                    
                    this.Cursor = Cursors.Default;
                }
                // Check if it's the "View Participants" button column (column index 7)
                else if (subItemIndex == 7 && appointment is GroupMeeting)
                {
                    GroupMeeting groupMeeting = appointment as GroupMeeting;
                    if (groupMeeting != null)
                    {
                        // Lấy dữ liệu người tham gia từ database
                        var dbContext = Program.ServiceProvider.GetService(typeof(CalendarApp.Data.ApplicationDbContext)) as CalendarApp.Data.ApplicationDbContext;
                        var participants = dbContext.GroupMeetingParticipants
                            .Where(p => p.GroupMeetingId == groupMeeting.Id)
                            .Include(p => p.Participant)
                            .Select(p => p.Participant)
                            .ToList();

                        // Tạo bảng định dạng hiển thị danh sách
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.AppendLine($"Danh sách người tham gia cuộc họp '{groupMeeting.Name}':");
                        sb.AppendLine();
                        sb.AppendLine("┌─────────┬──────────────────────────┐");
                        sb.AppendLine("│   ID    │      Tên người dùng      │");
                        sb.AppendLine("├─────────┼──────────────────────────┤");
                        
                        foreach (var user in participants)
                        {
                            if (user != null)
                            {
                                sb.AppendLine($"│ {user.Id,-7} │ {user.Name,-24} │");
                            }
                        }
                        
                        sb.AppendLine("└─────────┴──────────────────────────┘");
                        
                        // Hiển thị bảng với thông tin đầy đủ
                        MessageBox.Show(
                            sb.ToString(), 
                            "Thông tin người tham gia cuộc họp", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void LogoutMenu_Click(object sender, EventArgs e)
        {
            // Confirm logout
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?", 
                "Confirm Logout", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                // Close this form - the login form will be shown by its event handler
                this.Close();
            }
        }
    }
} 