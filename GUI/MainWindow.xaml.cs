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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel; // Using the ViewModel from the project
using Model; // Using the Model from the project

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserDB userDb;

        public MainWindow()
        {
            InitializeComponent();
            userDb = new UserDB();
            LoadUsers();
        }

        // Method to load data from the Access DB via ViewModel
        private void LoadUsers()
        {
            try
            {
                UserList users = userDb.SelectAll();
                UsersDataGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת הנתונים: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Button click event to refresh the list
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        // Button click event to add a new user
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // Simple validation like a student would do
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || 
                string.IsNullOrWhiteSpace(txtPassword.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("נא למלא את כל השדות החובה שגיאה", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int goal = 0;
            if (!int.TryParse(txtGoal.Text, out goal))
            {
                MessageBox.Show("היעד חייב להיות מספר", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Create a new User object
                User newUser = new User();
                newUser.Username = txtUsername.Text;
                newUser.Pass = txtPassword.Text; // Pass is from People
                newUser.Email = txtEmail.Text;
                newUser.Goal = goal;

                // Insert the new user via ViewModel
                userDb.Insert(newUser);
                int rowsAffected = userDb.SaveChanges();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("המשתמש נוסף בהצלחה!", "הודעה", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Clear the textboxes
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtEmail.Text = "";
                    txtGoal.Text = "";

                    // Reload the grid
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("ההוספה נכשלה, לא בוצע שום שינוי במסד הנתונים.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("אירעה שגיאה במהלך ההוספה: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}