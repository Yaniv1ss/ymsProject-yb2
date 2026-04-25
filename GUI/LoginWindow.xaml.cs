using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModel;
using Model;

namespace GUI
{
    public partial class LoginWindow : Window
    {
        private UserDB userDb;
        private ManagerDB managerDb;

        public LoginWindow()
        {
            InitializeComponent();
            userDb = new UserDB();
            managerDb = new ManagerDB();
            
            try
            {
                SubjectDB.EnsureDefaultSubjects();
            }
            catch { }
        }

        private void btnLogin_Click(object? sender, RoutedEventArgs e)
        {
            if (txtUsername == null || txtPassword == null) return;
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("נא למלא שם משתמש וסיסמה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Fetch all users to verify credentials
                UserList users = userDb.SelectAll();
                User? currentUser = users.Find(u => u.Username == username && u.Pass == password);

                if (currentUser != null)
                {
                    // Check if user is ALSO a manager
                    ManagerList managers = managerDb.SelectAll();
                    Manager? currentManager = managers.Find(m => m.Id == currentUser.Id);

                    Session.CurrentUser = currentUser;
                    Session.IsAdmin = (currentManager != null); // User is an admin if they exist in Manager table

                    if (Session.IsAdmin)
                    {
                        AdminDashboardWindow adminDashboard = new AdminDashboardWindow();
                        adminDashboard.Show();
                    }
                    else
                    {
                        DashboardWindow dashboard = new DashboardWindow();
                        dashboard.Show();
                    }
                    this.Close(); // Close login window
                }
                else
                {
                    MessageBox.Show("שם משתמש או סיסמה שגויים", "שגיאת התחברות", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בהתחברות למסד הנתונים: " + ex.Message, "שגיאת מערכת", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdmin_Click(object? sender, RoutedEventArgs e)
        {
            if (txtUsername == null || txtPassword == null) return;
            // Super Admin hardcoded override
            if (txtUsername.Text == "admin" && txtPassword.Password == "sillyfemboy2409")
            {
                Session.CurrentUser = new Manager { Username = "admin", Id = -1 }; // Dummy admin user
                Session.IsAdmin = true;
                
                AdminDashboardWindow adminDashboard = new AdminDashboardWindow();
                adminDashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("כניסת מנהל (Super Admin):\nיש להזין בשם משתמש 'admin' ובסיסמה את סיסמת מנהל המערכת, ולאחר מכן ללחוץ על הלינק שוב.", "הדרכה", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnRegister_Click(object? sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Close();
        }

        private void btnClose_Click(object? sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnAdminLogin_Click(object? sender, RoutedEventArgs e)
        {
            if (AdminCodeOverlay != null && txtAdminCode != null)
            {
                AdminCodeOverlay.Visibility = Visibility.Visible;
                txtAdminCode.Clear();
                txtAdminCode.Focus();
            }
        }

        private void btnCancelAdmin_Click(object? sender, RoutedEventArgs e)
        {
            if (AdminCodeOverlay != null) AdminCodeOverlay.Visibility = Visibility.Collapsed;
        }

        private void btnConfirmAdmin_Click(object? sender, RoutedEventArgs e)
        {
            if (txtAdminCode == null) return;
            if (txtAdminCode.Password == "sillyfemboy2409")
            {
                Session.CurrentUser = new Manager { Username = "admin", Id = -1 };
                Session.IsAdmin = true;
                AdminDashboardWindow admin = new AdminDashboardWindow();
                admin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("קוד שגוי!", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Allow dragging the window
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
