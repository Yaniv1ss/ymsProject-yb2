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
    public class PeopleDB : BaseDB                                                                                                                                                                     
    {
        public PeopleList SelectAll()
        {
            command.CommandText = $"SELECT * FROM People";
            PeopleList pList = new PeopleList(base.Select());
            return pList;
        }


        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            People p = (entity as People)!;
            p.Username = reader!["userName"].ToString();
            p.Pass = reader!["pass"].ToString();
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new People();
        }

        static private PeopleList list = new PeopleList();
        public static People SelectById(int id)
        {
            PeopleDB db = new PeopleDB();
            list = db.SelectAll();
            People g = list.Find(item => item.Id == id)!;
            return g;
        }

       

        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
            People c = entity as People;
            if (c != null)
            {
                string sqlStr = $"Insert INTO People (Username,Pass) VALUES (@cUsername,@cPass)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUsername", c.Username));
                command.Parameters.Add(new OleDbParameter("@cPass", c.Pass));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            People c = entity as People;
            if (c != null)
            {
                string sqlStr = $"UPDATE People SET Username=@cUsername, Pass=@cPass " +
                    $" WHERE ID=@id";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@cUsername", c.Username));
                command.Parameters.Add(new OleDbParameter("@cPass", c.Pass));
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            People c = entity as People;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM People where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }
        }

    }
}


