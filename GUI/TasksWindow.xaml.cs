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
    public partial class TasksWindow : Window
    {
        private TasksDB tasksDb;
        private SubjectDB subjectDb;
        private List<Tasks> myTasksList;

        public TasksWindow()
        {
            InitializeComponent();
            tasksDb = new TasksDB();
            subjectDb = new SubjectDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            try
            {
                cmbSubject.ItemsSource = subjectDb.SelectAll();
                dpDueDate.SelectedDate = DateTime.Now.AddDays(7);
            }
            catch { }
        }

        private void LoadData()
        {
            if (Session.CurrentUser is User currentUser)
            {
                try
                {
                    var all = tasksDb.SelectAll();
                    myTasksList = all.Cast<Tasks>().Where(t => t.User_id != null && t.User_id.Id == currentUser.Id).ToList();
                    dgTasks.ItemsSource = myTasksList.OrderBy(t => t.Is_done).ThenBy(t => t.DueDate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בטעינת משימות: " + ex.Message);
                }
            }
        }

        private void UpdateStreak()
        {
            if (Session.CurrentUser is User user)
            {
                DateTime today = DateTime.Today;
                DateTime? lastDate = user.LastTaskDate?.Date;

                if (lastDate == null || lastDate < today)
                {
                    if (lastDate == today.AddDays(-1))
                    {
                        user.Streak++;
                    }
                    else
                    {
                        user.Streak = 1;
                    }
                    user.LastTaskDate = today;

                    try
                    {
                        UserDB userDb = new UserDB();
                        userDb.Update(user);
                        userDb.SaveChanges();
                    }
                    catch { }
                }
            }
        }

        private void dgTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelection = dgTasks.SelectedItem != null;
            btnDone.IsEnabled = hasSelection;
            btnDelete.IsEnabled = hasSelection;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || cmbSubject.SelectedItem == null || dpDueDate.SelectedDate == null)
            {
                MessageBox.Show("נא למלא כותרת, מקצוע ותאריך יעד.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Session.CurrentUser is User currentUser)
            {
                Tasks newTask = new Tasks()
                {
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    Subject_id = (Subject)cmbSubject.SelectedItem,
                    DueDate = dpDueDate.SelectedDate.Value,
                    Is_done = false,
                    User_id = currentUser
                };

                try
                {
                    tasksDb.Insert(newTask);
                    int res = tasksDb.SaveChanges();
                    if (res > 0)
                    {
                        UpdateStreak(); // עדכון רצף
                        txtTitle.Clear();
                        txtDescription.Clear();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בהוספת המשימה: " + ex.Message);
                }
            }
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is Tasks selectedTask)
            {
                try
                {
                    selectedTask.Is_done = !selectedTask.Is_done; // Toggle status
                    tasksDb.Update(selectedTask);
                    tasksDb.SaveChanges();
                    if (selectedTask.Is_done)
                    {
                        UpdateStreak(); // עדכון רצף כשמשימה הושלמה
                    }
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בעדכון המשימה: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is Tasks selectedTask)
            {
                try
                {
                    tasksDb.Delete(selectedTask);
                    tasksDb.SaveChanges();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה במחיקת המשימה: " + ex.Message);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
