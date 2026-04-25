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
    public class SchedulesDB : BaseDB
    {
        public SchedulesList SelectAll()
        {
            command.CommandText = $"SELECT * FROM Schedules";
            SchedulesList pList = new SchedulesList(base.Select());
            return pList;
        }


        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Schedules p = (entity as Schedules)!;
            p.User_id = UserDB.SelectById((int)reader!["user_id"])!;
            p.Subject_id = SubjectDB.SelectById((int)reader!["subject_id"])!;
            p.Day_of_the_week = reader!["day_of_week"].ToString();
            p.Hour = Convert.ToInt32(reader!["hour"]);
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new Schedules();
        }

        static private SchedulesList list = new SchedulesList();
        public static Schedules SelectById(int id)
        {
            SchedulesDB db = new SchedulesDB();
            list = db.SelectAll();
            Schedules g = list.Find(item => item.Id == id)!;
            return g;
        }

      
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Schedules c = (entity as Schedules)!;
            if (c != null)
            {
                string sqlStr = $"Insert INTO Schedules (User_id, day_of_week, [Hour], Subject_id) VALUES (@cUser_id, @cDay_of_the_week, @cHour, @cSubject_id)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUser_id", c.User_id!.Id));
                command.Parameters.Add(new OleDbParameter("@cDay_of_the_week", c.Day_of_the_week));
                command.Parameters.Add(new OleDbParameter("@cHour", c.Hour));
                command.Parameters.Add(new OleDbParameter("@cSubject_id", c.Subject_id!.Id));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Schedules c = (entity as Schedules)!;
            if (c != null)
            {
                string sqlStr = $"UPDATE Schedules SET day_of_week=@day_of_week, User_id=@user_id, [Hour]=@chour, " +
                    $"Subject_id=@subject_id WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@day_of_week", c.Day_of_the_week));
                command.Parameters.Add(new OleDbParameter("@user_id", c.User_id!.Id));
                command.Parameters.Add(new OleDbParameter("@chour", c.Hour));
                command.Parameters.Add(new OleDbParameter("@subject_id", c.Subject_id!.Id));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Schedules c = entity as Schedules;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM Schedules where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }



    }


}


