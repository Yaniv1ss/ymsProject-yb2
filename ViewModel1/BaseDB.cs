using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
namespace ViewModel
{
    public abstract class BaseDB
    {
        protected static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                      + System.IO.Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location
                      + "/../../../../../ViewModel1/AccessTimeManagmentYM.accdb");
        protected static OleDbConnection? connection;
        protected OleDbCommand command;
        protected OleDbDataReader? reader;
        public static string Path()
        {
            String[] args = Environment.GetCommandLineArgs();
            string s;
            if (args.Length == 1)
            {
                s = args[0];
            }
            else
            {
                s = args[1];
                s = s.Replace("/service:", "");
            }
            string[] st = s.Split('\\');
            int x = st.Length - 6;
            st[x] = "VViewModel";
            Array.Resize(ref st, x + 1);
            string str = String.Join('\\', st);
            return str;
        }
        //C:\Users\nativ\Downloads\Exampl_Project\MyWhatApp\VViewModel\ExampleProjectBagrutGrades.accdb
        public BaseDB()
        {
            var x = Path();
            connection ??= new OleDbConnection(connectionString);
            command = new OleDbCommand();
            command.Connection = connection!;
        }

        public abstract BaseEntity NewEntity();



        protected List<BaseEntity> Select()
        {
            List<BaseEntity> list = new List<BaseEntity>();
            try
            {
                command.Connection = connection!;
                if (connection!.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BaseEntity entity = NewEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                
                System.Diagnostics.Debug.WriteLine(
                    e.Message + "\nSQL:" + command.CommandText);
                throw new Exception(e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (reader != null) reader.Close();
                //   if (connection.State == ConnectionState.Open) connection.Close();
            }
            return list;
        }

        protected async Task<List<BaseEntity>> SelectAsync(string sqlStr)
        {
            OleDbConnection connection = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            List<BaseEntity> list = new List<BaseEntity>();

            try
            {
                command.Connection = connection;
                command.CommandText = sqlStr;
                connection.Open();
                this.reader = (OleDbDataReader)await command.ExecuteReaderAsync();


                while (reader.Read())
                {
                    BaseEntity entity = NewEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\nSQL:" + command.CommandText);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return list;
        }


        protected virtual BaseEntity CreateModel(BaseEntity entity)
        {
            entity.Id = (int)reader!["id"];
            return entity;
        }

        protected abstract void CreateDeletedSQL(BaseEntity entity, OleDbCommand cmd);
        public static List<ChangeEntity> deleted = new List<ChangeEntity>();


        public virtual void Delete(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null & entity.GetType() == reqEntity.GetType())
            {
                deleted.Add(new ChangeEntity(this.CreateDeletedSQL, entity));
            }
        }

        protected abstract void CreateInsertdSQL(BaseEntity entity, OleDbCommand cmd);
        public static List<ChangeEntity> inserted = new List<ChangeEntity>();


        public virtual void Insert(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null & entity.GetType() == reqEntity.GetType())
            {
                inserted.Add(new ChangeEntity(this.CreateInsertdSQL, entity));
            }
        }

        protected abstract void CreateUpdatedSQL(BaseEntity entity, OleDbCommand cmd);
        public static List<ChangeEntity> updated = new List<ChangeEntity>();


        public virtual void Update(BaseEntity entity)
        {
            BaseEntity reqEntity = this.NewEntity();
            if (entity != null & entity.GetType() == reqEntity.GetType())
            {
                updated.Add(new ChangeEntity(this.CreateUpdatedSQL, entity));
            }
        }
        public class ChangeEntity
        {
            private BaseEntity entity;
            private CreateSql createSql;

            public ChangeEntity(CreateSql createSql, BaseEntity entity)
            {
                this.createSql = createSql;
                this.entity = entity;
            }

            public BaseEntity Entity { get => entity; set => entity = value; }
            public CreateSql CreateSql { get => createSql; set => createSql = value; }
        }
        public delegate void CreateSql(BaseEntity entity,
                                                       OleDbCommand command);


 





        public int SaveChanges()
        {
            OleDbTransaction trans = null;
            int records_affected = 0;

            try
            {
                command.Connection = connection!;

                if (connection!.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                trans = connection.BeginTransaction();
                command.Transaction = trans;

                foreach (var entity in inserted)
                {
                    command.Parameters.Clear();
                    entity.CreateSql(entity.Entity, command); 
                    int affected = command.ExecuteNonQuery();
                    records_affected += affected;

                    if (affected > 0 && entity.Entity.Id == 0)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "Select @@Identity";
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            entity.Entity.Id = Convert.ToInt32(result);
                        }
                    }
                }

                foreach (var entity in updated)
                {
                    command.Parameters.Clear();
                    entity.CreateSql(entity.Entity, command);        //cmd.CommandText = CreateUpdateSQL(entity.Entity);
                    records_affected += command.ExecuteNonQuery();
                }

                foreach (var entity in deleted)
                {
                    command.Parameters.Clear();
                    entity.CreateSql(entity.Entity, command);

                    records_affected += command.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                records_affected = 0;
                System.Diagnostics.Debug.WriteLine(ex.Message + "\n SQL:" + command.CommandText);
            }
            finally
            {
                inserted.Clear();
                updated.Clear();
                deleted.Clear();
            }

            return records_affected;
        }

    }
}



