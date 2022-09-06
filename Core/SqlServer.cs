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

        public bool add(string name)
        {

            SQLiteDataReader sql_read;
            SQLiteCommand cmd = sqlServer.CreateCommand();


            cmd.CommandText = $"SELECT NAME FROM project where NAME = '{name}'";
            sql_read = cmd.ExecuteReader();
            if (sql_read.Read())
            {
                MessageBox.Show("数据已存在！", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            cmd.Dispose();

            cmd.CommandText = "INSERT INTO project(NAME,COUNTTIME,TIMES) VALUES(@name, @counttime, @times)";
            cmd.Parameters.Add("name", DbType.String).Value = name;
            cmd.Parameters.Add("counttime", DbType.String).Value = "0000:00:00";
            cmd.Parameters.Add("times", DbType.Int32).Value = 0;
            cmd.ExecuteNonQuery();

            cmd.CommandText = $"SELECT NAME FROM project where NAME = '{name}'";
            sql_read = cmd.ExecuteReader();
            if (sql_read.Read())
                return true;
            else
            {
                MessageBox.Show("插入失败！", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
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
