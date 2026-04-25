using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class UserSubjectDB : BaseDB
    {
        public UserSubjectList SelectAll()
        {
            command.CommandText = $"SELECT * FROM User_Subject";
            UserSubjectList pList = new UserSubjectList(base.Select());
            return pList;
        }


        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            UserSubject p = entity as UserSubject;
            p.User_id = UserDB.SelectById((int)reader["User_id"]);
            p.Subject_id = SubjectDB.SelectById((int)reader["Subject_id"]);
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new UserSubject();
        }

        static private UserSubjectList list = new UserSubjectList();
        public static UserSubject SelectById(int id)
        {
            UserSubjectDB db = new UserSubjectDB();
            list = db.SelectAll();
            UserSubject g = list.Find(item => item.Id == id);
            return g;
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            UserSubject c = entity as UserSubject;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM User_Subject where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }


        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            UserSubject c = entity as UserSubject;
            if (c != null)
            {
                string sqlStr = $"Insert INTO  User_Subject (User_id,Subject_id) VALUES (@cUser_id,@cSubject_id)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUser_id", c.User_id.Id));
                command.Parameters.Add(new OleDbParameter("@cSubject_id", c.Subject_id.Id));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            UserSubject c = entity as UserSubject;
            if (c != null)
            {
                string sqlStr = $"UPDATE User_Subject SET User_id=@cUser_id, Subject_id=@cSubject_id " +
                    $" WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUser_id", c.User_id.Id));
                command.Parameters.Add(new OleDbParameter("@cExam_id", c.Subject_id.Id));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        }
    }

