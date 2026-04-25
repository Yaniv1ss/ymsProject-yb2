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
    public class UserDB : PeopleDB
    {
        public new UserList SelectAll()
        {
            command.CommandText = $"SELECT People.*, [User].username AS Expr1, [User].email, [User].goal, [User].streak, [User].last_task_date, [User].grade_goal FROM" +
                $" (People INNER JOIN [User] ON People.Id = [User].id)";
            UserList pList = new UserList(base.Select());
            return pList;
        }


        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            User p = (entity as User)!;          
            p.Username = reader!["username"].ToString();
            p.Email = reader!["email"].ToString();
            p.Goal = int.Parse(reader!["goal"].ToString());
            
            // קריאת יעד ממוצע
            object gradeGoalVal = reader!["grade_goal"];
            p.GradeGoal = (gradeGoalVal != DBNull.Value) ? Convert.ToInt32(gradeGoalVal) : 85;

            // קריאת נתוני רצף
            object streakVal = reader!["streak"];
            p.Streak = (streakVal != DBNull.Value) ? Convert.ToInt32(streakVal) : 0;
            object dateVal = reader!["last_task_date"];
            p.LastTaskDate = (dateVal != DBNull.Value) ? (DateTime?)Convert.ToDateTime(dateVal) : null;
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new User();
        }

        static private UserList list = new UserList();
        public new static User SelectById(int id)
        {
            UserDB db = new UserDB();
            list = db.SelectAll();
            User g = list.Find(item => item.Id == id)!;
            return g;
        }

        public override void Insert(BaseEntity entity)
        {
            User user = entity as User;
            if (user != null)
            {
                inserted.Add(new ChangeEntity(base.CreateInsertdSQL, entity));
                inserted.Add(new ChangeEntity(this.CreateInsertdSQL, entity));
            }
        }

        public override void Delete(BaseEntity entity)
        {
            User user = entity as User;
            if (user != null)
            {
                deleted.Add(new ChangeEntity(this.CreateDeletedSQL, entity));
                deleted.Add(new ChangeEntity(base.CreateDeletedSQL, entity));
            }
        }

        public override void Update(BaseEntity entity)
        {
            User user = entity as User;
            if (user != null)
            {
                updated.Add(new ChangeEntity(this.CreateUpdatedSQL, entity));
                updated.Add(new ChangeEntity(base.CreateUpdatedSQL, entity));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            User c = entity as User;
            if (c != null)
            {
                string sqlStr = $"UPDATE [User] SET Username=@cUsername, Email=@cEmail, " +
                    $"Goal=@cGoal, streak=@cStreak, last_task_date=@cLastDate, grade_goal=@cGradeGoal WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUsername", c.Username));
                command.Parameters.Add(new OleDbParameter("@cEmail", c.Email));
                command.Parameters.Add(new OleDbParameter("@cGoal", c.Goal));
                command.Parameters.Add(new OleDbParameter("@cStreak", c.Streak));
                command.Parameters.Add(new OleDbParameter("@cLastDate", c.LastTaskDate.HasValue ? (object)c.LastTaskDate.Value : DBNull.Value));
                command.Parameters.Add(new OleDbParameter("@cGradeGoal", c.GradeGoal));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            User c = entity as User;
            if (c != null)
            {
                string sqlStr = $"Insert INTO [User] (Id, Email, Username, Goal, streak, last_task_date, grade_goal) VALUES (@id, @cEmail, @cUsername, @cGoal, @cStreak, @cLastDate, @cGradeGoal)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
                command.Parameters.Add(new OleDbParameter("@cEmail", c.Email ?? ""));
                command.Parameters.Add(new OleDbParameter("@cUsername", c.Username ?? ""));
                command.Parameters.Add(new OleDbParameter("@cGoal", c.Goal));
                command.Parameters.Add(new OleDbParameter("@cStreak", c.Streak));
                command.Parameters.Add(new OleDbParameter("@cLastDate", c.LastTaskDate.HasValue ? (object)c.LastTaskDate.Value : DBNull.Value));
                command.Parameters.Add(new OleDbParameter("@cGradeGoal", c.GradeGoal));
            }
        }
        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            User c = entity as User;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM [User] where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }
    }
}
