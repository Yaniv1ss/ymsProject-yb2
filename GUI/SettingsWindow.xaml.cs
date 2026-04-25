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
    public partial class SettingsWindow : Window
    {
        private UserDB userDb;
        private User currentUser;

        public SettingsWindow()
        {
            InitializeComponent();
            userDb = new UserDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser is User u)
            {
                currentUser = u;
                txtUsername.Text = u.Username;
                txtEmail.Text = u.Email;
                txtGoal.Text = u.Goal.ToString();
            }
            else
            {
                MessageBox.Show("רק תלמיד יכול לערוך הגדרות אישיות בעמוד זה.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("לא ניתן להשאיר שם משתמש ריק.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int goal = 85;
                if (!int.TryParse(txtGoal.Text, out goal))
                {
                    MessageBox.Show("היעד חייב להיות מספר.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Apply changes to the object
                currentUser.Username = txtUsername.Text;
                currentUser.Email = txtEmail.Text;
                currentUser.Goal = goal;

                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    currentUser.Pass = txtPassword.Text;
                }

                userDb.Update(currentUser);
                int result = userDb.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show("הנתונים עודכנו בהצלחה!", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Session is automatically updated since we modified the very object it points to
                    this.Close();
                }
                else
                {
                    MessageBox.Show("אירעה שגיאה. הנתונים לא נשמרו.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאת מסד נתונים: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string colorHex)
            {
                try
                {
                    Color color = (Color)ColorConverter.ConvertFromString(colorHex);
                    Application.Current.Resources["PrimaryColor"] = color;
                    Application.Current.Resources["PrimaryBrush"] = new SolidColorBrush(color);
                    
                    // Persist the choice
                    App.SaveTheme(colorHex);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error changing theme: " + ex.Message);
                }
            }
        }
    }
}
