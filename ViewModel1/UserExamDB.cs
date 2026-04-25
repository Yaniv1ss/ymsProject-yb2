using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class UserExamDB : BaseDB
    {
        public UserExamList SelectAll()
        {
            command.CommandText = $"SELECT * FROM UserE";
            UserExamList pList = new UserExamList(base.Select());
            return pList;
        }


        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            UserExam p = entity as UserExam;
            p.User_id = UserDB.SelectById((int)reader["User_id"]);
            p.Exam_id = ExamsDB.SelectById((int)reader["Exam_id"]);
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new UserExam();
        }

        static private UserExamList list = new UserExamList();
        public static UserExam SelectById(int id)
        {
            UserExamDB db = new UserExamDB();
            list = db.SelectAll();
            UserExam g = list.Find(item => item.Id == id);
            return g;
        }

        
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            UserExam c = entity as UserExam;
            if (c != null)
            {
                string sqlStr = $"Insert INTO UserE (User_id, Exam_id) VALUES (@cUser_id, @cExam_id)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUser_id", c.User_id.Id));
                command.Parameters.Add(new OleDbParameter("@cExam_id", c.Exam_id.Id));
            }
        }


        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            UserExam c = entity as UserExam;
            if (c != null)
            {
                string sqlStr = $"UPDATE UserE  SET User_id=@cUser_id ,Exam_id=@Exam_id WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUser_id", c.User_id.Id));
                command.Parameters.Add(new OleDbParameter("@cExam_id", c.Exam_id.Id));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            UserExam c = entity as UserExam;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM UserE where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }


    }

}

