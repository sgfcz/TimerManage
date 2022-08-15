using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace TimeManager.Core
{
    internal class SqlServer
    {
        SQLiteConnection sqlServer = new SQLiteConnection("data source=./Data/base.db3");
        public bool connect()
        {
            try
            {
                sqlServer.Open();
                newTable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public void close()
        {
            sqlServer.Close();
        }

        public bool add()
        {
            return true;
        }

        public bool delete()
        {
            return true;
        }
        
        public List<string> search()
        {
            SQLiteDataReader sql_read;
            SQLiteCommand cmd = sqlServer.CreateCommand();
            cmd.CommandText = "SELECT NAME FROM project";
            sql_read = cmd.ExecuteReader();
            List<string> names = new List<string>();
            while (sql_read.Read())
            {
                names.Add(sql_read.GetString(0));
            }
            return names;
        }
        private void newTable()
        {
           if (sqlServer.State != ConnectionState.Open)
            {
                sqlServer.Open();
            }
           if (sqlServer.GetSchema("Tables").Rows.Count != 0)
            {
                return;
            }
            string table1 = "CREATE TABLE project" +
                "(ID INT PRIMARY KEY," +
                "NAME Text," +
                "COUNTTIME TEXT," +
                "TIMES INT)";
            string table2 = "CREATE TABLE last" +
                "(NAME TEXT PRIMARY KEY)";
            SQLiteCommand cmd = sqlServer.CreateCommand();
            cmd.CommandText = table1;
            cmd.ExecuteNonQuery();
            cmd.CommandText = table2;
            cmd.ExecuteNonQuery();
            sqlServer.Close();
        }
    }

}
