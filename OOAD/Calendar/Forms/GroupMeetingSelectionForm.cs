using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Calendar.Models;

namespace Calendar.Forms
{
    public partial class GroupMeetingSelectionForm : Form
    {
        private List<GroupMeeting> _groupMeetings;
        public GroupMeeting SelectedGroupMeeting { get; private set; } = null;
        public bool CreateNewAppointment { get; private set; } = false;

        public GroupMeetingSelectionForm(List<GroupMeeting> groupMeetings)
        {
            InitializeComponent();
            _groupMeetings = groupMeetings;
            
            // Cấu hình DataGridView
            ConfigureDataGrid();
            
            // Hiển thị danh sách GroupMeeting
            DisplayGroupMeetings();
        }

        private void ConfigureDataGrid()
        {
            // Cấu hình các cột
            dgvGroupMeetings.AutoGenerateColumns = false;
            dgvGroupMeetings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            // Tạo các cột
            var creatorColumn = new DataGridViewTextBoxColumn
            {
                Name = "Creator",
                HeaderText = "Người tạo",
                Width = 100
            };
            dgvGroupMeetings.Columns.Add(creatorColumn);
            
            var nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Tên cuộc họp",
                Width = 150
            };
            dgvGroupMeetings.Columns.Add(nameColumn);
            
            var locationColumn = new DataGridViewTextBoxColumn
            {
                Name = "Location",
                HeaderText = "Địa điểm",
                Width = 120
            };
            dgvGroupMeetings.Columns.Add(locationColumn);
            
            var startTimeColumn = new DataGridViewTextBoxColumn
            {
                Name = "StartTime",
                HeaderText = "Thời gian bắt đầu",
                Width = 150
            };
            dgvGroupMeetings.Columns.Add(startTimeColumn);
            
            var endTimeColumn = new DataGridViewTextBoxColumn
            {
                Name = "EndTime",
                HeaderText = "Thời gian kết thúc",
                Width = 150
            };
            dgvGroupMeetings.Columns.Add(endTimeColumn);
        }

        private void DisplayGroupMeetings()
        {
            dgvGroupMeetings.Rows.Clear();
            
            foreach (var meeting in _groupMeetings)
            {
                dgvGroupMeetings.Rows.Add(
                    meeting.User?.Name ?? "Unknown",
                    meeting.Name,
                    meeting.Location,
                    meeting.StartTime.ToString("dd/MM/yyyy HH:mm"),
                    meeting.EndTime.ToString("dd/MM/yyyy HH:mm")
                );
            }
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (dgvGroupMeetings.SelectedRows.Count > 0)
            {
                int index = dgvGroupMeetings.SelectedRows[0].Index;
                SelectedGroupMeeting = _groupMeetings[index];
                CreateNewAppointment = false;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một cuộc họp nhóm để tham gia.", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateNewAppointment = true;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
            this.dgvGroupMeetings = new System.Windows.Forms.DataGridView();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupMeetings)).BeginInit();
            this.SuspendLayout();
            
            // lblInfo
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblInfo.Location = new System.Drawing.Point(12, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(470, 20);
            this.lblInfo.Text = "Đã tìm thấy các cuộc họp nhóm trùng. Bạn có muốn tham gia một cuộc họp?";
            
            // dgvGroupMeetings
            this.dgvGroupMeetings.AllowUserToAddRows = false;
            this.dgvGroupMeetings.AllowUserToDeleteRows = false;
            this.dgvGroupMeetings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroupMeetings.Location = new System.Drawing.Point(12, 40);
            this.dgvGroupMeetings.Name = "dgvGroupMeetings";
            this.dgvGroupMeetings.ReadOnly = true;
            this.dgvGroupMeetings.RowHeadersWidth = 51;
            this.dgvGroupMeetings.Size = new System.Drawing.Size(680, 220);
            this.dgvGroupMeetings.TabIndex = 0;
            
            // btnJoin
            this.btnJoin.BackColor = System.Drawing.Color.ForestGreen;
            this.btnJoin.FlatAppearance.BorderSize = 0;
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.ForeColor = System.Drawing.Color.White;
            this.btnJoin.Location = new System.Drawing.Point(150, 270);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(120, 40);
            this.btnJoin.TabIndex = 1;
            this.btnJoin.Text = "Tham gia";
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            
            // btnCreate
            this.btnCreate.BackColor = System.Drawing.Color.Blue;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.Location = new System.Drawing.Point(290, 270);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(200, 40);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Tạo cuộc hẹn mới";
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            
            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(510, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // GroupMeetingSelectionForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 330);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.dgvGroupMeetings);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupMeetingSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Lựa chọn cuộc họp nhóm";
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupMeetings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dgvGroupMeetings;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        #endregion
    }
} 