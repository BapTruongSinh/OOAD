using System;
using System.Windows.Forms;
using Calendar.Data;
using Calendar.Forms;
using Microsoft.EntityFrameworkCore;

namespace Calendar;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        // Tạo cơ sở dữ liệu nếu chưa tồn tại
        using (var dbContext = new CalendarDbContext())
        {
            try
            {
                // Thay vì sử dụng migration, tạo database trực tiếp
                dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kết nối đến cơ sở dữ liệu: {ex.Message}", 
                    "Lỗi cơ sở dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Form1 sẽ tự xử lý việc đăng nhập và quản lý người dùng
        Application.Run(new Form1());
    }    
}
