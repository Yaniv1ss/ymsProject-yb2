using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ExamsDB : BaseDB
    {
        public ExamsList SelectAll()
        {
            command.CommandText = $"SELECT * FROM Exams";
            ExamsList pList = new ExamsList(base.Select());
            return pList;
        }
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Exams p = entity as Exams;
            p.Subject_id = SubjectDB.SelectById((int)reader["subject_id"]);
            p.Title = reader["title"].ToString();
            p.Exam_date = reader["exam_date"].ToString();
            // קריאת ציון
            object gradeVal = reader["grade"];
            p.Grade = (gradeVal != DBNull.Value) ? Convert.ToInt32(gradeVal) : 0;
            base.CreateModel(entity);
            return p;
        }
        public override BaseEntity NewEntity()
        {
            return new Exams();
        }
        static private ExamsList list = new ExamsList();
        public static Exams SelectById(int id)
        {
            ExamsDB db = new ExamsDB();
            list = db.SelectAll();

            Exams g = list.Find(item => item.Id == id);
            return g;
        }

       
       
        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Exams c = entity as Exams;
            if (c != null)
            {
                string sqlStr = $"UPDATE Exams SET Subject_id=@Subject_id, Title=@Title, " +
                    $"Exam_date=@Exam_date, grade=@cGrade WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@Subject_id", c.Subject_id.Id));
                command.Parameters.Add(new OleDbParameter("@Title", c.Title));
                command.Parameters.Add(new OleDbParameter("@Exam_date", c.Exam_date));
                command.Parameters.Add(new OleDbParameter("@cGrade", c.Grade));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }

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
                string sqlStr = $"Insert INTO Exams (Subject_id,Title,Exam_date,grade) VALUES (@Subject_id,@cTitle,@Exam_date,@cGrade)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@Subject_id", c.Subject_id.Id));
                command.Parameters.Add(new OleDbParameter("@Title", c.Title));
                command.Parameters.Add(new OleDbParameter("@Exam_date", c.Exam_date));
                command.Parameters.Add(new OleDbParameter("@cGrade", c.Grade));
            }
        }
    }
}