using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Calendar.Models;
using Calendar.Services;
using Calendar.Forms;
using System.Text;

namespace Calendar
{
    public partial class Form1 : Form
    {
        private readonly ICalendarService _calendarService;
        private User? _currentUser;
        private DateTime _selectedDate = DateTime.Today;
        private System.Windows.Forms.Timer _reminderTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
            _calendarService = new CalendarService();
            
            // Khởi tạo timer để kiểm tra lời nhắc
            InitializeReminderTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Hiển thị form đăng nhập khi khởi động
            ShowLoginForm();

            // Cấu hình DataGridView
            ConfigureDataGridView();
            
            // Cập nhật trạng thái menu ban đầu
            UpdateMenuStatus();
        }

        private void ConfigureDataGridView()
        {
            // Thiết lập sự kiện khi người dùng double-click vào một cuộc hẹn
            dgvAppointments.CellDoubleClick += DgvAppointments_CellDoubleClick;
            
            // Thiết lập sự kiện khi người dùng chọn hàng khác
            dgvAppointments.SelectionChanged += DgvAppointments_SelectionChanged;
        }

        private void DgvAppointments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo người dùng đã click vào một hàng chứ không phải header
            {
                // Chọn hàng được double-click
                dgvAppointments.Rows[e.RowIndex].Selected = true;

                // Gọi hàm sửa cuộc hẹn
                EditSelectedAppointment();
            }
        }

        private void DgvAppointments_SelectionChanged(object sender, EventArgs e)
        {
            UpdateMenuStatus();
        }

        private void ShowLoginForm()
        {
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                _currentUser = loginForm.LoggedInUser;
                Text = $"Lịch - {_currentUser?.Name}";
                LoadAppointmentsForDate(_selectedDate);
                
                // Kích hoạt timer kiểm tra lời nhắc sau khi đăng nhập
                _reminderTimer.Enabled = true;
            }
            else
            {
                // Nếu không đăng nhập thành công, đóng ứng dụng
                Close();
            }
        }

        private void LoadUserAppointments()
        {
            if (_currentUser != null)
            {
                // Get regular appointments
                var appointments = _calendarService.GetAppointmentsForUser(_currentUser.Id);

                // Get group meetings where user is a participant but not creator
                var participantMeetings = _calendarService.GetGroupMeetingsForUser(_currentUser.Id)
                    .Where(gm => gm.UserId != _currentUser.Id)
                    .ToList();

                // Combine both lists
                var allAppointments = appointments.Concat(participantMeetings).ToList();

                // Display all appointments
                DisplayAppointments(allAppointments);
            }
        }

        private void LoadAppointmentsForDate(DateTime date)
        {
            if (_currentUser != null)
            {
                lblSelectedDate.Text = $"Lịch hẹn ngày: {date:dd/MM/yyyy}";

                // Get regular appointments for the date
                var appointments = _calendarService.GetAppointmentsForDate(_currentUser.Id, date);

                // Get group meetings where user is a participant but not creator
                var participantMeetings = _calendarService.GetGroupMeetingsForUser(_currentUser.Id)
                    .Where(gm => gm.UserId != _currentUser.Id &&
                           gm.StartTime.Date == date.Date)
                    .ToList();

                // Combine both lists
                var allAppointments = appointments.Concat(participantMeetings).ToList();

                // Display all appointments
                DisplayAppointments(allAppointments);
            }
        }

        private void DisplayAppointments(IEnumerable<Appointment> appointments)
        {
            dgvAppointments.Rows.Clear();

            foreach (var appointment in appointments)
            {
                string type;

                if (appointment is GroupMeeting groupMeeting)
                {
                    // Phân biệt giữa người tạo và người tham gia
                    if (groupMeeting.UserId == _currentUser.Id)
                    {
                        type = "Họp nhóm";
                    }
                    else
                    {
                        type = "Người tham gia";
                    }
                }
                else
                {
                    type = "Cá nhân";
                }

                dgvAppointments.Rows.Add(
                    appointment.Id,
                    appointment.Name,
                    appointment.Location,
                    appointment.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    appointment.EndTime.ToString("dd/MM/yyyy HH:mm"),
                    type
                );
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            _selectedDate = e.Start.Date;
            LoadAppointmentsForDate(_selectedDate);
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            ShowAddAppointmentForm();
        }

        private void addAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddAppointmentForm();
        }

        private void ShowAddAppointmentForm()
        {
            if (_currentUser == null) return;

            var appointmentForm = new AppointmentForm(_currentUser, _selectedDate);
            if (appointmentForm.ShowDialog() == DialogResult.OK)
            {
                LoadAppointmentsForDate(_selectedDate);
            }
        }

        private void btnEditAppointment_Click(object sender, EventArgs e)
        {
            EditSelectedAppointment();
        }

        private void editAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedAppointment();
        }

        private void EditSelectedAppointment()
        {
            if (_currentUser == null || dgvAppointments.SelectedRows.Count == 0) return;

            int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["columnId"].Value);
            var appointment = _calendarService.GetAppointmentsForUser(_currentUser.Id)
                .Where(a => a.Id == appointmentId)
                .FirstOrDefault();

            if (appointment != null)
            {
                // Mở form chỉnh sửa cuộc hẹn
                using (var appointmentForm = new AppointmentForm(_currentUser, appointment))
                {
                    if (appointmentForm.ShowDialog() == DialogResult.OK)
                    {
                        // Làm mới dữ liệu sau khi sửa
                        LoadAppointmentsForDate(_selectedDate);
                    }
                }
            }
        }

        private void btnDeleteAppointment_Click(object sender, EventArgs e)
        {
            DeleteSelectedAppointment();
        }

        private void deleteAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedAppointment();
        }

        private void DeleteSelectedAppointment()
        {
            if (dgvAppointments.SelectedRows.Count == 0) return;

            int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["columnId"].Value);
            string? appointmentName = dgvAppointments.SelectedRows[0].Cells["columnName"].Value?.ToString();

            if (appointmentName != null && MessageBox.Show($"Bạn có chắc muốn xóa lịch hẹn '{appointmentName}'?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_calendarService.DeleteAppointment(appointmentId))
                {
                    MessageBox.Show("Xóa lịch hẹn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointmentsForDate(_selectedDate);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa lịch hẹn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Tắt timer kiểm tra lời nhắc
                _reminderTimer.Enabled = false;
                
                _currentUser = null;
                dgvAppointments.Rows.Clear();
                ShowLoginForm();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void appointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thêmLờiNhắcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddReminderForSelectedAppointment();
        }

        private void AddReminderForSelectedAppointment()
        {
            if (_currentUser == null || dgvAppointments.SelectedRows.Count == 0) return;

            int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["columnId"].Value);
            
            // Gạj các cuộc hẹn thông thường và cả cuộc họp nhóm mà người dùng tham gia
            var appointment = _calendarService.GetAppointmentsForUser(_currentUser.Id)
                .Where(a => a.Id == appointmentId)
                .FirstOrDefault();
        
            // Nếu không tìm thấy trong danh sách cuộc hẹn thông thường, tìm trong danh sách cuộc họp nhóm
            if (appointment == null)
            {
                appointment = _calendarService.GetGroupMeetingsForUser(_currentUser.Id)
                    .Where(gm => gm.Id == appointmentId)
                    .FirstOrDefault();
            }

            if (appointment != null)
            {
                // Mở form thêm lời nhắc
                using (var reminderForm = new ReminderForm(_currentUser, appointment))
                {
                    if (reminderForm.ShowDialog() == DialogResult.OK)
                    {
                        // Thông báo thành công đã được hiển thị trong ReminderForm
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin cuộc hẹn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddReminder_Click(object sender, EventArgs e)
        {
            AddReminderForSelectedAppointment();
        }

        // Handler để xem lời nhắc của cuộc hẹn
        private void lờiNhắcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentUser == null || dgvAppointments.SelectedRows.Count == 0) return;

            int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["columnId"].Value);
            
            // Lấy cuộc hẹn từ ID
            var appointment = _calendarService.GetAppointmentsForUser(_currentUser.Id)
                .FirstOrDefault(a => a.Id == appointmentId);

            if (appointment == null)
            {
                appointment = _calendarService.GetGroupMeetingsForUser(_currentUser.Id)
                    .FirstOrDefault(gm => gm.Id == appointmentId);
            }

            if (appointment != null)
            {
                // Mở form danh sách lời nhắc
                using (var reminderListForm = new ReminderListForm(_currentUser, appointment))
                {
                    reminderListForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin cuộc hẹn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handler để xem người tham gia cuộc họp nhóm
        private void ngườiThamGiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentUser == null || dgvAppointments.SelectedRows.Count == 0) return;

            int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["columnId"].Value);
            
            // Kiểm tra xem đây có phải là cuộc họp nhóm không
            var groupMeeting = _calendarService.GetGroupMeetingsForUser(_currentUser.Id)
                .FirstOrDefault(gm => gm.Id == appointmentId);
            
            if (groupMeeting != null)
            {
                // Mở form danh sách người tham gia
                using (var participantsForm = new ParticipantsListForm(_currentUser, groupMeeting))
                {
                    participantsForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Cuộc hẹn được chọn không phải là cuộc họp nhóm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Cập nhật trạng thái của menu dựa trên lựa chọn hiện tại
        private void UpdateMenuStatus()
        {
            bool hasSelection = dgvAppointments.SelectedRows.Count > 0;
            
            // Kích hoạt/vô hiệu hóa các tùy chọn menu
            editAppointmentToolStripMenuItem.Enabled = hasSelection;
            deleteAppointmentToolStripMenuItem.Enabled = hasSelection;
            thêmLờiNhắcToolStripMenuItem.Enabled = hasSelection;
            lờiNhắcToolStripMenuItem.Enabled = hasSelection;
            
            // Chỉ kích hoạt menu người tham gia nếu đã chọn và là cuộc họp nhóm
            ngườiThamGiaToolStripMenuItem.Enabled = false;
            
            if (hasSelection)
            {
                try
                {
                    int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["columnId"].Value);
                    string typeText = dgvAppointments.SelectedRows[0].Cells["columnType"].Value?.ToString();
                    
                    // Chỉ kích hoạt nếu là cuộc họp nhóm hoặc người tham gia
                    ngườiThamGiaToolStripMenuItem.Enabled = (typeText == "Họp nhóm" || typeText == "Người tham gia");
                }
                catch
                {
                    // Nếu có lỗi, giữ nút bị vô hiệu hóa
                    ngườiThamGiaToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void InitializeReminderTimer()
        {
            // Cấu hình timer để kiểm tra mỗi 60 giây
            _reminderTimer.Interval = 60 * 1000; // 60 seconds
            _reminderTimer.Tick += ReminderTimer_Tick;
            _reminderTimer.Enabled = false; // Sẽ được bật sau khi đăng nhập
        }

        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            CheckDueReminders();
        }

        private void CheckDueReminders()
        {
            if (_currentUser == null) return;
            
            try
            {
                // Lấy thời gian hiện tại
                DateTime now = DateTime.Now;
                // Lấy thời gian trước đó 1 phút để không bỏ sót các lời nhắc
                DateTime oneMinuteAgo = now.AddMinutes(-1);
                
                // Lấy tất cả lời nhắc của người dùng
                var reminders = _calendarService.GetRemindersForUser(_currentUser.Id);
                
                // Lọc các lời nhắc đến hạn (thời gian trong khoảng 1 phút vừa qua)
                var dueReminders = reminders.Where(r => 
                    r.Time >= oneMinuteAgo && 
                    r.Time <= now &&
                    r.Appointment != null).ToList();
                    
                // Hiển thị thông báo cho từng lời nhắc đến hạn
                foreach (var reminder in dueReminders)
                {
                    string appointmentName = reminder.Appointment?.Name ?? "Không rõ";
                    string appointmentTime = reminder.Appointment?.StartTime.ToString("dd/MM/yyyy HH:mm") ?? "Không rõ";
                    
                    // Hiển thị thông báo
                    using (var notificationForm = new NotificationForm(
                        $"Nhắc nhở cuộc hẹn: {appointmentName}",
                        $"Cuộc hẹn sẽ bắt đầu lúc {appointmentTime}\n{reminder.Note}"))
                    {
                        notificationForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nhưng không hiển thị cho người dùng để tránh làm gián đoạn
                System.Diagnostics.Debug.WriteLine($"Lỗi khi kiểm tra lời nhắc: {ex.Message}");
            }
        }

        // Thêm phương thức kiểm tra lời nhắc thủ công (để test)
        private void ManualCheckReminders()
        {
            if (_currentUser != null)
            {
                // Lấy tất cả lời nhắc của người dùng đã đăng nhập
                var reminders = _calendarService.GetRemindersForUser(_currentUser.Id).ToList();
                
                if (reminders.Count == 0)
                {
                    MessageBox.Show("Không có lời nhắc nào được thiết lập.", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                StringBuilder sb = new StringBuilder("Các lời nhắc hiện có:\n\n");
                
                foreach (var reminder in reminders)
                {
                    string appointmentName = reminder.Appointment?.Name ?? "Không rõ";
                    string appointmentTime = reminder.Appointment?.StartTime.ToString("dd/MM/yyyy HH:mm") ?? "Không rõ";
                    
                    sb.AppendLine($"Cuộc hẹn: {appointmentName}");
                    sb.AppendLine($"Thời gian họp: {appointmentTime}");
                    sb.AppendLine($"Thời gian nhắc: {reminder.Time:dd/MM/yyyy HH:mm}");
                    sb.AppendLine($"Ghi chú: {reminder.Note}");
                    sb.AppendLine("---------------------");
                }
                
                MessageBox.Show(sb.ToString(), "Danh sách lời nhắc", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
