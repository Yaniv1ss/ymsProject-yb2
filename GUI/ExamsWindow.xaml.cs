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
    public partial class ExamsWindow : Window
    {
        private ExamsDB examsDb;
        private UserExamDB userExamDb;
        private SubjectDB subjectDb;
        private UserDB userDb;
        private List<Exams> myExamsList;

        public ExamsWindow()
        {
            InitializeComponent();
            examsDb = new ExamsDB();
            userExamDb = new UserExamDB();
            subjectDb = new SubjectDB();
            userDb = new UserDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbSubject.ItemsSource = subjectDb.SelectAll();
                dpExamDate.SelectedDate = DateTime.Now;
            }
            catch { }

            LoadExams();
        }

        private void LoadExams()
        {
            if (Session.CurrentUser is User currentUser)
            {
                // הגדרת היעד בתצוגה
                txtGradeGoal.Text = currentUser.GradeGoal.ToString();

                try
                {
                    var allExams = examsDb.SelectAll().Cast<Exams>().ToList();
                    var allUserExams = userExamDb.SelectAll().Cast<UserExam>().ToList();
                    
                    var myExamIds = allUserExams.Where(ue => ue.User_id != null && ue.User_id.Id == currentUser.Id).Select(ue => ue.Exam_id.Id).ToList();
                    myExamsList = allExams.Where(e => myExamIds.Contains(e.Id)).OrderByDescending(e => e.Id).ToList();
                    
                    dgExams.ItemsSource = myExamsList;

                    var gradedExams = myExamsList.Where(e => e.Grade > 0).ToList();
                    if (gradedExams.Count > 0)
                    {
                        double avg = gradedExams.Average(e => e.Grade);
                        txtAverage.Text = Math.Round(avg, 2).ToString();
                    }
                    else
                    {
                        txtAverage.Text = "אין נתונים";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בטעינת נתונים: " + ex.Message);
                }
            }
        }

        private void btnAddExam_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || cmbSubject.SelectedItem == null || dpExamDate.SelectedDate == null)
            {
                MessageBox.Show("נא למלא כותרת, מקצוע ותאריך.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Session.CurrentUser is User currentUser)
            {
                Exams newExam = new Exams()
                {
                    Title = txtTitle.Text,
                    Subject_id = (Subject)cmbSubject.SelectedItem,
                    Exam_date = dpExamDate.SelectedDate.Value.ToShortDateString(),
                    Grade = 0
                };

                try
                {
                    // הוספת המבחן לטבלת Exams
                    examsDb.Insert(newExam);
                    int res = examsDb.SaveChanges();

                    if (res > 0)
                    {
                        // חיבור המבחן למשתמש בטבלת UserE
                        UserExam ue = new UserExam()
                        {
                            User_id = currentUser,
                            Exam_id = newExam
                        };
                        userExamDb.Insert(ue);
                        userExamDb.SaveChanges();

                        txtTitle.Clear();
                        LoadExams();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בהוספת המבחן: " + ex.Message);
                }
            }
        }

        private void btnUpdateGrade_Click(object sender, RoutedEventArgs e)
        {
            if (dgExams.SelectedItem is Exams selectedExam)
            {
                if (int.TryParse(txtGrade.Text, out int newGrade) && newGrade >= 0 && newGrade <= 100)
                {
                    try
                    {
                        selectedExam.Grade = newGrade;
                        examsDb.Update(selectedExam);
                        int res = examsDb.SaveChanges();
                        
                        if (res > 0)
                        {
                            txtGrade.Clear();
                            btnUpdateGrade.IsEnabled = false;
                            LoadExams(); 
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("שגיאה בעדכון הציון: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("נא להזין ציון תקין בין 0 ל-100.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnUpdateGoal_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CurrentUser is User currentUser)
            {
                if (int.TryParse(txtGradeGoal.Text, out int newGoal) && newGoal >= 0 && newGoal <= 100)
                {
                    try
                    {
                        currentUser.GradeGoal = newGoal;
                        userDb.Update(currentUser);
                        userDb.SaveChanges();
                        MessageBox.Show("היעד נשמר בהצלחה!", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadExams();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("שגיאה בשמירת היעד: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("נא להזין יעד תקין בין 0 ל-100.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void dgExams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgExams.SelectedItem is Exams selected)
            {
                txtGrade.Text = selected.Grade.ToString();
                btnUpdateGrade.IsEnabled = true;
                btnDeleteExam.IsEnabled = true;
                
                // Populate edit fields
                txtTitle.Text = selected.Title;
                if (DateTime.TryParse(selected.Exam_date, out DateTime dt))
                    dpExamDate.SelectedDate = dt;
                
                // Select matching subject
                foreach (var item in cmbSubject.Items)
                {
                    if (item is Subject s && s.Id == selected.Subject_id.Id)
                    {
                        cmbSubject.SelectedItem = item;
                        break;
                    }
                }
                
                btnEditExamDetails.Visibility = Visibility.Visible;
                btnAddExam.Content = "הוסף מבחן חדש";
            }
            else
            {
                txtGrade.Text = "";
                btnUpdateGrade.IsEnabled = false;
                btnDeleteExam.IsEnabled = false;
                btnEditExamDetails.Visibility = Visibility.Collapsed;
                btnAddExam.Content = "הוסף מבחן";
            }
        }

        private void btnDeleteExam_Click(object sender, RoutedEventArgs e)
        {
            if (dgExams.SelectedItem is Exams selectedExam && Session.CurrentUser is User currentUser)
            {
                MessageBoxResult result = MessageBox.Show($"האם אתה בטוח שברצונך למחוק את המבחן '{selectedExam.Title}'?", "אישור מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // 1. First remove the UserExam link to avoid foreign key conflicts
                    var userExams = userExamDb.SelectAll().Cast<UserExam>().ToList();
                    var link = userExams.FirstOrDefault(ue => ue.Exam_id.Id == selectedExam.Id && ue.User_id.Id == currentUser.Id);
                    if (link != null)
                    {
                        userExamDb.Delete(link);
                        userExamDb.SaveChanges();
                    }

                    // 2. Delete the actual exam
                    examsDb.Delete(selectedExam);
                    examsDb.SaveChanges();

                    MessageBox.Show("המבחן נמחק בהצלחה.", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadExams();
                }
            }
        }

        private void btnEditExamDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgExams.SelectedItem is Exams selectedExam && Session.CurrentUser is User currentUser)
            {
                if (cmbSubject.SelectedItem == null || string.IsNullOrWhiteSpace(txtTitle.Text) || dpExamDate.SelectedDate == null)
                {
                    MessageBox.Show("נא למלא את כל הפרטים (מקצוע, כותרת, ותאריך).", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Subject selectedSub = cmbSubject.SelectedItem as Subject;

                selectedExam.Subject_id = selectedSub;
                selectedExam.Title = txtTitle.Text;
                selectedExam.Exam_date = dpExamDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                
                examsDb.Update(selectedExam);
                int res = examsDb.SaveChanges();

                if (res > 0)
                {
                    MessageBox.Show("פרטי המבחן עודכנו בהצלחה!", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Reset fields
                    txtTitle.Text = "";
                    cmbSubject.SelectedItem = null;
                    dpExamDate.SelectedDate = null;
                    dgExams.SelectedItem = null;
                    
                    LoadExams();
                }
                else
                {
                    MessageBox.Show("אירעה שגיאה. הנתונים לא עודכנו.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
