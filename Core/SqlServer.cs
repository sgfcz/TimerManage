using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.IO;
using TimeManager.Model;

namespace TimeManager.Core
{
    public class SqlServer
    {
        SQLiteConnection sqlServer;
        public SqlServer()
        {
            if (!Directory.Exists("./Data/"))
            {
                Directory.CreateDirectory("./Data");
            }
            sqlServer = new SQLiteConnection("data source=./Data/base.db3"); //需要在生成目录新建Data文件夹
        }
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

        public bool delete(string commandText)
        {
            SQLiteCommand cmd = sqlServer.CreateCommand();
            cmd.CommandText = commandText;
            cmd.ExecuteNonQuery();
            return true;
        }

        public bool Update(string projectName, string nowTime)
        {
            if (isOpen())
            {
                sqlServer.Open();
            }
            string countTime = "";
            int countTimes = 0;
            SQLiteDataReader sql_read;
            SQLiteCommand cmd = sqlServer.CreateCommand();
            cmd.CommandText = "SELECT * FROM project WHERE NAME=\"" + projectName + "\"";
            sql_read = cmd.ExecuteReader();
            while (sql_read.Read())
            {
                countTime = sql_read.GetString(2);
                countTimes = sql_read.GetInt32(3);

            }
            cmd.Dispose();

            string[] countTimeWords = countTime.Split(':');
            string[] nowTimeWords = nowTime.Split(':');
            List<int> countTimeInts = new();
            for(int i = 0; i < countTimeWords.Length; i++)
            {
                countTimeInts.Add(Convert.ToInt32(countTimeWords[i]) + Convert.ToInt32(nowTimeWords[i])); 
            }
            countTimes = countTimes + 1;
            countTime  = $"{countTimeInts[0].ToString().PadLeft(4, '0')}:{countTimeInts[1].ToString().PadLeft(2, '0')}:" +
                $"{countTimeInts[2].ToString().PadLeft(2, '0')}";

            cmd.CommandText = "UPDATE project SET COUNTTIME = @count, TIMES = @times WHERE NAME = @name";
            cmd.Parameters.AddWithValue("@count", countTime);
            cmd.Parameters.AddWithValue("@times", countTimes);
            cmd.Parameters.AddWithValue("@name", projectName);
            cmd.ExecuteNonQuery();

            return true;
        }

        public bool UpdateLast(string name)
        {
            SQLiteCommand cmd = sqlServer.CreateCommand();
            cmd.CommandText = "DELETE FROM last";
            cmd.ExecuteNonQuery();
            //cmd.Dispose();

            cmd.CommandText = "INSERT INTO last(NAME) VALUES(@name)";
            cmd.Parameters.Add("name", DbType.String).Value = name;
            cmd.ExecuteNonQuery();

            return true;
        }

        public bool isOpen()
        {
            if (sqlServer.State != ConnectionState.Open)
                return true;
            else 
                return false;
        }
        
        public List<String> search(string commandText)
        {
            if (isOpen())
            {
                sqlServer.Open();
            }
            SQLiteDataReader sql_read;
            SQLiteCommand cmd = sqlServer.CreateCommand();
            cmd.CommandText = commandText;
            sql_read = cmd.ExecuteReader();
            List<string> names = new();
            while (sql_read.Read())
            {
                names.Add(sql_read.GetString(0));
            }
            return names;
        }

        public List<ProjectMessages> searchMessage(string commandText)
        {
            if (isOpen())
            {
                sqlServer.Open();
            }
            SQLiteDataReader sql_read;
            SQLiteCommand cmd = sqlServer.CreateCommand();
            cmd.CommandText = commandText;
            sql_read = cmd.ExecuteReader();
            List<ProjectMessages> projectMessages = new();
            while (sql_read.Read())
            {
                ProjectMessages message = new ProjectMessages { CountTime = sql_read.GetString(2), 
                                                                    Times = sql_read.GetInt32(3) };
                projectMessages.Add(message);
            }
            return projectMessages;
        }
        private void newTable()
        {
           if (isOpen())
            {
                sqlServer.Open();
            }
           if (sqlServer.GetSchema("Tables").Rows.Count != 0)
            {
                return;
            }
            string table1 = "CREATE TABLE project" +
                "(ID INTEGER PRIMARY KEY AUTOINCREMENT," +
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
        }
    }

}
