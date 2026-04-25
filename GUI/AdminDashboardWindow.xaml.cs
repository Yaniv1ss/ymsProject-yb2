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
    public partial class AdminDashboardWindow : Window
    {
        private UserDB userDb;
        private ManagerDB managerDb;
        private UserList? allUsers;
        private System.Windows.Threading.DispatcherTimer? timer;

        public AdminDashboardWindow()
        {
            InitializeComponent();
            userDb = new UserDB();
            managerDb = new ManagerDB();
            StartClock();
        }

        private void StartClock()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => { if (txtTime != null) txtTime.Text = DateTime.Now.ToString("HH:mm:ss"); };
            timer.Start();
            txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Window_Loaded(object? sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                allUsers = userDb.SelectAll();
                // We show all users, but we could highlight if they are already an admin
                if (dgUsers != null) dgUsers.ItemsSource = allUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת נתונים: " + ex.Message);
            }
        }

        private void dgUsers_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (dgUsers != null && dgUsers.SelectedItem is User selectedUser)
            {
                if (txtSelectedUser != null) txtSelectedUser.Text = $"משתמש נבחר: {selectedUser.Username}";
                LoadUserDetails(selectedUser);
            }
        }

        private void LoadUserDetails(User user)
        {
            try
            {
                // Load Tasks
                TasksDB tasksDb = new TasksDB();
                var allTasks = tasksDb.SelectAll();
                if (dgTasks != null) dgTasks.ItemsSource = allTasks.Cast<Tasks>().Where(t => t.User_id != null && t.User_id.Id == user.Id).ToList();

                // Load Schedules
                SchedulesDB scheduleDb = new SchedulesDB();
                var allSchedules = scheduleDb.SelectAll();
                if (dgSchedules != null) dgSchedules.ItemsSource = allSchedules.Cast<Schedules>().Where(s => s.User_id != null && s.User_id.Id == user.Id).OrderBy(s => s.Hour).ToList();

                // Load Exams
                ExamsDB examsDb = new ExamsDB();
                var allExams = examsDb.SelectAll();
                // Filter exams that belong to this user via UserExam table
                UserExamDB userExamDb = new UserExamDB();
                var userExamIds = userExamDb.SelectAll().Cast<UserExam>().Where(ue => ue.User_id != null && ue.User_id.Id == user.Id).Select(ue => ue.Exam_id.Id).ToList();
                if (dgExams != null) dgExams.ItemsSource = allExams.Cast<Exams>().Where(ex => userExamIds.Contains(ex.Id)).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading details: " + ex.Message);
            }
        }

        private void btnLogout_Click(object? sender, RoutedEventArgs e)
        {
            Session.Logout();
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void btnMakeAdmin_Click(object? sender, RoutedEventArgs e)
        {
            if (dgUsers != null && dgUsers.SelectedItem is User selectedUser)
            {
                try
                {
                    // Check if already an admin
                    ManagerList currentManagers = managerDb.SelectAll();
                    if (currentManagers.Any(m => m.Id == selectedUser.Id))
                    {
                        MessageBox.Show("המשתמש כבר מוגדר כמנהל מערכת.", "מידע", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    // Convert to Manager
                    Manager newAdmin = new Manager { Id = selectedUser.Id };
                    managerDb.InsertExistingIntoManagerOnly(newAdmin);
                    int result = managerDb.SaveChanges();

                    if (result > 0)
                    {
                        MessageBox.Show($"המשתמש {selectedUser.Username} קודם בהצלחה להיות מנהל!", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("אירעה שגיאה. הקדום לא בוצע.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאת מערכת: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("אנא בחר משתמש מהרשימה תחילה.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnRemoveAdmin_Click(object? sender, RoutedEventArgs e)
        {
            if (dgUsers != null && dgUsers.SelectedItem is User selectedUser)
            {
                try
                {
                    ManagerList currentManagers = managerDb.SelectAll();
                    if (!currentManagers.Any(m => m.Id == selectedUser.Id))
                    {
                        MessageBox.Show("המשתמש אינו מוגדר כמנהל מערכת.", "מידע", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    MessageBoxResult answer = MessageBox.Show($"האם אתה בטוח שברצונך להסיר את הרשאות הניהול מהמשתמש {selectedUser.Username}?", "אישור הסרה", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    
                    if (answer == MessageBoxResult.Yes)
                    {
                        Manager adminToRemove = currentManagers.First(m => m.Id == selectedUser.Id);
                        managerDb.DeleteFromManagerOnly(adminToRemove);
                        int result = managerDb.SaveChanges();

                        if (result > 0)
                        {
                            MessageBox.Show($"הוסרו הרשאות הניהול מהמשתמש {selectedUser.Username}.", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            // אם מנהל הסיר את עצמו
                            if (Session.CurrentUser != null && Session.CurrentUser.Id == selectedUser.Id)
                            {
                                MessageBox.Show("הסרת את עצמך מניהול. המערכת תתנתק כעת.", "התנתקות", MessageBoxButton.OK, MessageBoxImage.Information);
                                btnLogout_Click(null, null);
                            }
                        }
                        else
                        {
                            MessageBox.Show("אירעה שגיאה. ההסרה לא בוצעה.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאת מערכת: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("אנא בחר משתמש מהרשימה תחילה.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDeleteUser_Click(object? sender, RoutedEventArgs e)
        {
            if (dgUsers != null && dgUsers.SelectedItem is User selectedUser)
            {
                // מניעת מחיקת המנהל הנוכחי את עצמו
                if (Session.CurrentUser != null && Session.CurrentUser.Id == selectedUser.Id)
                {
                    MessageBox.Show("אינך יכול למחוק את עצמך!", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult answer = MessageBox.Show($"האם אתה בטוח שברצונך למחוק לחלוטין את המשתמש {selectedUser.Username} ואת כל הנתונים שלו (משימות, מערכת)?", "אישור מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                
                if (answer == MessageBoxResult.Yes)
                {
                    try
                    {
                        // 1. הסרה מטבלת מנהלים אם קיים
                        ManagerList currentManagers = managerDb.SelectAll();
                        var adminEntry = currentManagers.FirstOrDefault(m => m.Id == selectedUser.Id);
                        if (adminEntry != null)
                        {
                            managerDb.DeleteFromManagerOnly(adminEntry);
                        }

                        // 2. הסרת משימות
                        TasksDB tasksDb = new TasksDB();
                        var userTasks = tasksDb.SelectAll().Cast<Tasks>().Where(t => t.User_id != null && t.User_id.Id == selectedUser.Id).ToList();
                        foreach (var task in userTasks) tasksDb.Delete(task);
                        tasksDb.SaveChanges();

                        // 3. הסרת מערכת שעות
                        SchedulesDB schedulesDb = new SchedulesDB();
                        var userSchedules = schedulesDb.SelectAll().Cast<Schedules>().Where(s => s.User_id != null && s.User_id.Id == selectedUser.Id).ToList();
                        foreach (var schedule in userSchedules) schedulesDb.Delete(schedule);
                        schedulesDb.SaveChanges();

                        // 4. הסרת קישורי נושאים ומבחנים (אם קיים ב-ViewModel)
                        UserSubjectDB userSubDb = new UserSubjectDB();
                        var userSubs = userSubDb.SelectAll().Cast<UserSubject>().Where(us => us.User_id != null && us.User_id.Id == selectedUser.Id).ToList();
                        foreach (var us in userSubs) userSubDb.Delete(us);
                        userSubDb.SaveChanges();

                        UserExamDB userExamDb = new UserExamDB();
                        var userExams = userExamDb.SelectAll().Cast<UserExam>().Where(ue => ue.User_id != null && ue.User_id.Id == selectedUser.Id).ToList();
                        foreach (var ue in userExams) userExamDb.Delete(ue);
                        userExamDb.SaveChanges();

                        // 5. מחיקת המשתמש והאדם
                        userDb.Delete(selectedUser);
                        int result = userDb.SaveChanges();

                        if (result > 0)
                        {
                            MessageBox.Show($"המשתמש {selectedUser.Username} נמחק בהצלחה.", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                            if (dgTasks != null) dgTasks.ItemsSource = null;
                            if (dgSchedules != null) dgSchedules.ItemsSource = null;
                            if (dgExams != null) dgExams.ItemsSource = null;
                            if (txtSelectedUser != null) txtSelectedUser.Text = "בחר משתמש...";
                        }
                        else
                        {
                            MessageBox.Show("אירעה שגיאה במחיקה.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("שגיאה במחיקת משתמש: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("אנא בחר משתמש למחיקה.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnSaveUserChanges_Click(object? sender, RoutedEventArgs e)
        {
            if (dgUsers != null && dgUsers.SelectedItem is User selectedUser)
            {
                try
                {
                    int totalResults = 0;

                    // 1. Update User Details
                    userDb.Update(selectedUser);
                    totalResults += userDb.SaveChanges();

                    // 2. Update Tasks
                    if (dgTasks != null && dgTasks.ItemsSource is List<Tasks> tasksList)
                    {
                        TasksDB tDb = new TasksDB();
                        foreach (var task in tasksList) tDb.Update(task);
                        totalResults += tDb.SaveChanges();
                    }

                    // 3. Update Schedules
                    if (dgSchedules != null && dgSchedules.ItemsSource is List<Schedules> schedulesList)
                    {
                        SchedulesDB sDb = new SchedulesDB();
                        foreach (var schedule in schedulesList) sDb.Update(schedule);
                        totalResults += sDb.SaveChanges();
                    }

                    // 4. Update Exams
                    if (dgExams != null && dgExams.ItemsSource is List<Exams> examsList)
                    {
                        ExamsDB eDb = new ExamsDB();
                        foreach (var exam in examsList) eDb.Update(exam);
                        totalResults += eDb.SaveChanges();
                    }

                    if (totalResults > 0)
                    {
                        MessageBox.Show("כל השינויים נשמרו בהצלחה!", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                        LoadUserDetails(selectedUser);
                    }
                    else
                    {
                        MessageBox.Show("לא זוהו שינויים לשמירה.", "מידע", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בשמירת נתונים: " + ex.Message, "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("אנא בחר משתמש מהרשימה.", "אזהרה", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks != null && dgTasks.SelectedItem is Tasks selectedTask)
            {
                if (MessageBox.Show($"למחוק את המשימה '{selectedTask.Title}'?", "אישור", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    TasksDB tDb = new TasksDB();
                    tDb.Delete(selectedTask);
                    tDb.SaveChanges();
                    if (dgUsers.SelectedItem is User u) LoadUserDetails(u);
                }
            }
        }

        private void btnDeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedules != null && dgSchedules.SelectedItem is Schedules selectedSchedule)
            {
                if (MessageBox.Show("למחוק שעה זו מהמערכת?", "אישור", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SchedulesDB sDb = new SchedulesDB();
                    sDb.Delete(selectedSchedule);
                    sDb.SaveChanges();
                    if (dgUsers.SelectedItem is User u) LoadUserDetails(u);
                }
            }
        }

        private void btnDeleteExam_Click(object sender, RoutedEventArgs e)
        {
            if (dgExams != null && dgExams.SelectedItem is Exams selectedExam)
            {
                if (MessageBox.Show($"למחוק את המבחן '{selectedExam.Title}'?", "אישור", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // מחיקת הקשר בין המשתמש למבחן
                    UserExamDB ueDb = new UserExamDB();
                    var userExams = ueDb.SelectAll().Cast<UserExam>().Where(ue => ue.Exam_id.Id == selectedExam.Id && ue.User_id.Id == ((User)dgUsers.SelectedItem).Id).ToList();
                    foreach (var ue in userExams) ueDb.Delete(ue);
                    ueDb.SaveChanges();

                    // מחיקת המבחן עצמו
                    ExamsDB eDb = new ExamsDB();
                    eDb.Delete(selectedExam);
                    eDb.SaveChanges();
                    
                    if (dgUsers.SelectedItem is User u) LoadUserDetails(u);
                }
            }
        }
    }
}
