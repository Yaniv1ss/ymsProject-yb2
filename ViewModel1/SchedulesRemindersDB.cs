using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ScheduleReminderDB : BaseDB
    {
        public SchedulesRemindersList SelectAll()
        {
            command.CommandText = $"SELECT * FROM Schedule_Reminder";
            SchedulesRemindersList pList = new SchedulesRemindersList(base.Select());
            return pList;
        }


        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            SchedulesRemainders p = entity as SchedulesRemainders;
            p.SchedulesId = SchedulesDB.SelectById((int)reader["Schedule_Id"]);
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new SchedulesRemainders();
        }

        static private SchedulesRemindersList list = new SchedulesRemindersList();
        public static SchedulesRemainders SelectById(int id)
        {
            ScheduleReminderDB db = new ScheduleReminderDB();
            list = db.SelectAll();
            SchedulesRemainders g = list.Find(item => item.Id == id);
            return g;
        }

       
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            SchedulesRemainders c = entity as SchedulesRemainders;
            if (c != null)
            {
                string sqlStr = $"Insert INTO  Schedule_Reminder (SchedulesId) VALUES (@cSchedulesId)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSchedulesId", c.SchedulesId));
            }
        }


        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            SchedulesRemainders c = entity as SchedulesRemainders;
            if (c != null)
            {
                string sqlStr = $"UPDATE Schedule_Reminder  SET Schedule_Reminder=@cSchedulesId WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSchedulesId", c.SchedulesId));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            SchedulesRemainders c = entity as SchedulesRemainders;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM Schedule_Reminder where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }
    }
}




