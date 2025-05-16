using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Calendar.Models;
using Calendar.Services;

namespace Calendar.Forms
{
    public partial class ReminderListForm : Form
    {
        private readonly ICalendarService _calendarService;
        private readonly User _currentUser;
        private readonly Appointment _appointment;
        private List<Reminder> _reminders;

        public ReminderListForm(User user, Appointment appointment)
        {
            InitializeComponent();
            _calendarService = new CalendarService();
            _currentUser = user;
            _appointment = appointment;
            
            // Thiết lập thông tin cuộc hẹn
            this.Text = $"Lời nhắc cho: {_appointment.Name}";
            lblAppointmentInfo.Text = $"Cuộc hẹn: {_appointment.Name} ({_appointment.StartTime:dd/MM/yyyy HH:mm} - {_appointment.EndTime:dd/MM/yyyy HH:mm})";
            
            // Load danh sách lời nhắc
            LoadReminders();
        }
        
        private void LoadReminders()
        {
            // Lấy danh sách lời nhắc cho cuộc hẹn này
            _reminders = _calendarService.GetRemindersForUser(_currentUser.Id)
                .Where(r => r.AppointmentId == _appointment.Id)
                .ToList();
            
            // Xóa dữ liệu cũ và cập nhật lại DataGridView
            dgvReminders.Rows.Clear();
            
            foreach (var reminder in _reminders)
            {
                dgvReminders.Rows.Add(
                    reminder.Id,
                    reminder.Time.ToString("dd/MM/yyyy HH:mm"),
                    reminder.Note
                );
            }
            
            // Cập nhật trạng thái các nút
            UpdateButtonStatus();
        }
        
        private void UpdateButtonStatus()
        {
            bool hasSelection = dgvReminders.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }
        
        private void dgvReminders_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Mở form thêm lời nhắc mới
            using (var reminderForm = new ReminderForm(_currentUser, _appointment))
            {
                if (reminderForm.ShowDialog() == DialogResult.OK)
                {
                    // Nạp lại danh sách lời nhắc
                    LoadReminders();
                }
            }
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvReminders.SelectedRows.Count == 0) return;
            
            int reminderId = Convert.ToInt32(dgvReminders.SelectedRows[0].Cells["columnId"].Value);
            var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
            
            if (reminder != null)
            {
                // Mở form chỉnh sửa lời nhắc
                using (var editForm = new ReminderEditForm(_currentUser, reminder, _appointment))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Nạp lại danh sách lời nhắc
                        LoadReminders();
                    }
                }
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReminders.SelectedRows.Count == 0) return;
            
            int reminderId = Convert.ToInt32(dgvReminders.SelectedRows[0].Cells["columnId"].Value);
            string reminderTime = dgvReminders.SelectedRows[0].Cells["columnTime"].Value.ToString();
            
            // Hiện hộp thoại xác nhận xóa
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc muốn xóa lời nhắc vào lúc {reminderTime}?",
                "Xác nhận xóa", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                
            if (confirmResult == DialogResult.Yes)
            {
                // Thực hiện xóa lời nhắc
                bool success = _calendarService.DeleteReminder(reminderId);
                
                if (success)
                {
                    MessageBox.Show("Đã xóa lời nhắc thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Nạp lại danh sách lời nhắc
                    LoadReminders();
                }
                else
                {
                    MessageBox.Show("Không thể xóa lời nhắc. Vui lòng thử lại!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
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
            this.dgvReminders = new System.Windows.Forms.DataGridView();
            this.columnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).BeginInit();
            this.SuspendLayout();
            
            // lblAppointmentInfo
            this.lblAppointmentInfo.AutoSize = true;
            this.lblAppointmentInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAppointmentInfo.Location = new System.Drawing.Point(12, 9);
            this.lblAppointmentInfo.Name = "lblAppointmentInfo";
            this.lblAppointmentInfo.Size = new System.Drawing.Size(103, 20);
            this.lblAppointmentInfo.TabIndex = 0;
            this.lblAppointmentInfo.Text = "Thông tin cuộc hẹn";
            
            // dgvReminders
            this.dgvReminders.AllowUserToAddRows = false;
            this.dgvReminders.AllowUserToDeleteRows = false;
            this.dgvReminders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReminders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.columnId,
                this.columnTime,
                this.columnNote
            });
            this.dgvReminders.Location = new System.Drawing.Point(12, 40);
            this.dgvReminders.MultiSelect = false;
            this.dgvReminders.Name = "dgvReminders";
            this.dgvReminders.ReadOnly = true;
            this.dgvReminders.RowHeadersWidth = 51;
            this.dgvReminders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReminders.Size = new System.Drawing.Size(776, 300);
            this.dgvReminders.TabIndex = 1;
            this.dgvReminders.SelectionChanged += new System.EventHandler(this.dgvReminders_SelectionChanged);
            
            // columnId
            this.columnId.HeaderText = "ID";
            this.columnId.MinimumWidth = 6;
            this.columnId.Name = "columnId";
            this.columnId.ReadOnly = true;
            this.columnId.Visible = false;
            this.columnId.Width = 125;
            
            // columnTime
            this.columnTime.HeaderText = "Thời gian nhắc nhở";
            this.columnTime.MinimumWidth = 6;
            this.columnTime.Name = "columnTime";
            this.columnTime.ReadOnly = true;
            this.columnTime.Width = 200;
            
            // columnNote
            this.columnNote.HeaderText = "Ghi chú";
            this.columnNote.MinimumWidth = 6;
            this.columnNote.Name = "columnNote";
            this.columnNote.ReadOnly = true;
            this.columnNote.Width = 400;
            
            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(12, 353);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 35);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Thêm mới";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            // btnEdit
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(138, 353);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(120, 35);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Chỉnh sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            
            // btnDelete
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(264, 353);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 35);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            // btnClose
            this.btnClose.Location = new System.Drawing.Point(668, 353);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // ReminderListForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.lblAppointmentInfo);
            this.Controls.Add(this.dgvReminders);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReminderListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Danh sách lời nhắc";
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblAppointmentInfo;
        private System.Windows.Forms.DataGridView dgvReminders;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNote;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        #endregion
    }
} 