namespace Calendar;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        menuStrip1 = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        logoutToolStripMenuItem = new ToolStripMenuItem();
        exitToolStripMenuItem = new ToolStripMenuItem();
        appointmentsToolStripMenuItem = new ToolStripMenuItem();
        addAppointmentToolStripMenuItem = new ToolStripMenuItem();
        editAppointmentToolStripMenuItem = new ToolStripMenuItem();
        deleteAppointmentToolStripMenuItem = new ToolStripMenuItem();
        splitContainer1 = new SplitContainer();
        monthCalendar1 = new MonthCalendar();
        btnAddAppointment = new Button();
        lblSelectedDate = new Label();
        dgvAppointments = new DataGridView();
        columnId = new DataGridViewTextBoxColumn();
        columnName = new DataGridViewTextBoxColumn();
        columnLocation = new DataGridViewTextBoxColumn();
        columnStartTime = new DataGridViewTextBoxColumn();
        columnEndTime = new DataGridViewTextBoxColumn();
        columnType = new DataGridViewTextBoxColumn();
        btnEditAppointment = new Button();
        btnDeleteAppointment = new Button();
        xemChiTiếtToolStripMenuItem = new ToolStripMenuItem();
        lờiNhắcToolStripMenuItem = new ToolStripMenuItem();
        ngườiThamGiaToolStripMenuItem = new ToolStripMenuItem();
        thêmLờiNhắcToolStripMenuItem = new ToolStripMenuItem();
        btnAddReminder = new Button();
        menuStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAppointments).BeginInit();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new Size(24, 24);
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, appointmentsToolStripMenuItem, xemChiTiếtToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(1379, 33);
        menuStrip1.TabIndex = 0;
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { logoutToolStripMenuItem, exitToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(103, 29);
        fileToolStripMenuItem.Text = "Hệ thống";
        fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
        // 
        // logoutToolStripMenuItem
        // 
        logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
        logoutToolStripMenuItem.Size = new Size(195, 34);
        logoutToolStripMenuItem.Text = "Ðăng xuất";
        logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(195, 34);
        exitToolStripMenuItem.Text = "Thoát";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // appointmentsToolStripMenuItem
        // 
        appointmentsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addAppointmentToolStripMenuItem, editAppointmentToolStripMenuItem, deleteAppointmentToolStripMenuItem, thêmLờiNhắcToolStripMenuItem });
        appointmentsToolStripMenuItem.Name = "appointmentsToolStripMenuItem";
        appointmentsToolStripMenuItem.Size = new Size(92, 29);
        appointmentsToolStripMenuItem.Text = "Lịch hẹn";
        appointmentsToolStripMenuItem.Click += appointmentsToolStripMenuItem_Click;
        // 
        // addAppointmentToolStripMenuItem
        // 
        addAppointmentToolStripMenuItem.Name = "addAppointmentToolStripMenuItem";
        addAppointmentToolStripMenuItem.Size = new Size(270, 34);
        addAppointmentToolStripMenuItem.Text = "Thêm lịch hẹn";
        addAppointmentToolStripMenuItem.Click += addAppointmentToolStripMenuItem_Click;
        // 
        // editAppointmentToolStripMenuItem
        // 
        editAppointmentToolStripMenuItem.Name = "editAppointmentToolStripMenuItem";
        editAppointmentToolStripMenuItem.Size = new Size(270, 34);
        editAppointmentToolStripMenuItem.Text = "Sửa lịch hẹn";
        editAppointmentToolStripMenuItem.Click += editAppointmentToolStripMenuItem_Click;
        // 
        // deleteAppointmentToolStripMenuItem
        // 
        deleteAppointmentToolStripMenuItem.Name = "deleteAppointmentToolStripMenuItem";
        deleteAppointmentToolStripMenuItem.Size = new Size(270, 34);
        deleteAppointmentToolStripMenuItem.Text = "Xóa lịch hẹn";
        deleteAppointmentToolStripMenuItem.Click += deleteAppointmentToolStripMenuItem_Click;
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(monthCalendar1);
        splitContainer1.Panel1.Controls.Add(btnAddAppointment);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(lblSelectedDate);
        splitContainer1.Panel2.Controls.Add(dgvAppointments);
        splitContainer1.Panel2.Controls.Add(btnEditAppointment);
        splitContainer1.Panel2.Controls.Add(btnDeleteAppointment);
        splitContainer1.Panel2.Controls.Add(btnAddReminder);
        splitContainer1.Size = new Size(1379, 457);
        splitContainer1.SplitterDistance = 459;
        splitContainer1.TabIndex = 1;
        // 
        // monthCalendar1
        // 
        monthCalendar1.Location = new Point(0, 0);
        monthCalendar1.MaxSelectionCount = 1;
        monthCalendar1.Name = "monthCalendar1";
        monthCalendar1.TabIndex = 0;
        monthCalendar1.DateChanged += monthCalendar1_DateChanged;
        // 
        // btnAddAppointment
        // 
        btnAddAppointment.Location = new Point(0, 0);
        btnAddAppointment.Name = "btnAddAppointment";
        btnAddAppointment.Size = new Size(75, 23);
        btnAddAppointment.TabIndex = 1;
        btnAddAppointment.Text = "Thêm lịch hẹn";
        btnAddAppointment.Click += btnAddAppointment_Click;
        // 
        // lblSelectedDate
        // 
        lblSelectedDate.Location = new Point(0, 0);
        lblSelectedDate.Name = "lblSelectedDate";
        lblSelectedDate.Size = new Size(100, 23);
        lblSelectedDate.TabIndex = 0;
        lblSelectedDate.Text = "Lịch hẹn ngày hôm nay:";
        // 
        // dgvAppointments
        // 
        dgvAppointments.AllowUserToAddRows = false;
        dgvAppointments.AllowUserToDeleteRows = false;
        dgvAppointments.ColumnHeadersHeight = 34;
        dgvAppointments.Columns.AddRange(new DataGridViewColumn[] { columnId, columnName, columnLocation, columnStartTime, columnEndTime, columnType });
        dgvAppointments.Location = new Point(3, 36);
        dgvAppointments.MultiSelect = false;
        dgvAppointments.Name = "dgvAppointments";
        dgvAppointments.ReadOnly = true;
        dgvAppointments.RowHeadersWidth = 62;
        dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAppointments.Size = new Size(912, 418);
        dgvAppointments.TabIndex = 1;
        // 
        // columnId
        // 
        columnId.HeaderText = "ID";
        columnId.MinimumWidth = 8;
        columnId.Name = "columnId";
        columnId.ReadOnly = true;
        columnId.ToolTipText = "ID";
        columnId.Visible = false;
        columnId.Width = 150;
        // 
        // columnName
        // 
        columnName.HeaderText = "Tên lịch hẹn";
        columnName.MinimumWidth = 8;
        columnName.Name = "columnName";
        columnName.ReadOnly = true;
        columnName.Width = 200;
        // 
        // columnLocation
        // 
        columnLocation.HeaderText = "Địa điểm";
        columnLocation.MinimumWidth = 8;
        columnLocation.Name = "columnLocation";
        columnLocation.ReadOnly = true;
        columnLocation.Width = 200;
        // 
        // columnStartTime
        // 
        columnStartTime.HeaderText = "Thời gian bắt đầu";
        columnStartTime.MinimumWidth = 8;
        columnStartTime.Name = "columnStartTime";
        columnStartTime.ReadOnly = true;
        columnStartTime.Width = 150;
        // 
        // columnEndTime
        // 
        columnEndTime.HeaderText = "Thời gian kết thúc";
        columnEndTime.MinimumWidth = 8;
        columnEndTime.Name = "columnEndTime";
        columnEndTime.ReadOnly = true;
        columnEndTime.Width = 150;
        // 
        // columnType
        // 
        columnType.HeaderText = "Loại Lịch";
        columnType.MinimumWidth = 8;
        columnType.Name = "columnType";
        columnType.ReadOnly = true;
        columnType.Width = 150;
        // 
        // btnEditAppointment
        // 
        btnEditAppointment.Location = new Point(0, 0);
        btnEditAppointment.Name = "btnEditAppointment";
        btnEditAppointment.Size = new Size(75, 23);
        btnEditAppointment.TabIndex = 2;
        btnEditAppointment.Text = "Sửa lịch hẹn";
        btnEditAppointment.Click += btnEditAppointment_Click;
        // 
        // btnDeleteAppointment
        // 
        btnDeleteAppointment.Location = new Point(0, 0);
        btnDeleteAppointment.Name = "btnDeleteAppointment";
        btnDeleteAppointment.Size = new Size(75, 23);
        btnDeleteAppointment.TabIndex = 3;
        btnDeleteAppointment.Text = "Xóa lịch hẹn";
        btnDeleteAppointment.Click += btnDeleteAppointment_Click;
        // 
        // xemChiTiếtToolStripMenuItem
        // 
        xemChiTiếtToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lờiNhắcToolStripMenuItem, ngườiThamGiaToolStripMenuItem });
        xemChiTiếtToolStripMenuItem.Name = "xemChiTiếtToolStripMenuItem";
        xemChiTiếtToolStripMenuItem.Size = new Size(121, 29);
        xemChiTiếtToolStripMenuItem.Text = "Xem chi tiết";
        // 
        // lờiNhắcToolStripMenuItem
        // 
        lờiNhắcToolStripMenuItem.Name = "lờiNhắcToolStripMenuItem";
        lờiNhắcToolStripMenuItem.Size = new Size(270, 34);
        lờiNhắcToolStripMenuItem.Text = "Lời nhắc";
        lờiNhắcToolStripMenuItem.Click += lờiNhắcToolStripMenuItem_Click;
        // 
        // ngườiThamGiaToolStripMenuItem
        // 
        ngườiThamGiaToolStripMenuItem.Name = "ngườiThamGiaToolStripMenuItem";
        ngườiThamGiaToolStripMenuItem.Size = new Size(270, 34);
        ngườiThamGiaToolStripMenuItem.Text = "Người tham gia";
        ngườiThamGiaToolStripMenuItem.Click += ngườiThamGiaToolStripMenuItem_Click;
        // 
        // thêmLờiNhắcToolStripMenuItem
        // 
        thêmLờiNhắcToolStripMenuItem.Name = "thêmLờiNhắcToolStripMenuItem";
        thêmLờiNhắcToolStripMenuItem.Size = new Size(270, 34);
        thêmLờiNhắcToolStripMenuItem.Text = "Thêm lời nhắc";
        thêmLờiNhắcToolStripMenuItem.Click += thêmLờiNhắcToolStripMenuItem_Click;
        // 
        // btnAddReminder
        // 
        btnAddReminder.Location = new Point(277, 460);
        btnAddReminder.Name = "btnAddReminder";
        btnAddReminder.Size = new Size(130, 35);
        btnAddReminder.TabIndex = 4;
        btnAddReminder.Text = "Thêm lời nhắc";
        btnAddReminder.UseVisualStyleBackColor = true;
        btnAddReminder.Click += btnAddReminder_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1379, 457);
        Controls.Add(menuStrip1);
        Controls.Add(splitContainer1);
        MainMenuStrip = menuStrip1;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Lịch";
        Load += Form1_Load;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAppointments).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem appointmentsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addAppointmentToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editAppointmentToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteAppointmentToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.MonthCalendar monthCalendar1;
    private System.Windows.Forms.Button btnAddAppointment;
    private System.Windows.Forms.DataGridView dgvAppointments;
    private System.Windows.Forms.Label lblSelectedDate;
    private System.Windows.Forms.Button btnEditAppointment;
    private System.Windows.Forms.Button btnDeleteAppointment;
    private DataGridViewTextBoxColumn columnId;
    private DataGridViewTextBoxColumn columnName;
    private DataGridViewTextBoxColumn columnLocation;
    private DataGridViewTextBoxColumn columnStartTime;
    private DataGridViewTextBoxColumn columnEndTime;
    private DataGridViewTextBoxColumn columnType;
    private ToolStripMenuItem thêmLờiNhắcToolStripMenuItem;
    private ToolStripMenuItem xemChiTiếtToolStripMenuItem;
    private ToolStripMenuItem lờiNhắcToolStripMenuItem;
    private ToolStripMenuItem ngườiThamGiaToolStripMenuItem;
    private Button btnAddReminder;
}
