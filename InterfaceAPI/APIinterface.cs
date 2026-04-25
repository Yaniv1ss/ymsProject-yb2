using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI
{
     public class APIinterface
    {
        public interface IApiService
        {
            public Task<ExamsList> GetAllExams();
            public Task<int> InsertAExam(Exams exams);
            public Task<int> UpdateAExam(Exams Exams);
            public Task<int> DeleteAExam(int id);


            public Task<ExamsRemaindersList> GetAllExamsRemainders();
            public Task<int> InsertAExamsRemainders(ExamsRemainders ExamsRemainders);
            public Task<int> UpdateAExamsRemainders(ExamsRemainders ExamsRemainders);
            public Task<int> DeleteAExamsRemainders(int id);


            public Task<ManagerList> GetAllManagerList();
            public Task<int> InsertAManagerList(Manager Manager);
            public Task<int> UpdateAManagerList(Manager Manager);
            public Task<int> DeleteAManagerList( int id);


            public Task<PeopleList> GetAllPeople();
            public Task<int> InsertAPeople(People People);
            public Task<int> UpdateAPeople(People People);
            public Task<int> DeleteAPeople(int id);


            public Task<SchedulesList> GetAllSchedules();
            public Task<int> InsertASchedules(Schedules Schedules);
            public Task<int> UpdateASchedules(Schedules Schedules);
            public Task<int> DeleteASchedules(int id);


            public Task<SchedulesRemindersList> GetAllSchedulesReminders();
            public Task<int> InsertASchedulesReminders(SchedulesRemainders SchedulesRemainders);
            public Task<int> UpdateASchedulesReminders(SchedulesRemainders SchedulesRemainders);
            public Task<int> DeleteASchedulesReminders(int id);

            public Task<SubjectList> GetAllSubject();
            public Task<int> InsertASubjects(Subject Subject);
            public Task<int> UpdateASubject(Subject Subject);
            public Task<int> DeleteASubject(int id);

            public Task<TasksList> GetAllTasks();
            public Task<int> InsertATasks(Tasks Tasks);
            public Task<int> UpdateATasks(Tasks Tasks);
            public Task<int> DeleteATasks(int id);


            public Task<TasksRemindersList> GetAllTasksReminders();
            public Task<int> InsertATasksReminders(TasksRemindersList TasksRemindersList);
            public Task<int> UpdateATasksReminders(TasksRemindersList TasksRemindersList);
            public Task<int> DeleteATasksReminders(int id);


            public Task<UserList> GetAllUser();
            public Task<int> InsertAUser(User User);
            public Task<int> UpdateAUser(User User);
            public Task<int> DeleteAUser(int id);

            public Task<UserExamList> GetAllUserExam();
            public Task<int> InsertAUserExam(UserExam UserExam);
            public Task<int> UpdateAUserExam(UserExam UserExam);
            public Task<int> DeleteAUserExam(int id);

            public Task<UserSubjectList> GetAllUserSubject();
            public Task<int> InsertAUserSubject(UserSubject UserSubject);
            public Task<int> UpdateAUserSubject(UserSubject UsUserSubject);
            public Task<int> DeleteAUserSubject(int id);


        }

    }
}
