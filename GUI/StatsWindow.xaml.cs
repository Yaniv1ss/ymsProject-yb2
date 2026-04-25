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
    public partial class StatsWindow : Window
    {
        private TasksDB tasksDb;

        public StatsWindow()
        {
            InitializeComponent();
            tasksDb = new TasksDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStats();
        }

        private void LoadStats()
        {
            if (Session.CurrentUser is User currentUser)
            {
                // Streak
                txtStreak.Text = $"🔥 {currentUser.Streak} ימים";

                try
                {
                    // Goal progress - משימות שבוצעו
                    var allTasks = tasksDb.SelectAll().Cast<Tasks>().ToList();
                    var userTasks = allTasks.Where(t => t.User_id != null && t.User_id.Id == currentUser.Id).ToList();
                    
                    int completedTasks = userTasks.Count(t => t.Is_done);
                    int goal = currentUser.Goal > 0 ? currentUser.Goal : 1; // מניעת חלוקה באפס
                    
                    pbGoal.Maximum = goal;
                    pbGoal.Value = completedTasks > goal ? goal : completedTasks;
                    txtGoal.Text = $"{completedTasks}/{goal} משימות";

                    double percentage = (double)completedTasks / goal;
                    if (percentage < 0.3)
                    {
                        txtXpLow.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtXpLow.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בטעינת נתונים: " + ex.Message);
                }
            }
        }

        private void btnEditGoal_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.ShowDialog();
            // Reload stats after user potentially changes the goal
            LoadStats();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
