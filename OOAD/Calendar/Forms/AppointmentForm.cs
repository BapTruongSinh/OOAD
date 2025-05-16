using System;
using System.Windows.Forms;
using Calendar.Models;
using Calendar.Services;

namespace Calendar.Forms
{
    public partial class AppointmentForm : Form
    {
        private readonly ICalendarService _calendarService;
        private readonly User _currentUser;
        private readonly DateTime _selectedDate;
        private bool _isGroupMeeting = false;
        private bool _isEditMode = false;
        private Appointment _existingAppointment = null;

        // Constructor cho thêm mới cuộc hẹn
        public AppointmentForm(User user, DateTime selectedDate)
        {
            InitializeComponent();
            _calendarService = new CalendarService();
            _currentUser = user;
            _selectedDate = selectedDate;
            _isEditMode = false;

            // Thiết lập giá trị mặc định
            dtpStartTime.Value = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, DateTime.Now.Hour, 0, 0);
            dtpEndTime.Value = dtpStartTime.Value.AddHours(1);
            
            this.Text = "Thêm cuộc hẹn mới";
        }
        
        // Constructor cho sửa cuộc hẹn
        public AppointmentForm(User user, Appointment appointment)
        {
            InitializeComponent();
            _calendarService = new CalendarService();
            _currentUser = user;
            _selectedDate = appointment.StartTime;
            _isEditMode = true;
            _existingAppointment = appointment;

            // Hiển thị thông tin cuộc hẹn hiện có
            txtName.Text = appointment.Name;
            txtLocation.Text = appointment.Location;
            dtpStartTime.Value = appointment.StartTime;
            dtpEndTime.Value = appointment.EndTime;
            
            // Xác định loại cuộc hẹn
            _isGroupMeeting = appointment is GroupMeeting;
            chkIsGroupMeeting.Checked = _isGroupMeeting;
            chkIsGroupMeeting.Enabled = false; // Không cho phép đổi loại cuộc hẹn khi sửa
            
            this.Text = "Sửa cuộc hẹn";
        }

        private void chkIsGroupMeeting_CheckedChanged(object sender, EventArgs e)
        {
            _isGroupMeeting = chkIsGroupMeeting.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra tên cuộc hẹn
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên cuộc hẹn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            // Kiểm tra thời gian hợp lệ
            if (dtpEndTime.Value <= dtpStartTime.Value)
            {
                MessageBox.Show("Thời gian kết thúc phải sau thời gian bắt đầu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndTime.Focus();
                return;
            }

            try
            {
                if (_isEditMode)
                {
                    // CẬP NHẬT CUỘC HẸN HIỆN CÓ
                    if (_existingAppointment != null)
                    {
                        // Lưu tên cũ để kiểm tra nếu người dùng đổi tên
                        string oldName = _existingAppointment.Name;
                        
                        // Tạo đối tượng tạm để kiểm tra trùng tên nếu tên thay đổi
                        if (oldName != txtName.Text && _calendarService.CheckDuplicateName(new Appointment 
                        { 
                            Id = _existingAppointment.Id, 
                            Name = txtName.Text,
                            UserId = _currentUser.Id 
                        }))
                        {
                            MessageBox.Show("Tên cuộc hẹn đã tồn tại. Vui lòng chọn tên khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtName.Focus();
                            return;
                        }
                        
                        // Cập nhật thông tin
                        _existingAppointment.Name = txtName.Text;
                        _existingAppointment.Location = txtLocation.Text;
                        _existingAppointment.StartTime = dtpStartTime.Value;
                        _existingAppointment.EndTime = dtpEndTime.Value;
                        
                        // Kiểm tra trùng lịch nếu thời gian thay đổi
                        var overlappingAppointment = _calendarService.FindOverlappingAppointment(_existingAppointment);
                        if (overlappingAppointment != null)
                        {
                            var confirmResult = MessageBox.Show(
                                $"Cuộc hẹn này bị trùng thời gian với cuộc hẹn \"{overlappingAppointment.Name}\" " +
                                $"({overlappingAppointment.StartTime:HH:mm} - {overlappingAppointment.EndTime:HH:mm}). " +
                                $"Bạn có muốn thay thế cuộc hẹn cũ không?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question
                            );

                            if (confirmResult == DialogResult.Yes)
                            {
                                // Xóa cuộc hẹn cũ nếu người dùng đồng ý thay thế
                                _calendarService.DeleteAppointment(overlappingAppointment.Id);
                            }
                            else
                            {
                                // Giữ form mở để người dùng có thể chỉnh sửa thời gian
                                dtpStartTime.Focus();
                                return;
                            }
                        }
                        
                        // Lưu thay đổi vào cơ sở dữ liệu
                        bool success = _calendarService.UpdateAppointment(_existingAppointment);
                        if (success)
                        {
                            MessageBox.Show("Đã cập nhật cuộc hẹn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật cuộc hẹn. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    // THÊM MỚI CUỘC HẸN
                    // Tạo đối tượng tạm để kiểm tra trùng tên
                    var tempAppointment = new Appointment 
                    { 
                        Name = txtName.Text, 
                        UserId = _currentUser.Id,
                        StartTime = dtpStartTime.Value,
                        EndTime = dtpEndTime.Value
                    };
                    
                    // Kiểm tra có cuộc họp nhóm nào trùng tên VÀ thời gian hay không
                    var matchingGroupMeetings = _calendarService.FindMatchingGroupMeetings(txtName.Text, dtpStartTime.Value, dtpEndTime.Value).ToList();
                    
                    if (matchingGroupMeetings.Count > 0)
                    {
                        // Hiển thị form chọn cuộc họp nhóm
                        using (var selectionForm = new GroupMeetingSelectionForm(matchingGroupMeetings))
                        {
                            if (selectionForm.ShowDialog() == DialogResult.OK)
                            {
                                if (selectionForm.CreateNewAppointment)
                                {
                                    // Người dùng chọn tạo cuộc hẹn mới, tiếp tục như bình thường
                                }
                                else if (selectionForm.SelectedGroupMeeting != null)
                                {
                                    // Người dùng chọn tham gia vào cuộc họp nhóm
                                    var groupMeeting = selectionForm.SelectedGroupMeeting;
                                    bool joinSuccess = _calendarService.AddUserToGroupMeeting(groupMeeting.Id, _currentUser.Id);
                                    
                                    if (joinSuccess)
                                    {
                                        MessageBox.Show($"Đã tham gia cuộc họp nhóm '{groupMeeting.Name}' thành công!", 
                                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        
                                        DialogResult = DialogResult.OK;
                                        Close();
                                        return;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Không thể tham gia cuộc họp nhóm. Vui lòng thử lại!", 
                                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                // Người dùng đã hủy
                                return;
                            }
                        }
                    }
                    else
                    {
                        // Kiểm tra trùng tên với các cuộc hẹn cá nhân
                        if (_calendarService.CheckDuplicateName(tempAppointment))
                        {
                            MessageBox.Show("Tên cuộc hẹn đã tồn tại. Vui lòng chọn tên khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtName.Focus();
                            return;
                        }
                        
                        // Kiểm tra trùng thời gian
                        var overlappingAppointment = _calendarService.FindOverlappingAppointment(tempAppointment);
                        if (overlappingAppointment != null)
                        {
                            var confirmResult = MessageBox.Show(
                                $"Cuộc hẹn này bị trùng thời gian với cuộc hẹn \"{overlappingAppointment.Name}\" " +
                                $"({overlappingAppointment.StartTime:HH:mm} - {overlappingAppointment.EndTime:HH:mm}). " +
                                $"Bạn có muốn thay thế cuộc hẹn cũ không?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question
                            );

                            if (confirmResult == DialogResult.Yes)
                            {
                                // Xóa cuộc hẹn cũ nếu người dùng đồng ý thay thế
                                _calendarService.DeleteAppointment(overlappingAppointment.Id);
                            }
                            else
                            {
                                // Giữ form mở để người dùng có thể chỉnh sửa thời gian
                                dtpStartTime.Focus();
                                return;
                            }
                        }
                    }
                    
                    if (_isGroupMeeting)
                    {
                        // Tạo cuộc họp nhóm
                        var groupMeeting = new GroupMeeting
                        {
                            Name = txtName.Text,
                            Location = txtLocation.Text,
                            StartTime = dtpStartTime.Value,
                            EndTime = dtpEndTime.Value,
                            UserId = _currentUser.Id
                        };

                        // Thêm vào cơ sở dữ liệu
                        _calendarService.AddGroupMeeting(groupMeeting);

                        // Thêm người dùng hiện tại vào danh sách người tham gia
                        _calendarService.AddUserToGroupMeeting(groupMeeting.Id, _currentUser.Id);

                        MessageBox.Show("Đã tạo cuộc họp nhóm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Tạo cuộc hẹn cá nhân
                        var appointment = new Appointment
                        {
                            Name = txtName.Text,
                            Location = txtLocation.Text,
                            StartTime = dtpStartTime.Value,
                            EndTime = dtpEndTime.Value,
                            UserId = _currentUser.Id
                        };

                        // Thêm vào cơ sở dữ liệu
                        _calendarService.AddAppointment(appointment);

                        MessageBox.Show("Đã tạo cuộc hẹn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMsg += "\nChi tiết lỗi: " + ex.InnerException.Message;
                }
                
                MessageBox.Show($"Lỗi khi {(_isEditMode ? "sửa" : "tạo")} cuộc hẹn:\n{errorMsg}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 
