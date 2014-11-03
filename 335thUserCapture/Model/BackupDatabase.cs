using _335thUserCapture.Interfaces;
using System.Data.SqlServerCe;
using System;
using System.Data;
using System.Collections.Generic;
using System.Windows;

namespace _335thUserCapture.Model
{
    public class BackupDatabase : ISaveBackupInformation, IDisposable, IGetBackupInformation
    {
        SqlCeConnection _connection;
        public BackupDatabase()
        {

            try
            {
                _connection = new SqlCeConnection(@"Data Source = .\DB\BackupDetails.sdf");
            }
            catch (Exception e)
            {

                MessageBox.Show("Something went really wrong with the Database");
                MessageBox.Show(e.ToString());
            }
        }

        public int SaveBackupInfo(string user, string computer, string backupLocation)
        {
            try{
                _connection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went really wrong with opening the Database");
                MessageBox.Show(e.ToString());
            }
            DateTime now = DateTime.Now;
            var command = _connection.CreateCommand();

            //insert data
            command.CommandText = @"INSERT INTO BackupEntries ([User],[Computer],[BackupLocation],[Start])
                                    VALUES (@user, @computer, @backupLocation, @now)";
            SqlCeParameter param = new SqlCeParameter("@user", SqlDbType.NVarChar);
            command.Parameters.Add(param);
            param = new SqlCeParameter("@computer", SqlDbType.NVarChar);
            command.Parameters.Add(param);
            param = new SqlCeParameter("@backupLocation", SqlDbType.NVarChar);
            command.Parameters.Add(param);
            param = new SqlCeParameter("@now", SqlDbType.DateTime);
            command.Parameters.Add(param);

            command.Parameters["@user"].Value = user;
            command.Parameters["@computer"].Value = computer;
            command.Parameters["@backupLocation"].Value = backupLocation;
            command.Parameters["@now"].Value = now;

            command.Prepare();

            command.ExecuteNonQuery();
            command.Dispose();

            command = _connection.CreateCommand();

            //Lets get the row ID and return it
            command.CommandText = @"SELECT ID FROM BackupEntries WHERE (Start = @now)";

            param = new SqlCeParameter("@now", SqlDbType.DateTime);
            command.Parameters.Add(param);
            command.Parameters["@now"].Value = now;

            command.Prepare();

            var results = command.ExecuteReader();

            int ID = 0;
            //Not implemented in this database (documentation WTF)
            //if (results.HasRows == false)
            //    throw new ApplicationException("Database has no rows");
            while (results.Read())
            {
                ID = (int)results["ID"];
            }

            //kill everything off
            results.Dispose();
            command.Dispose();
            _connection.Close();

            return ID;
        }

        public void CompletedBackup(int ID)
        {
            try
            {
                _connection.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went really wrong with opening the Database");
            }

            DateTime now = DateTime.Now;
            var command = _connection.CreateCommand();

            //UPDATE data
            command.CommandText = @"UPDATE BackupEntries SET [End] = @now WHERE ID = @ID";
            SqlCeParameter param = new SqlCeParameter("@ID", SqlDbType.Int);
            command.Parameters.Add(param);
            param = new SqlCeParameter("@now", SqlDbType.DateTime);
            command.Parameters.Add(param);
            command.Parameters["@ID"].Value = ID;
            command.Parameters["@now"].Value = now;
            command.ExecuteNonQuery();

            //kill everything off
            command.Dispose();
            _connection.Close();
        }

        public List<IUserJob> AllBackups()
        {
            try
            {
                _connection.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went really wrong with opening the Database");
            }

            var command = _connection.CreateCommand();

            //Get all data
            command.CommandText = @"SELECT ID, [User], Computer, BackupLocation, Start, [End] FROM BackupEntries ORDER BY [Start] DESC";
            command.Prepare();
            var reader = command.ExecuteReader();

            var backups = new List<IUserJob>();
            BackupJob temp;
            while (reader.Read())
            {
                temp = new BackupJob();
                temp.ID = (int)reader["ID"];
                temp.User = (string)reader["User"];
                temp.Computer = (string)reader["Computer"];
                temp.BackupLocation = (string)reader["BackupLocation"];
                temp.Start = (DateTime)reader["Start"];
                if (reader.IsDBNull(5)==false)
                    temp.End = (DateTime)reader["End"];

                backups.Add(temp);
            }

            //kill everything off
            command.Dispose();
            _connection.Close();

            return backups;
        }



        public void Dispose()
        {
            _connection.Dispose();
        }
        
    }

    public class BackupJob : IUserJob{

        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _user;
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        private string _computer;
        public string Computer
        {
            get
            {
                return _computer;
            }
            set
            {
                _computer = value;
            }
        }

        private string _backupLocation;
        public string BackupLocation
        {
            get
            {
                return _backupLocation;
            }
            set
            {
                _backupLocation = value;
            }
        }

        private DateTime _start;
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }

        private DateTime _end;
        public DateTime End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
            }
        }

        public override string ToString()
        {
            return "User:" + this.User + "\r\nComputer:" + this.Computer + "\r\nStart time:" + Start.ToLocalTime();
        }

        private string _currentBackupLocation;
        public string CurrentBackupLocation
        {
            get
            {
                return _currentBackupLocation;
            }
            set
            {
                _currentBackupLocation = value;
            }
        }
    }
}
