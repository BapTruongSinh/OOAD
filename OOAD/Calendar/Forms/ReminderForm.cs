using System;
using System.Windows.Forms;
using Calendar.Models;
using Calendar.Services;

namespace Calendar.Forms
{
    public partial class ReminderForm : Form
    {
        private readonly ICalendarService _calendarService;
        private readonly User _currentUser;
        private readonly Appointment _appointment;

        public ReminderForm(User user, Appointment appointment)
        {
            InitializeComponent();
            _calendarService = new CalendarService();
            _currentUser = user;
            _appointment = appointment;

            // Hiển thị thông tin cuộc hẹn
            lblAppointmentInfo.Text = $"Cuộc hẹn: {_appointment.Name} ({_appointment.StartTime:dd/MM/yyyy HH:mm})";
            
            // Đặt mặc định thời gian nhắc nhở là 30 phút trước cuộc hẹn
            dtpReminderTime.Value = _appointment.StartTime.AddMinutes(-30);
            
            // Đặt giới hạn thời gian nhắc nhở không được sau thời gian bắt đầu cuộc hẹn
            dtpReminderTime.MaxDate = _appointment.StartTime.AddMinutes(-1);
            
            // Thiết lập ghi chú mặc định
            txtNote.Text = $"Nhắc nhở cho {_appointment.Name}";
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
                // Tạo nhắc nhở mới
                var reminder = new Reminder
                {
                    Time = dtpReminderTime.Value,
                    Note = txtNote.Text,
                    UserId = _currentUser.Id,
                    AppointmentId = _appointment.Id
                };
                
                // Lưu nhắc nhở
                _calendarService.AddReminder(reminder);
                MessageBox.Show("Đã thêm nhắc nhở thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\n\nChi tiết lỗi: " + ex.InnerException.Message;
                }
                
                MessageBox.Show($"Lỗi khi thêm nhắc nhở: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 