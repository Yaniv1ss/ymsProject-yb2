using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Threading.Tasks;
using ViewModel;

namespace API_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InsertController : ControllerBase
    {
        [HttpPost]
        public int InsertExams([FromBody] Exams Exams)
        {
            ExamsDB db = new ExamsDB();
            db.Insert(Exams);
            int x = db.SaveChanges();
            return x;
        }

        [HttpPost]
        public int InsertExamsRemainders([FromBody] ExamsRemainders ExamsRemainders)
        {
            ExamsRemaindersDB db = new ExamsRemaindersDB();
            db.Insert(ExamsRemainders);
            int x = db.SaveChanges();
            return x;
        }

        [HttpPost]
        public int InsertManager([FromBody] Manager Manager)
        {
            ManagerDB db = new ManagerDB();
            db.Insert(Manager);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertPeople([FromBody] People People)
        {
            PeopleDB db = new PeopleDB();
            db.Insert(People);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertSchduleReminder([FromBody] SchedulesRemainders SchedulesRemainders)
        {
            ScheduleReminderDB db = new ScheduleReminderDB();
            db.Insert(SchedulesRemainders);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertSchdule([FromBody] Schedules Schedules)
        {
            SchedulesDB db = new SchedulesDB();
            db.Insert(Schedules);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertSubject([FromBody] Subject Subject)
        {
            SubjectDB db = new SubjectDB();
            db.Insert(Subject);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertTasks([FromBody] Tasks Tasks)
        {
            TasksDB db = new TasksDB();
            db.Insert(Tasks);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertTasksRemainders([FromBody] TasksRemainders TasksRemainders)
        {
            TasksRemindersDB db = new TasksRemindersDB();
            db.Insert(TasksRemainders);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertUser([FromBody] User User)
        {
            UserDB db = new UserDB();
            db.Insert(User);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertUser_Subject([FromBody] UserSubject UserSubject)
        {
            UserSubjectDB db = new UserSubjectDB();
            db.Insert(UserSubject);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPost]
        public int InsertUserE([FromBody] UserExam UserExam)
        {
            UserExamDB db = new UserExamDB();
            db.Insert(UserExam);
            int x = db.SaveChanges();
            return x;
        }


    }
}

