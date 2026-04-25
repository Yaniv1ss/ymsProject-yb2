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
    public partial class ScheduleWindow : Window
    {
        private SchedulesDB scheduleDb;
        private SubjectDB subjectDb;
        private SchedulesList mySchedules;

        public ScheduleWindow()
        {
            InitializeComponent();
            scheduleDb = new SchedulesDB();
            subjectDb = new SubjectDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            
            try
            {
                cmbSubject.ItemsSource = subjectDb.SelectAll();
            }
            catch { }
        }

        private void LoadData()
        {
            if (Session.CurrentUser is User currentUser)
            {
                try
                {
                    var all = scheduleDb.SelectAll();
                    // Filter to only this user's schedules
                    mySchedules = new SchedulesList(all.Where(s => s.User_id.Id == currentUser.Id));
                    dgSchedule.ItemsSource = mySchedules.OrderBy(s => GetDayValue(s.Day_of_the_week)).ThenBy(s => s.Hour);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בטעינת מערכת השעות: " + ex.Message);
                }
            }
        }

        private int GetDayValue(string day)
        {
            return day switch
            {
                "ראשון" => 1,
                "שני" => 2,
                "שלישי" => 3,
                "רביעי" => 4,
                "חמישי" => 5,
                "שישי" => 6,
                _ => 7
            };
        }

        private void dgSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = dgSchedule.SelectedItem != null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDay.SelectedItem == null || cmbHour.SelectedItem == null || cmbSubject.SelectedItem == null)
            {
                MessageBox.Show("אנא מלא את כל השדות כדי להוסיף שיעור.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Session.CurrentUser is User currentUser)
            {
                ComboBoxItem selectedDay = (ComboBoxItem)cmbDay.SelectedItem;
                ComboBoxItem selectedHour = (ComboBoxItem)cmbHour.SelectedItem;
                Subject selectedSubject = (Subject)cmbSubject.SelectedItem;

                Schedules newSchedule = new Schedules()
                {
                    User_id = currentUser,
                    Day_of_the_week = selectedDay.Content.ToString(),
                    Hour = int.Parse(selectedHour.Content.ToString()),
                    Subject_id = selectedSubject
                };

                try
                {
                    scheduleDb.Insert(newSchedule);
                    int res = scheduleDb.SaveChanges();
                    if (res > 0)
                    {
                        LoadData();
                        MessageBox.Show("שיעור נוסף בהצלחה!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה הוספת שיעור: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.SelectedItem is Schedules selectedBlock)
            {
                try
                {
                    scheduleDb.Delete(selectedBlock);
                    int res = scheduleDb.SaveChanges();
                    if (res > 0)
                    {
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה במחיקת השיעור: " + ex.Message);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnManageSubjects_Click(object sender, RoutedEventArgs e)
        {
            SubjectsWindow sub = new SubjectsWindow();
            sub.ShowDialog();
            
            // Refresh subjects combo box
            try
            {
                cmbSubject.ItemsSource = subjectDb.SelectAll();
            }
            catch { }
        }
    }
}
