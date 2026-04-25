using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ExamsRemaindersDB : BaseDB
    {
        public ExamsRemaindersList SelectAll()
        {
            command.CommandText = $"SELECT * FROM ExamsRemainders";
            ExamsRemaindersList pList = new ExamsRemaindersList(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            ExamsRemainders p = entity as ExamsRemainders;
            p.Subject_id = SubjectDB.SelectById((int)reader["subject_id"]);
            base.CreateModel(entity);
            return p;
        }
        public override BaseEntity NewEntity()
        {
            return new ExamsRemainders();
        }
        static private ExamsRemaindersList list = new ExamsRemaindersList();
        public static ExamsRemainders SelectById(int id)
        {
            ExamsRemaindersDB db = new ExamsRemaindersDB();
            list = db.SelectAll();

            ExamsRemainders g = list.Find(item => item.Id == id);
            return g;
        }

      

        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            ExamsRemainders c = entity as ExamsRemainders;
            if (c != null)
            {
                string sqlStr = $"Insert INTO  ExamsRemainders (subject_id) VALUES (@Subject_id)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@Subject_id", c.Subject_id.Id));
             
            }
        }


        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            ExamsRemainders c = entity as ExamsRemainders;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM ExamsRemainders where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }
        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            ExamsRemainders c = entity as ExamsRemainders;
            if (c != null)
            {
                string sqlStr = $"UPDATE ExamsRemainders SET Subject_id=@Subject_id WHERE ID=@id";

            
                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@Subject_id", c.Subject_id.Id));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }

    }   
}
