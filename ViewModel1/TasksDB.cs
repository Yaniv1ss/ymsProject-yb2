using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1416
namespace ViewModel
{
    public class TasksDB : BaseDB
    {
        public TasksList SelectAll()
        {
            command.CommandText = $"SELECT * FROM Tasks";
            TasksList pList = new TasksList(base.Select());
            return pList;
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Tasks p = (entity as Tasks)!;
            p.Subject_id = SubjectDB.SelectById((int)reader!["subject_id"])!;
            p.Title = reader!["Title"].ToString();
            p.Description = reader!["Description"].ToString();
            p.DueDate = (DateTime)reader!["Due_Date"];
            p.Is_done = (bool)reader!["Is_done"];
            p.User_id = UserDB.SelectById((int)reader!["User_id"])!;
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new Tasks();
        }

        static private TasksList list = new TasksList();
        public static Tasks SelectById(int id)
        {
            TasksDB db = new TasksDB();
            list = db.SelectAll();

            Tasks g = list.Find(item => item.Id == id)!;
            return g;
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Tasks c = entity as Tasks;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM Tasks where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Tasks c = (entity as Tasks)!;
            if (c != null)
            {
                string sqlStr = $"Insert INTO Tasks (Subject_id, [Title], [Description], [Due_Date], [Is_done], [User_id]) VALUES (@cSubject_id, @cTitle, @cDescription, @cDueDate, @cIs_done, @cUser_id)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSubject_id", (c.Subject_id != null) ? c.Subject_id.Id : 0));
                command.Parameters.Add(new OleDbParameter("@cTitle", c.Title ?? ""));
                command.Parameters.Add(new OleDbParameter("@cDescription", c.Description ?? ""));
                command.Parameters.Add(new OleDbParameter("@cDueDate", c.DueDate.ToShortDateString()));
                command.Parameters.Add(new OleDbParameter("@cIs_done", c.Is_done));
                command.Parameters.Add(new OleDbParameter("@cUser_id", (c.User_id != null) ? c.User_id.Id : 0));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Tasks c = (entity as Tasks)!;
            if (c != null)
            {
                string sqlStr = $"UPDATE Tasks SET Subject_id=@cSubject_id, [Title]=@cTitle, [Description]=@cDescription, [Due_Date]=@cDueDate, [Is_done]=@cIs_done, [User_id]=@cUser_id WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSubject_id", (c.Subject_id != null) ? c.Subject_id.Id : 0));
                command.Parameters.Add(new OleDbParameter("@cTitle", c.Title ?? ""));
                command.Parameters.Add(new OleDbParameter("@cDescription", c.Description ?? ""));
                command.Parameters.Add(new OleDbParameter("@cDueDate", c.DueDate.ToShortDateString()));
                command.Parameters.Add(new OleDbParameter("@cIs_done", c.Is_done));
                command.Parameters.Add(new OleDbParameter("@cUser_id", (c.User_id != null) ? c.User_id.Id : 0));
                command.Parameters.Add(new OleDbParameter("@Id", c.Id));
            }
        }
    }

    }



