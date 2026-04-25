using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using ViewModel;

namespace API_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        [HttpPut]
        [ActionName("ExamsUpdater")]
        public int UpdateExams([FromBody] Exams exams)
        {
            ExamsDB db = new ExamsDB();
            db.Update(exams);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPut]
        [ActionName("ExamsRemaindersUpdater")]
        public int UpdateExamsReminders([FromBody] ExamsRemainders examsRemainders)
        {
            ExamsRemaindersDB db = new ExamsRemaindersDB();
            db.Update(examsRemainders);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPut]
        [ActionName("ManagerUpdater")]
        public int UpdateManager([FromBody] Manager manager)
        {
            ManagerDB db = new ManagerDB();
            db.Update(manager);
            int x = db.SaveChanges();
            return x;
        }
        [HttpPut]
        [ActionName("PeopleUpdater")]
        public int UpdatePeople([FromBody] People people)
        {
            PeopleDB db = new PeopleDB();
            db.Update(people);
            int x = db.SaveChanges();
            return x;
        }
                    
        [HttpPut]
        [ActionName("")]
        public int UpdateSchedules([FromBody] Schedules schedules)
        {
            SchedulesDB db = new SchedulesDB();
            db.Update(schedules);
            int x = db.SaveChanges();
            return x;
        }

        [HttpPut]
        [ActionName("SchduleReminderUpdater")]
        public int UpdateSchduleReminder([FromBody] SchedulesRemainders schedulesRemainders)
        {
            ScheduleReminderDB db = new ScheduleReminderDB();
            db.Update(schedulesRemainders);
            int x = db.SaveChanges();
            return x;
        }

        [HttpPut]
        [ActionName("SchedulesUpdater")]
        public int UpdateSchedule([FromBody] Schedules Schedules)
        {
            SchedulesDB db = new SchedulesDB();
            db.Update(Schedules);
            int x = db.SaveChanges();
            return x;
        }


        [HttpPut]
        [ActionName("SubjectUpdater")]
        public int UpdateSubject([FromBody] Subject Subject)
        {
            SubjectDB db = new SubjectDB();
            db.Update(Subject);
            int x = db.SaveChanges();
            return x;
        }


        [HttpPut]
        [ActionName("TasksUpdater")]
        public int UpdateTasks([FromBody] Tasks Tasks)
        {
            TasksDB db = new TasksDB();
            db.Update(Tasks);
            int x = db.SaveChanges();
            return x;
        }


        [HttpPut]
        [ActionName("TasksRemaindersUpdater")]
        public int UpdateTasksRemainders([FromBody] TasksRemainders TasksRemainders)
        {
            TasksRemindersDB db = new TasksRemindersDB();
            db.Update(TasksRemainders);
            int x = db.SaveChanges();
            return x;
        }


        [HttpPut]
        [ActionName("UserUpdater")]
        public int UpdateUser([FromBody] User User)
        {
            UserDB db = new UserDB();
            db.Update(User);
            int x = db.SaveChanges();
            return x;
        }

        [HttpPut]
        [ActionName("User_SubjectUpdater")]
        public int UpdateUser_Subject([FromBody] UserSubject User_Subject)
        {
            UserSubjectDB db = new UserSubjectDB();
            db.Update(User_Subject);
            int x = db.SaveChanges();
            return x;
        }

        [HttpPut]
        [ActionName("UserE")]
        public int UpdateUserE([FromBody] UserExam UserE)
        {
            UserExamDB db = new UserExamDB();
            db.Update(UserE);
            int x = db.SaveChanges();
            return x;
        }

    }
}
    
