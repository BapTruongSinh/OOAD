using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Calendar.Models;
using Calendar.Services;

namespace Calendar.Forms
{
    public partial class ParticipantsListForm : Form
    {
        private readonly ICalendarService _calendarService;
        private readonly User _currentUser;
        private GroupMeeting _groupMeeting;
        private List<User> _participants;
        private bool _isCreator;

        public ParticipantsListForm(User user, GroupMeeting groupMeeting)
        {
            InitializeComponent();
            _calendarService = new CalendarService();
            _currentUser = user;
            _groupMeeting = groupMeeting;
            _isCreator = (_currentUser.Id == _groupMeeting.UserId);
            
            // Thiết lập thông tin cuộc họp
            this.Text = $"Người tham gia: {_groupMeeting.Name}";
            lblMeetingInfo.Text = $"Cuộc họp: {_groupMeeting.Name} ({_groupMeeting.StartTime:dd/MM/yyyy HH:mm} - {_groupMeeting.EndTime:dd/MM/yyyy HH:mm})";
            
            // Nút xóa người tham gia chỉ hiển thị cho người tạo cuộc họp
            btnRemove.Visible = _isCreator;
            
            // Tải danh sách người tham gia
            LoadParticipants();
        }
        
        private void LoadParticipants()
        {
            // Reload group meeting from database to get current participants
            var refreshedGroupMeeting = _calendarService.GetGroupMeetingsForUser(_currentUser.Id)
                .FirstOrDefault(gm => gm.Id == _groupMeeting.Id);
        
            if (refreshedGroupMeeting != null)
            {
                // Update the reference to use the refreshed data
                _participants = refreshedGroupMeeting.Users.ToList();
            }
            else
            {
                // Fallback to cached data if can't reload
                _participants = _groupMeeting.Users.ToList();
            }
            
            // Xóa dữ liệu cũ và cập nhật lại DataGridView
            dgvParticipants.Rows.Clear();
            
            foreach (var participant in _participants)
            {
                string role = participant.Id == _groupMeeting.UserId ? "Người tạo" : "Người tham gia";
                bool isCreator = participant.Id == _groupMeeting.UserId;
                
                dgvParticipants.Rows.Add(
                    participant.Id,
                    participant.Name,
                    role,
                    isCreator
                );
            }
            
            // Cập nhật trạng thái nút
            UpdateButtonStatus();
        }
        
        private void UpdateButtonStatus()
        {
            if (!_isCreator)
            {
                btnRemove.Enabled = false;
                return;
            }
            
            if (dgvParticipants.SelectedRows.Count == 0)
            {
                btnRemove.Enabled = false;
                return;
            }
            
            // Kiểm tra xem hàng được chọn có phải là người tạo hay không
            bool isCreator = Convert.ToBoolean(dgvParticipants.SelectedRows[0].Cells["columnIsCreator"].Value);
            
            // Nếu người được chọn là người tạo (chính người dùng hiện tại), không cho phép xóa
            btnRemove.Enabled = !isCreator;
        }
        
        private void dgvParticipants_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }
        
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!_isCreator || dgvParticipants.SelectedRows.Count == 0) return;
            
            int participantId = Convert.ToInt32(dgvParticipants.SelectedRows[0].Cells["columnId"].Value);
            string participantName = dgvParticipants.SelectedRows[0].Cells["columnName"].Value.ToString();
            bool isCreator = Convert.ToBoolean(dgvParticipants.SelectedRows[0].Cells["columnIsCreator"].Value);
            
            // Kiểm tra xem người dùng có đang cố gắng xóa chính mình không (người tạo)
            if (isCreator || participantId == _currentUser.Id)
            {
                MessageBox.Show("Bạn không thể xóa người tạo cuộc họp!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Hiện hộp thoại xác nhận xóa
            var confirmResult = MessageBox.Show(
                $"Bạn có chắc muốn xóa {participantName} khỏi cuộc họp này?",
                "Xác nhận xóa", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                
            if (confirmResult == DialogResult.Yes)
            {
                // Thực hiện xóa người tham gia
                bool success = _calendarService.RemoveUserFromGroupMeeting(_groupMeeting.Id, participantId);
                
                if (success)
                {
                    MessageBox.Show($"Đã xóa {participantName} khỏi cuộc họp thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Get an updated version of the group meeting
                    var refreshedMeeting = _calendarService.GetGroupMeetingsForUser(_currentUser.Id)
                        .FirstOrDefault(gm => gm.Id == _groupMeeting.Id);
                        
                    if (refreshedMeeting != null)
                    {
                        // Update our reference with the refreshed data
                        _groupMeeting = refreshedMeeting;
                    }
                    
                    // Nạp lại danh sách người tham gia
                    LoadParticipants();
                }
                else
                {
                    MessageBox.Show("Không thể xóa người tham gia. Vui lòng thử lại!", "Lỗi", 
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
            this.lblMeetingInfo = new System.Windows.Forms.Label();
            this.dgvParticipants = new System.Windows.Forms.DataGridView();
            this.columnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnIsCreator = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).BeginInit();
            this.SuspendLayout();
            
            // lblMeetingInfo
            this.lblMeetingInfo.AutoSize = true;
            this.lblMeetingInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMeetingInfo.Location = new System.Drawing.Point(12, 9);
            this.lblMeetingInfo.Name = "lblMeetingInfo";
            this.lblMeetingInfo.Size = new System.Drawing.Size(103, 20);
            this.lblMeetingInfo.TabIndex = 0;
            this.lblMeetingInfo.Text = "Thông tin cuộc họp";
            
            // dgvParticipants
            this.dgvParticipants.AllowUserToAddRows = false;
            this.dgvParticipants.AllowUserToDeleteRows = false;
            this.dgvParticipants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParticipants.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.columnId,
                this.columnName,
                this.columnRole,
                this.columnIsCreator
            });
            this.dgvParticipants.Location = new System.Drawing.Point(12, 40);
            this.dgvParticipants.MultiSelect = false;
            this.dgvParticipants.Name = "dgvParticipants";
            this.dgvParticipants.ReadOnly = true;
            this.dgvParticipants.RowHeadersWidth = 51;
            this.dgvParticipants.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvParticipants.Size = new System.Drawing.Size(576, 300);
            this.dgvParticipants.TabIndex = 1;
            this.dgvParticipants.SelectionChanged += new System.EventHandler(this.dgvParticipants_SelectionChanged);
            
            // columnId
            this.columnId.HeaderText = "ID";
            this.columnId.MinimumWidth = 6;
            this.columnId.Name = "columnId";
            this.columnId.ReadOnly = true;
            this.columnId.Visible = false;
            this.columnId.Width = 125;
            
            // columnName
            this.columnName.HeaderText = "Tên người dùng";
            this.columnName.MinimumWidth = 6;
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            this.columnName.Width = 300;
            
            // columnRole
            this.columnRole.HeaderText = "Vai trò";
            this.columnRole.MinimumWidth = 6;
            this.columnRole.Name = "columnRole";
            this.columnRole.ReadOnly = true;
            this.columnRole.Width = 200;
            
            // columnIsCreator
            this.columnIsCreator.HeaderText = "Là người tạo";
            this.columnIsCreator.MinimumWidth = 6;
            this.columnIsCreator.Name = "columnIsCreator";
            this.columnIsCreator.ReadOnly = true;
            this.columnIsCreator.Visible = false;
            this.columnIsCreator.Width = 125;
            
            // btnRemove
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(12, 353);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(200, 35);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Xóa khỏi cuộc họp";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            
            // btnClose
            this.btnClose.Location = new System.Drawing.Point(468, 353);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // ParticipantsListForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.lblMeetingInfo);
            this.Controls.Add(this.dgvParticipants);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParticipantsListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Danh sách người tham gia";
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticipants)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblMeetingInfo;
        private System.Windows.Forms.DataGridView dgvParticipants;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRole;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnIsCreator;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnClose;
        #endregion
    }
} 