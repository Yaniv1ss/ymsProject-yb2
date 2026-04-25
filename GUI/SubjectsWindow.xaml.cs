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
    public partial class SubjectsWindow : Window
    {
        private SubjectDB subjectDb;

        public SubjectsWindow()
        {
            InitializeComponent();
            subjectDb = new SubjectDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgSubjects.ItemsSource = subjectDb.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת מקצועות: " + ex.Message);
            }
        }

        private void dgSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = (dgSubjects.SelectedItem != null);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = txtSubjectName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("נא להזין שם מקצוע.");
                return;
            }

            try
            {
                Subject s = new Subject { Subject_name = name };
                subjectDb.Insert(s);
                int res = subjectDb.SaveChanges();
                if (res > 0)
                {
                    txtSubjectName.Clear();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בהוספת מקצוע: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubjects.SelectedItem is Subject s)
            {
                try
                {
                    subjectDb.Delete(s);
                    subjectDb.SaveChanges();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה במחיקת מקצוע: " + ex.Message);
                }
            }
        }
    }
}
