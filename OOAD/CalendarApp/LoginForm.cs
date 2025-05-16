using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CalendarApp.Data;
using CalendarApp.Models;

namespace CalendarApp
{
    public partial class LoginForm : Form
    {
        // Database context
        private readonly ApplicationDbContext _context;
        private User _currentUser = null;

        public LoginForm()
        {
            // Lấy instance của DbContext từ DI container
            _context = Program.ServiceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Calendar App - Login";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(400, 300);

            // Create controls
            Label lblTitle = new Label
            {
                Text = "Calendar",
                Font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Location = new System.Drawing.Point(50, 20),
                Size = new System.Drawing.Size(300, 40)
            };

            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new System.Drawing.Point(50, 80),
                Size = new System.Drawing.Size(100, 20)
            };

            TextBox txtUsername = new TextBox
            {
                Location = new System.Drawing.Point(150, 80),
                Size = new System.Drawing.Size(200, 20)
            };

            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new System.Drawing.Point(50, 110),
                Size = new System.Drawing.Size(100, 20)
            };

            TextBox txtPassword = new TextBox
            {
                Location = new System.Drawing.Point(150, 110),
                Size = new System.Drawing.Size(200, 20),
                PasswordChar = '*'
            };

            Button btnLogin = new Button
            {
                Text = "Login",
                Location = new System.Drawing.Point(150, 150),
                Size = new System.Drawing.Size(100, 30)
            };

            Button btnRegister = new Button
            {
                Text = "Register",
                Location = new System.Drawing.Point(150, 190),
                Size = new System.Drawing.Size(100, 30)
            };

            // Add event handlers
            btnLogin.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Find user by username and validate password
                User foundUser = _context.Users
                    .Where(u => u.Name.ToLower() == txtUsername.Text.ToLower() && 
                           u.Password == txtPassword.Text)
                    .FirstOrDefault();
                
                if (foundUser != null)
                {
                    _currentUser = foundUser;
                    MainForm mainForm = new MainForm(_currentUser);
                    this.Hide();
                    mainForm.FormClosed += (s, args) => this.Show();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnRegister.Click += (sender, e) =>
            {
                RegisterForm registerForm = new RegisterForm(_context);
                if (registerForm.ShowDialog() == DialogResult.OK)
                {
                    // Successfully registered
                    MessageBox.Show("Registration successful. You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);

            this.ResumeLayout(false);
        }
    }
} 