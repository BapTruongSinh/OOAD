using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using CalendarApp.Data;
using CalendarApp.Models;

namespace CalendarApp
{
    public partial class RegisterForm : Form
    {
        private readonly ApplicationDbContext _context;

        public RegisterForm(ApplicationDbContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "Register New User";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(350, 250);
            this.DialogResult = DialogResult.Cancel;

            // Create controls
            Label lblName = new Label
            {
                Text = "Name:",
                Location = new System.Drawing.Point(30, 30),
                Size = new System.Drawing.Size(80, 20)
            };

            TextBox txtName = new TextBox
            {
                Location = new System.Drawing.Point(120, 30),
                Size = new System.Drawing.Size(180, 20)
            };

            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new System.Drawing.Point(30, 60),
                Size = new System.Drawing.Size(80, 20)
            };

            TextBox txtPassword = new TextBox
            {
                Location = new System.Drawing.Point(120, 60),
                Size = new System.Drawing.Size(180, 20),
                PasswordChar = '*'
            };

            Label lblConfirmPassword = new Label
            {
                Text = "Confirm:",
                Location = new System.Drawing.Point(30, 90),
                Size = new System.Drawing.Size(80, 20)
            };

            TextBox txtConfirmPassword = new TextBox
            {
                Location = new System.Drawing.Point(120, 90),
                Size = new System.Drawing.Size(180, 20),
                PasswordChar = '*'
            };

            Button btnRegister = new Button
            {
                Text = "Register",
                Location = new System.Drawing.Point(120, 130),
                Size = new System.Drawing.Size(100, 30)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(120, 170),
                Size = new System.Drawing.Size(100, 30)
            };

            // Add event handlers
            btnRegister.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Please enter a name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if username already exists in the database
                if (_context.Users.Any(u => u.Name.Equals(txtName.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Username already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create new user
                User newUser = new User
                { 
                    Name = txtName.Text,
                    Password = txtPassword.Text // Store the password
                };
                
                // Thêm vào database
                _context.Users.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            btnCancel.Click += (sender, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Add controls to form
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnCancel);

            this.ResumeLayout(false);
        }
    }
} 