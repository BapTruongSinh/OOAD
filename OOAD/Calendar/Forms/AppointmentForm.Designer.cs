namespace Calendar.Forms
{
    partial class AppointmentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            txtLocation = new TextBox();
            label3 = new Label();
            dtpStartTime = new DateTimePicker();
            label4 = new Label();
            dtpEndTime = new DateTimePicker();
            chkIsGroupMeeting = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            label5 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 19);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(118, 25);
            label1.TabIndex = 0;
            label1.Text = "Tên cuộc hẹn:";
            // 
            // txtName
            // 
            txtName.Location = new Point(241, 19);
            txtName.Margin = new Padding(4, 4, 4, 4);
            txtName.Name = "txtName";
            txtName.Size = new Size(414, 31);
            txtName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 69);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(87, 25);
            label2.TabIndex = 2;
            label2.Text = "Địa điểm:";
            // 
            // txtLocation
            // 
            txtLocation.Location = new Point(241, 66);
            txtLocation.Margin = new Padding(4, 4, 4, 4);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(414, 31);
            txtLocation.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 119);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(155, 25);
            label3.TabIndex = 4;
            label3.Text = "Thời gian bắt đầu:";
            // 
            // dtpStartTime
            // 
            dtpStartTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpStartTime.Format = DateTimePickerFormat.Custom;
            dtpStartTime.Location = new Point(241, 114);
            dtpStartTime.Margin = new Padding(4, 4, 4, 4);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.Size = new Size(414, 31);
            dtpStartTime.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 169);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(157, 25);
            label4.TabIndex = 6;
            label4.Text = "Thời gian kết thúc:";
            // 
            // dtpEndTime
            // 
            dtpEndTime.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpEndTime.Format = DateTimePickerFormat.Custom;
            dtpEndTime.Location = new Point(241, 164);
            dtpEndTime.Margin = new Padding(4, 4, 4, 4);
            dtpEndTime.Name = "dtpEndTime";
            dtpEndTime.Size = new Size(414, 31);
            dtpEndTime.TabIndex = 7;
            // 
            // chkIsGroupMeeting
            // 
            chkIsGroupMeeting.AutoSize = true;
            chkIsGroupMeeting.Location = new Point(241, 219);
            chkIsGroupMeeting.Margin = new Padding(4, 4, 4, 4);
            chkIsGroupMeeting.Name = "chkIsGroupMeeting";
            chkIsGroupMeeting.Size = new Size(218, 29);
            chkIsGroupMeeting.TabIndex = 8;
            chkIsGroupMeeting.Text = "Đây là cuộc họp nhóm";
            chkIsGroupMeeting.UseVisualStyleBackColor = true;
            chkIsGroupMeeting.CheckedChanged += chkIsGroupMeeting_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 219);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(121, 25);
            label5.TabIndex = 11;
            label5.Text = "Loại cuộc hẹn:";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.ForestGreen;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(192, 276);
            btnSave.Margin = new Padding(4, 4, 4, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(132, 44);
            btnSave.TabIndex = 12;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Gray;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(364, 276);
            btnCancel.Margin = new Padding(4, 4, 4, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(132, 44);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // AppointmentForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(696, 347);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(label5);
            Controls.Add(chkIsGroupMeeting);
            Controls.Add(dtpEndTime);
            Controls.Add(label4);
            Controls.Add(dtpStartTime);
            Controls.Add(label3);
            Controls.Add(txtLocation);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AppointmentForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thêm cuộc hẹn mới";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.CheckBox chkIsGroupMeeting;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
    }
} 
