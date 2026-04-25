using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TasksRemindersDB : BaseDB
    {
        public TasksRemindersList SelectAll()
        {
            command.CommandText = $"SELECT * FROM Exams";
            TasksRemindersList pList = new TasksRemindersList(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            TasksRemainders p = entity as TasksRemainders;
            p.Subject_id = SubjectDB.SelectById((int)reader["subject_id"]);
            base.CreateModel(entity);
            return p;
        }
        public override BaseEntity NewEntity()
        {
            return new TasksRemainders();
        }
        static private TasksRemindersList list = new TasksRemindersList();
        public static TasksRemainders SelectById(int id)
        {
            TasksRemindersDB db = new TasksRemindersDB();
            list = db.SelectAll();

            TasksRemainders g = list.Find(item => item.Id == id);
            return g;
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Exams c = entity as Exams;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM Exams where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Exams c = entity as Exams;
            if (c != null)
            {
                string sqlStr = $"Insert INTO  Exams (Subject_id) VALUES (@cSubject_id)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSubject_id", c.Subject_id));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
           TasksRemainders c = entity as TasksRemainders;
            if (c != null)
            {
                string sqlStr = $"UPDATE TasksReminders  SET Subject_id=@cSubject_id WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSubject_id", c.Subject_id.Id));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }

        
    
  

    }
}
