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
    public class SubjectDB : BaseDB
    {
        public SubjectList SelectAll()
        {
            command.CommandText = $"SELECT * FROM Subject";
            SubjectList pList = new SubjectList(base.Select());
            return pList;
        }


        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Subject p = (entity as Subject)!;
            p.Subject_name = reader!["subject_name"].ToString();
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new Subject();
        }

        static private SubjectList list = new SubjectList();
        public static Subject SelectById(int id)
        {
            SubjectDB db = new SubjectDB();
            list = db.SelectAll();
            Subject g = list.Find(item => item.Id == id)!;
            return g;
        }

        
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Subject c = (entity as Subject)!;
            if (c != null)
            {
                string sqlStr = $"Insert INTO [Subject] (Subject_name) VALUES (@cSubject_name)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSubject_name", c.Subject_name));
            }
        }


        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
           Subject c = (entity as Subject)!;
            if (c != null)
            {
                string sqlStr = $"UPDATE [Subject] SET Subject_name=@cSubject_name WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cSubject_name", c.Subject_name));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Subject c = (entity as Subject)!;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM [Subject] WHERE ID=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }




        public static void EnsureDefaultSubjects()
        {
            SubjectDB db = new SubjectDB();
            var existing = db.SelectAll();
            string[] defaults = { "מתמטיקה", "אנגלית", "תנ\"ך", "היסטוריה", "פיזיקה", "מדעי המחשב", "ספרות", "לשון" };

            foreach (var name in defaults)
            {
                if (!existing.Any(s => s.Subject_name == name))
                {
                    db.Insert(new Subject { Subject_name = name });
                }
            }
            db.SaveChanges();
        }
    }
}

