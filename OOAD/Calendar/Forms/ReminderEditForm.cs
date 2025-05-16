using System;
using System.Windows.Forms;
using Calendar.Models;
using Calendar.Services;

namespace Calendar.Forms
{
    public partial class ReminderEditForm : Form
    {
        private readonly ICalendarService _calendarService;
        private readonly User _currentUser;
        private readonly Reminder _reminder;
        private readonly Appointment _appointment;

        public ReminderEditForm(User user, Reminder reminder, Appointment appointment)
        {
            InitializeComponent();
            _calendarService = new CalendarService();
            _currentUser = user;
            _reminder = reminder;
            _appointment = appointment;

            // Hiển thị thông tin cuộc hẹn
            lblAppointmentInfo.Text = $"Cuộc hẹn: {_appointment.Name} ({_appointment.StartTime:dd/MM/yyyy HH:mm})";
            
            // Hiển thị thông tin lời nhắc hiện tại
            dtpReminderTime.Value = _reminder.Time;
            txtNote.Text = _reminder.Note;
            
            // Đặt giới hạn thời gian nhắc nhở không được sau thời gian bắt đầu cuộc hẹn
            dtpReminderTime.MaxDate = _appointment.StartTime.AddMinutes(-1);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtpReminderTime.Value >= _appointment.StartTime)
            {
                MessageBox.Show("Thời gian nhắc nhở phải trước thời gian bắt đầu cuộc hẹn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpReminderTime.Focus();
                return;
            }

            try
            {
                // Cập nhật thông tin lời nhắc
                _reminder.Time = dtpReminderTime.Value;
                _reminder.Note = txtNote.Text;
                
                // Lưu thay đổi
                bool success = _calendarService.UpdateReminder(_reminder);
                
                if (success)
                {
                    MessageBox.Show("Đã cập nhật lời nhắc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật lời nhắc. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\n\nChi tiết lỗi: " + ex.InnerException.Message;
                }
                
                MessageBox.Show($"Lỗi khi cập nhật lời nhắc: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
        #region Windows Form Designer generated code
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblAppointmentInfo = new System.Windows.Forms.Label();
            this.lblReminderTime = new System.Windows.Forms.Label();
            this.dtpReminderTime = new System.Windows.Forms.DateTimePicker();
            this.lblNote = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            // lblAppointmentInfo
            this.lblAppointmentInfo.AutoSize = true;
            this.lblAppointmentInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAppointmentInfo.Location = new System.Drawing.Point(12, 9);
            this.lblAppointmentInfo.Name = "lblAppointmentInfo";
            this.lblAppointmentInfo.Size = new System.Drawing.Size(103, 20);
            this.lblAppointmentInfo.TabIndex = 0;
            this.lblAppointmentInfo.Text = "Thông tin cuộc hẹn";
            
            // lblReminderTime
            this.lblReminderTime.AutoSize = true;
            this.lblReminderTime.Location = new System.Drawing.Point(12, 50);
            this.lblReminderTime.Name = "lblReminderTime";
            this.lblReminderTime.Size = new System.Drawing.Size(127, 20);
            this.lblReminderTime.TabIndex = 1;
            this.lblReminderTime.Text = "Thời gian nhắc nhở:";
            
            // dtpReminderTime
            this.dtpReminderTime.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpReminderTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReminderTime.Location = new System.Drawing.Point(145, 50);
            this.dtpReminderTime.Name = "dtpReminderTime";
            this.dtpReminderTime.Size = new System.Drawing.Size(250, 27);
            this.dtpReminderTime.TabIndex = 2;
            
            // lblNote
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(12, 90);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(61, 20);
            this.lblNote.TabIndex = 3;
            this.lblNote.Text = "Ghi chú:";
            
            // txtNote
            this.txtNote.Location = new System.Drawing.Point(12, 113);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(383, 100);
            this.txtNote.TabIndex = 4;
            
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(109, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 35);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(275, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // ReminderEditForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 277);
            this.Controls.Add(this.lblAppointmentInfo);
            this.Controls.Add(this.lblReminderTime);
            this.Controls.Add(this.dtpReminderTime);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReminderEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chỉnh sửa lời nhắc";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblAppointmentInfo;
        private System.Windows.Forms.Label lblReminderTime;
        private System.Windows.Forms.DateTimePicker dtpReminderTime;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        #endregion
    }
} 