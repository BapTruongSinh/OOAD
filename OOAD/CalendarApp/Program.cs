using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CalendarApp.Data;

namespace CalendarApp;

static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Cấu hình services và dependency injection
        ConfigureServices();
        
        // Khởi tạo cơ sở dữ liệu
        DbInitializer.Initialize(ServiceProvider);

        // Chạy ứng dụng với form đăng nhập
        Application.Run(new LoginForm());
    }

    private static void ConfigureServices()
    {
        var services = new ServiceCollection();

        // Đăng ký DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CalendarApp;Trusted_Connection=True;MultipleActiveResultSets=true");
        });

        // Tạo service provider
        ServiceProvider = services.BuildServiceProvider();
    }
}