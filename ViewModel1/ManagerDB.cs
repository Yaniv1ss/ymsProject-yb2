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
    public class ManagerDB : PeopleDB
    {
        public new ManagerList SelectAll()
        {
            command.CommandText = $"SELECT People.* FROM (Manager INNER JOIN People ON Manager.id = People.Id)";
            ManagerList pList = new ManagerList(base.Select());
            return pList;
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Manager p = (entity as Manager)!;
            base.CreateModel(entity);
            return p;
        }

        public override BaseEntity NewEntity()
        {
            return new Manager();
        }

        static private ManagerList list = new ManagerList();
        public new static Manager SelectById(int id)
        {
            ManagerDB db = new ManagerDB();
            list = db.SelectAll();
            Manager g = list.Find(item => item.Id == id)!;
            return g;
        }
        public override void Insert(BaseEntity entity)
        {
            Manager manager = entity as Manager;
            if (manager != null)
            {
                inserted.Add(new ChangeEntity(base.CreateInsertdSQL, entity));
                inserted.Add(new ChangeEntity(this.CreateInsertdSQL, entity));
            }
        }

        public void InsertExistingIntoManagerOnly(Manager manager)
        {
            if (manager != null)
            {
                // Only insert into the Manager table, skipping PeopleDB base insert
                inserted.Add(new ChangeEntity(this.CreateInsertdSQL, manager));
            }
        }

        public void DeleteFromManagerOnly(Manager manager)
        {
            if (manager != null)
            {
                // Only delete from the Manager table, keeping the user
                deleted.Add(new ChangeEntity(this.CreateDeletedSQL, manager));
            }
        }

        public override void Delete(BaseEntity entity)
        {
            Manager manager = entity as Manager;
            if (manager != null)
            {
                deleted.Add(new ChangeEntity(this.CreateDeletedSQL, entity));
                deleted.Add(new ChangeEntity(base.CreateDeletedSQL, entity));
            }
        }

        public override void Update(BaseEntity entity)
        {
            Manager manager = entity as Manager;
            if (manager != null)
            {
                updated.Add(new ChangeEntity(this.CreateUpdatedSQL, entity));
                updated.Add(new ChangeEntity(base.CreateUpdatedSQL, entity));
            }
        }

        protected override void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Manager c = entity as Manager;
            if (c != null)
            {
                string sqlStr = $"UPDATE Manager SET Id=@id WHERE ID=@id";

                command.CommandText = sqlStr;

                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }
        
        protected override void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd)
        {
           Manager c = entity as Manager;
            if (c != null)
            {
                string sqlStr = $"Insert INTO  Manager (Id) VALUES (@id)";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@id", c.Id));
            }
        }

        protected override void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd)
        {
            Manager c = entity as Manager;
            if (c != null)
            {
                string sqlStr = $"DELETE FROM Manager where id=@pid";

                command.CommandText = sqlStr;
                command.Parameters.Add(new OleDbParameter("@pid", c.Id));
            }

        }
    }
}



