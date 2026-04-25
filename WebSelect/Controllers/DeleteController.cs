using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using ViewModel;

namespace API_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        [HttpDelete("id")]
        public int DeleteExam(int id)
        {
            ExamsDB db = new ExamsDB();
            db.Delete(new Exams() { Id = id });
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteExamsRemainders(int id)
        {
            ExamsRemainders ExamsRemainders = ExamsRemaindersDB.SelectById(id);
            ExamsRemaindersDB db = new ExamsRemaindersDB();
            db.Delete(ExamsRemainders);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteManager(int id)
        {
            Manager Manager = ManagerDB.SelectById(id);
            ManagerDB db = new ManagerDB();
            db.Delete(Manager);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeletePeople(int id)
        {
            People People = PeopleDB.SelectById(id);
            PeopleDB db = new PeopleDB();
            db.Delete(People);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteSchedulesRemainders(int id)
        {
            SchedulesRemainders SchedulesRemainders = ScheduleReminderDB.SelectById(id);
            ScheduleReminderDB db = new ScheduleReminderDB();
            db.Delete(SchedulesRemainders);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteSchedules(int id)
        {
            Schedules Schedules = SchedulesDB.SelectById(id);
            SchedulesDB db = new SchedulesDB();
            db.Delete(Schedules);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteSubject(int id)
        {
            Subject Subject = SubjectDB.SelectById(id);
            SubjectDB db = new SubjectDB();
            db.Delete(Subject);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteTasks(int id)
        {
            Tasks Tasks = TasksDB.SelectById(id); ////SKJEDFHNSLKJN
            TasksDB db = new TasksDB();
            db.Delete(Tasks);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteTasksRemainders(int id)
        {
            TasksRemainders TasksRemainders = TasksRemindersDB.SelectById(id);
            TasksRemindersDB db = new TasksRemindersDB();
            db.Delete(TasksRemainders);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteUser(int id)
        {
            User User = UserDB.SelectById(id);
            UserDB db = new UserDB();
            db.Delete(User);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteUserSubject(int id)
        {
            UserSubject UserSubject = UserSubjectDB.SelectById(id);
            UserSubjectDB db = new UserSubjectDB();
            db.Delete(UserSubject);
            int x = db.SaveChanges();
            return x;
        }
        [HttpDelete("id")]
        public int DeleteUserE(int id)
        {
            UserExam UserExam = UserExamDB.SelectById(id);
            UserExamDB db = new UserExamDB();
            db.Delete(UserExam);
            int x = db.SaveChanges();
            return x;
        }


    }
}
