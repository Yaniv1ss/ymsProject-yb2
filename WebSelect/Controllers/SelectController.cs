using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using ViewModel;

namespace API_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SelectController : ControllerBase
    {
        [HttpGet]
        [ActionName("ExamSelector")]
        public ExamsList SelectAllExams()
        {
            ExamsDB db = new ExamsDB();
            ExamsList exams = db.SelectAll();
            return exams;
        }

        [HttpGet]
        [ActionName("ExamRemindersSelector")]
        public ExamsRemaindersList SelectAllExamsReminders()
        {
            ExamsRemaindersDB db = new ExamsRemaindersDB();
            ExamsRemaindersList ExamsRemainders = db.SelectAll();
            return ExamsRemainders;
        }

        [HttpGet]
        [ActionName("ManagerSelector")]
        public ManagerList SelectAllManagers()
        {
            ManagerDB db = new ManagerDB();
            ManagerList manager = db.SelectAll();
            return manager;
        }

        [HttpGet]
        [ActionName("PeopleSelector")]
        public PeopleList SelectAllPeople()
        {
            PeopleDB db = new PeopleDB();
            PeopleList people = db.SelectAll();
            return people;
        }

        [HttpGet]
        [ActionName("SchedulesSelector")]
        public SchedulesList SelectAllSchedules()
        {
            SchedulesDB db = new SchedulesDB();
            SchedulesList schedules = db.SelectAll();
            return schedules;
        }

        [HttpGet]
        [ActionName("SchedulesRemindersSelector")]
        public SchedulesRemindersList SelectAllSchedulesReminders()
        {
            ScheduleReminderDB db = new ScheduleReminderDB();
            SchedulesRemindersList schedulesRE = db.SelectAll();
            return schedulesRE;
        }


        [HttpGet]
        [ActionName("SubjectSelector")]
        public SubjectList SelectAllSubject()
        {
            SubjectDB db = new SubjectDB();
            SubjectList subjects = db.SelectAll();
            return subjects;
        }

        [HttpGet]
        [ActionName("TasksSelector")]
        public TasksList SelectAllTasks()
        {
            TasksDB db = new TasksDB();
            TasksList tasks = db.SelectAll();
            return tasks;
        }

        [HttpGet]
        [ActionName("TaskReminderssSelector")]
        public TasksRemindersList SelectAllTaskReminderss()
        {
            TasksRemindersDB db = new TasksRemindersDB();
            TasksRemindersList taskreminders = db.SelectAll();
            return taskreminders;
        }

        [HttpGet]//
        public TasksList SelectAllTask()
        {
            TasksDB db = new TasksDB();
            TasksList tasks = db.SelectAll();
            return tasks;
        }

        [HttpGet]
        [ActionName("UsersSelector")]
        public UserList SelectAllUsers()
        {
            UserDB db = new UserDB();
            UserList users = db.SelectAll();
            return users;
        }


        [HttpGet]
        [ActionName("UsersExamSelector")]
        public UserExamList SelectAllUsersExam()
        {
            UserExamDB db = new UserExamDB();
            UserExamList usersExam = db.SelectAll();
            return usersExam;
        }

        [HttpGet]
        [ActionName("UsersSubjectSelector")]
        public UserSubjectList SelectAllUsersSubject()
        {
            UserSubjectDB db = new UserSubjectDB();
            UserSubjectList usersExam = db.SelectAll();
            return usersExam;
        }
    }
}
