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
    public partial class RegisterWindow : Window
    {
        private UserDB userDb;

        public RegisterWindow()
        {
            InitializeComponent();
            userDb = new UserDB();
        }

        private void btnRegister_Click(object? sender, RoutedEventArgs e)
        {
            if (txtUsername == null || txtPassword == null || txtEmail == null || txtGoal == null) return;
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Password;
                string email = txtEmail.Text;
                
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("שם משתמש וסיסמה הם שדות חובה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int goal = 85;
                if (!int.TryParse(txtGoal.Text, out goal))
                {
                    MessageBox.Show("היעד חייב להיות מספר", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check if user exists (to prevent duplicates using the same username)
                UserList users = userDb.SelectAll();
                if (users.Any(u => u.Username == username))
                {
                    MessageBox.Show("שם המשתמש כבר קיים במערכת, בחר שם אחר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                User newUser = new User()
                {
                    Username = username,
                    Pass = password,
                    Email = email,
                    Goal = goal
                };

                userDb.Insert(newUser);
                int result = userDb.SaveChanges(); // This will execute PeopleDB insert, grab ID, then UserDB insert

                if (result > 0)
                {
                    MessageBox.Show("המשתמש נוצר בהצלחה! כעת תוכל להתחבר.", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    btnBackToLogin_Click(null, e);
                }
                else
                {
                    MessageBox.Show("שגיאה ביצירת משתמש במסד הנתונים.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה במערכת: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBackToLogin_Click(object? sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
