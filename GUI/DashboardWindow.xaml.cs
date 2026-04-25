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
using Model;

namespace GUI
{
    public partial class DashboardWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer? timer;

        public DashboardWindow()
        {
            InitializeComponent();
            StartClock();
        }

        private void StartClock()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (txtTime != null) txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Session.IsLoggedIn() && Session.CurrentUser != null)
            {
                txtWelcome.Text = $"שלום, {Session.CurrentUser.Username}!";
                if (Session.CurrentUser is User u)
                {
                    txtStreak.Text = $"🔥 רצף: {u.Streak}";
                }
            }
            else
            {
                MessageBox.Show("שגיאת התחברות. חוזר למסך כניסה.");
                btnLogout_Click(null, null);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.Logout();
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            TasksWindow tasksWindow = new TasksWindow();
            tasksWindow.ShowDialog();
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            ScheduleWindow scheduleWindow = new ScheduleWindow();
            scheduleWindow.ShowDialog();
        }

        private void btnExams_Click(object sender, RoutedEventArgs e)
        {
            ExamsWindow examsWindow = new ExamsWindow();
            examsWindow.ShowDialog();
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.ShowDialog();
            
            if (Session.CurrentUser is User u)
            {
                txtWelcome.Text = $"שלום, {u.Username}!";
                txtStreak.Text = $"🔥 רצף: {u.Streak}";
            }
        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            StatsWindow statsWindow = new StatsWindow();
            statsWindow.ShowDialog();
            
            // רענון הרצף למקרה שהשתנה
            if (Session.CurrentUser is User u)
            {
                txtStreak.Text = $"🔥 רצף: {u.Streak}";
            }
        }
    }
}
