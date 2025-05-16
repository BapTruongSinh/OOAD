using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using CalendarApp.Models;

namespace CalendarApp
{
    public partial class ReminderForm : Form
    {
        private Appointment _appointment;
        private User _user;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Reminder CreatedReminder { get; private set; }

        private DateTimePicker _dtpReminderTime;
        private TextBox _txtReminderNote;

        public ReminderForm(Appointment appointment, User user)
        {
            _appointment = appointment ?? throw new ArgumentNullException(nameof(appointment));
            _user = user ?? throw new ArgumentNullException(nameof(user));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Set Reminder";
            this.Size = new System.Drawing.Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.DialogResult = DialogResult.Cancel;

            // Create controls
            Label lblTitle = new Label
            {
                Text = "Set Reminder",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(300, 25)
            };

            Label lblAppointment = new Label
            {
                Text = $"Appointment: {_appointment.Name}",
                Font = new Font("Arial", 10, FontStyle.Regular),
                Location = new Point(20, 50),
                Size = new Size(350, 20)
            };

            Label lblAppointmentTime = new Label
            {
                Text = $"Time: {_appointment.StartTime:g} - {_appointment.EndTime:g}",
                Font = new Font("Arial", 10, FontStyle.Regular),
                Location = new Point(20, 75),
                Size = new Size(350, 20)
            };

            Label lblReminderTime = new Label
            {
                Text = "Reminder Time:",
                Location = new Point(20, 110),
                Size = new Size(100, 20)
            };

            _dtpReminderTime = new DateTimePicker
            {
                Location = new Point(130, 110),
                Size = new Size(200, 20),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt",
                ShowUpDown = true
            };

            Label lblNote = new Label
            {
                Text = "Note:",
                Location = new Point(20, 140),
                Size = new Size(100, 20)
            };

            _txtReminderNote = new TextBox
            {
                Location = new Point(130, 140),
                Size = new Size(200, 20)
            };

            Button btnSave = new Button
            {
                Text = "Save",
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(110, 180),
                Size = new Size(100, 35)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(230, 180),
                Size = new Size(100, 35)
            };

            // Set default reminder time to 30 minutes before appointment
            _dtpReminderTime.Value = _appointment.StartTime.AddMinutes(-30);
            
            // Default note
            _txtReminderNote.Text = $"Reminder for {_appointment.Name}";

            // Add event handlers
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (sender, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblAppointment);
            this.Controls.Add(lblAppointmentTime);
            this.Controls.Add(lblReminderTime);
            this.Controls.Add(_dtpReminderTime);
            this.Controls.Add(lblNote);
            this.Controls.Add(_txtReminderNote);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate reminder time is before appointment start time
            if (_dtpReminderTime.Value >= _appointment.StartTime)
            {
                MessageBox.Show("Reminder time must be before the appointment start time.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create reminder
            string reminderNote = string.IsNullOrWhiteSpace(_txtReminderNote.Text) ? 
                $"Reminder for {_appointment.Name}" : _txtReminderNote.Text;
            
            CreatedReminder = new Reminder(_dtpReminderTime.Value, reminderNote, _appointment, _user);
            
            // Show notification that it will alert at the specified time
            MessageBox.Show($"A reminder has been set for {_dtpReminderTime.Value:g}.", 
                "Reminder Set", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
} 