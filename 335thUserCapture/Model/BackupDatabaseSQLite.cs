using _335thUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _335thUserCapture.Model
{
    public class BackupDatabaseSQLite : IGetBackupInformation, ISaveBackupInformation
    {

        private SQLiteConnection _connection;
        private IFolderInformation _folders;
        public BackupDatabaseSQLite(IFolderInformation folders)
        {
            _folders = folders;
            try
            {

                _connection = new SQLiteConnection(@"Data Source = .\DB\BackupDetails.sqlite");
            }
            catch (Exception e)
            {

                MessageBox.Show("Something went really wrong with the Database");
                MessageBox.Show(e.ToString());
            }
        }
        public int SaveBackupInfo(string user, string computer, string backupLocation)
        {
            _connection.Open();
            DateTime now = DateTime.Now;
            var command = _connection.CreateCommand();
            //insert data
            command.CommandText = @"INSERT INTO BackupEntries ([User],[Computer],[BackupLocation],[StartTime])
                                    VALUES (@user, @computer, @backupLocation, @now)";
            command.Parameters.Add(new SQLiteParameter("@user", DbType.String));
            command.Parameters.Add(new SQLiteParameter("@computer", DbType.String));
            command.Parameters.Add(new SQLiteParameter("@backupLocation", DbType.String));
            command.Parameters.Add(new SQLiteParameter("@now", DbType.DateTime));

            command.Parameters["@user"].Value = user;
            command.Parameters["@computer"].Value = computer;
            command.Parameters["@backupLocation"].Value = backupLocation;
            command.Parameters["@now"].Value = now;

            command.Prepare();

            command.ExecuteNonQuery();
            command.Dispose();

            command = _connection.CreateCommand();

            //Lets get the row ID and return it
            command.CommandText = @"SELECT ID FROM BackupEntries WHERE (StartTime = @now)";
            command.Parameters.Add(new SQLiteParameter("@now", DbType.DateTime));
            command.Parameters["@now"].Value = now;

            command.Prepare();

            var results = command.ExecuteReader();

            int ID = 0;
            //Not implemented in this database (documentation WTF)
            //if (results.HasRows == false)
            //    throw new ApplicationException("Database has no rows");
            while (results.Read())
            {
                ID = results.GetInt32(0);
            }

            //kill everything off
            results.Dispose();
            command.Dispose();
            _connection.Close();

            return ID;
        }

        public void CompletedBackup(int ID)
        {
            _connection.Open();

            DateTime now = DateTime.Now;
            var command = _connection.CreateCommand();

            //UPDATE data
            command.CommandText = @"UPDATE BackupEntries SET [EndTime] = @now WHERE ID = @ID";
            command.Parameters.Add(new SQLiteParameter("@ID", DbType.Int32));
            command.Parameters.Add(new SQLiteParameter("@now", DbType.DateTime));
            
            command.Parameters["@ID"].Value = ID;
            command.Parameters["@now"].Value = now;
            command.ExecuteNonQuery();

            //kill everything off
            command.Dispose();
            _connection.Close();
        }

        public List<IUserJob> AllBackups()
        {
            _connection.Open();
            var command = _connection.CreateCommand();

            //Get all data
            command.CommandText = @"SELECT ID, [User], Computer, BackupLocation, StartTime, EndTime FROM BackupEntries ORDER BY [StartTime] DESC";
            command.Prepare();
            var reader = command.ExecuteReader();

            var backups = new List<IUserJob>();
            BackupJob temp;
            while (reader.Read())
            {
                temp = new BackupJob();
                temp.ID = reader.GetInt32(0);
                temp.User = (string)reader["User"];
                temp.Computer = (string)reader["Computer"];
                temp.BackupLocation = (string)reader["BackupLocation"];
                temp.Start = DateTime.SpecifyKind(reader.GetDateTime(4),DateTimeKind.Local);
                if (reader.IsDBNull(5) == false)
                    temp.End = DateTime.SpecifyKind(reader.GetDateTime(5), DateTimeKind.Local);
                temp.CurrentBackupLocation = _folders.BaseFolder + temp.BackupLocation;
                backups.Add(temp);
            }

            //kill everything off
            command.Dispose();
            _connection.Close();

            return backups;
        }
    }
}
