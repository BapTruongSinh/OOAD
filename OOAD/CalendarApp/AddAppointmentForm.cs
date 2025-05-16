using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;
using CalendarApp.Models;

namespace CalendarApp
{
    public partial class AddAppointmentForm : Form
    {
        private User _currentUser;
        private bool _isGroupMeeting = false;
        private Calendar _calendar;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Appointment CreatedAppointment { get; private set; }

        private TextBox _txtName;
        private TextBox _txtLocation;
        private DateTimePicker _dtpDate;
        private DateTimePicker _dtpStartTime;
        private DateTimePicker _dtpEndTime;
        private CheckBox _chkGroupMeeting;

        public AddAppointmentForm(User currentUser, bool isGroupMeeting)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _isGroupMeeting = isGroupMeeting;
            _calendar = new Calendar(_currentUser);
            InitializeComponent();
        }

        // Constructor that accepts an existing Calendar instance
        public AddAppointmentForm(User currentUser, bool isGroupMeeting, Calendar calendar)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _isGroupMeeting = isGroupMeeting;
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Add Appointment";
            this.Size = new System.Drawing.Size(450, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.DialogResult = DialogResult.Cancel;

            // Create controls
            Label lblTitle = new Label
            {
                Text = "New Appointment",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(400, 30)
            };

            Label lblName = new Label
            {
                Text = "Name:",
                Location = new Point(20, 70),
                Size = new Size(100, 20)
            };

            _txtName = new TextBox
            {
                Location = new Point(120, 70),
                Size = new Size(250, 20)
            };

            Label lblLocation = new Label
            {
                Text = "Location:",
                Location = new Point(20, 100),
                Size = new Size(100, 20)
            };

            _txtLocation = new TextBox
            {
                Location = new Point(120, 100),
                Size = new Size(250, 20)
            };

            Label lblDate = new Label
            {
                Text = "Date:",
                Location = new Point(20, 130),
                Size = new Size(100, 20)
            };

            _dtpDate = new DateTimePicker
            {
                Location = new Point(120, 130),
                Size = new Size(120, 20),
                Format = DateTimePickerFormat.Short
            };

            Label lblStartTime = new Label
            {
                Text = "Start Time:",
                Location = new Point(20, 160),
                Size = new Size(100, 20)
            };

            _dtpStartTime = new DateTimePicker
            {
                Location = new Point(120, 160),
                Size = new Size(120, 20),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };

            Label lblEndTime = new Label
            {
                Text = "End Time:",
                Location = new Point(20, 190),
                Size = new Size(100, 20)
            };

            _dtpEndTime = new DateTimePicker
            {
                Location = new Point(120, 190),
                Size = new Size(120, 20),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };

            _chkGroupMeeting = new CheckBox
            {
                Text = "This is a Group Meeting",
                Location = new Point(20, 220),
                Size = new Size(250, 40)
            };

            Button btnSave = new Button
            {
                Text = "Save",
                Location = new Point(120, 260),
                Size = new Size(90, 30)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(230, 260),
                Size = new Size(90, 30)
            };

            // Set default values
            _dtpDate.Value = DateTime.Today;
            _dtpStartTime.Value = DateTime.Now.AddHours(1).Date.AddHours(DateTime.Now.Hour);
            _dtpEndTime.Value = DateTime.Now.AddHours(2).Date.AddHours(DateTime.Now.Hour);
            _chkGroupMeeting.Checked = _isGroupMeeting;

            // Add event handlers
            _chkGroupMeeting.CheckedChanged += (sender, e) =>
            {
                lblTitle.Text = _chkGroupMeeting.Checked ? "New Group Meeting" : "New Appointment";
            };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (sender, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(_txtName);
            this.Controls.Add(lblLocation);
            this.Controls.Add(_txtLocation);
            this.Controls.Add(lblDate);
            this.Controls.Add(_dtpDate);
            this.Controls.Add(lblStartTime);
            this.Controls.Add(_dtpStartTime);
            this.Controls.Add(lblEndTime);
            this.Controls.Add(_dtpEndTime);
            this.Controls.Add(_chkGroupMeeting);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(_txtName.Text))
            {
                MessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtLocation.Text))
            {
                MessageBox.Show("Please enter a location.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create start and end datetime by combining the date and time
            DateTime startDate = _dtpDate.Value.Date.Add(_dtpStartTime.Value.TimeOfDay);
            DateTime endDate = _dtpDate.Value.Date.Add(_dtpEndTime.Value.TimeOfDay);

            // Check if end time is after start time
            if (endDate <= startDate)
            {
                MessageBox.Show("End time must be after start time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get current appointment data
            string name = _txtName.Text;
            string location = _txtLocation.Text;
            bool isGroupMeeting = _chkGroupMeeting.Checked;

            // Check for appointments with the same name (name duplication check)
            var sameNameAppointments = _calendar.GetUserAppointments()
                .Where(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (sameNameAppointments.Any())
            {
                DialogResult nameResult = MessageBox.Show(
                    $"An appointment with the name '{name}' already exists. Do you want to use this name anyway?",
                    "Name Already Used",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (nameResult == DialogResult.No)
                    return;
            }

            // Check for time conflicts with user's existing appointments
            var conflictingAppointments = _calendar.GetUserAppointments()
                .Where(a => startDate < a.EndTime && endDate > a.StartTime);

            if (conflictingAppointments.Any())
            {
                var conflictingAppointment = conflictingAppointments.First();
                DialogResult replaceResult = MessageBox.Show(
                    $"You already have an appointment scheduled during this time: '{conflictingAppointment.Name}' from {conflictingAppointment.StartTime:g} to {conflictingAppointment.EndTime:g}.\n\nDo you want to replace it?",
                    "Time Conflict",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (replaceResult == DialogResult.Yes)
                {
                    // Remove the conflicting appointment before continuing
                    var dbContext = Program.ServiceProvider.GetService(typeof(CalendarApp.Data.ApplicationDbContext)) as CalendarApp.Data.ApplicationDbContext;
                    dbContext.Appointments.Remove(conflictingAppointment);
                    dbContext.SaveChanges();
                }
                else
                {
                    // User chose not to replace the appointment
                    return;
                }
            }

            // For group meetings, check if there's already one with the same details
            if (isGroupMeeting)
            {
                // Also check if there are individual appointments with the same details
                var conflictingIndividualAppointments = _calendar.GetUserAppointments()
                    .Where(a => !(a is GroupMeeting) && 
                           a.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                           a.Location.Equals(location, StringComparison.OrdinalIgnoreCase) && 
                           Math.Abs((a.StartTime - startDate).TotalMinutes) < 1 && 
                           Math.Abs((a.EndTime - endDate).TotalMinutes) < 1);

                if (conflictingIndividualAppointments.Any())
                {
                    var conflictingAppt = conflictingIndividualAppointments.First();
                    MessageBox.Show(
                        $"There is already an individual appointment '{conflictingAppt.Name}' with the exact same details. Please choose a different name, location or time for your group meeting.",
                        "Appointment Conflict",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            // For non-group meetings, check if there's a matching group meeting to join
            if (!isGroupMeeting)
            {
                // Check for existing individual appointments with the same details
                var duplicateAppointments = _calendar.GetUserAppointments()
                    .Where(a => !(a is GroupMeeting) &&
                           a.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                           a.Location.Equals(location, StringComparison.OrdinalIgnoreCase) && 
                           Math.Abs((a.StartTime - startDate).TotalMinutes) < 1 && 
                           Math.Abs((a.EndTime - endDate).TotalMinutes) < 1);

                if (duplicateAppointments.Any())
                {
                    MessageBox.Show(
                        $"An appointment '{name}' with the same details already exists. Please use a different name, location, or time.",
                        "Duplicate Appointment",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Check for existing group meetings with same name, location, and time from global list
                var matchingGroupMeetings = Calendar.GetAllGroupMeetings()
                    .Where(gm => 
                        gm.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                        gm.Location.Equals(location, StringComparison.OrdinalIgnoreCase) && 
                        Math.Abs((gm.StartTime - startDate).TotalMinutes) < 1 && 
                        Math.Abs((gm.EndTime - endDate).TotalMinutes) < 1)
                    .ToList();

                if (matchingGroupMeetings.Any())
                {
                    // Always show the selection form, even for a single meeting
                    using (var selectionForm = new GroupMeetingSelectionForm(matchingGroupMeetings))
                    {
                        if (selectionForm.ShowDialog() == DialogResult.OK)
                        {
                            var selectedMeeting = selectionForm.SelectedMeeting;
                            if (selectedMeeting != null)
                            {
                                // Add the user to the selected group meeting
                                ((GroupMeeting)selectedMeeting).AddToList(_currentUser);
                                MessageBox.Show("You have been added to the selected group meeting.", 
                                    "Joining Meeting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
                                // Set the created appointment to the group meeting
                                CreatedAppointment = selectedMeeting;
                                
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                                return;
                            }
                        }
                        else
                        {
                            return; // Stay on form if canceled
                        }
                    }
                }
            }

            // Create the appointment or group meeting
            if (isGroupMeeting)
            {
                CreatedAppointment = new GroupMeeting(name, location, startDate, endDate, _currentUser);
                // Add the user as participant in the meeting
                ((GroupMeeting)CreatedAppointment).AddToList(_currentUser);
                
                MessageBox.Show("Group meeting created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                CreatedAppointment = new Appointment(name, location, startDate, endDate, _currentUser);
                MessageBox.Show("Appointment created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
} 