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
    /// <summary>
    /// Backup database for recording backup jobs.  Currently dumps the
    /// data into a SQlite database for ease of management and portability.
    /// NOTICE: This was originally implemented with Microsoft SQL Compact but SQL
    /// Compact dropped support for opening databases over a network drive.
    /// As this tool requires running over the network, had to scrap it and move
    /// to SQLIte
    /// </summary>
    /// <see cref="http://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki"/>
    /// <seealso cref="https://connect.microsoft.com/SQLServer/feedback/details/646333/sql-ce-4-0-no-longer-supports-opening-files-on-a-network-share"/>
    public class BackupDatabaseSQLite : IGetBackupInformation, ISaveBackupInformation
    {

        private SQLiteConnection _connection;
        private IFolderInformation _folders;

        /// <summary>
        /// Currently needs the database and tables to already be created
        /// TODO: Check for existing file and create if necessary
        /// </summary>
        /// <param name="folders"></param>
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

        /// <summary>
        /// Records the Start of a backup job. Always finish the backkup entry
        /// by calling CompleteBackup with the ID returned by this method
        /// </summary>
        /// <param name="user">User that was backed up</param>
        /// <param name="computer">Computername</param>
        /// <param name="backupLocation">Relative location to the 335UserCapture Executable</param>
        /// <returns>ID ofthe record.  Save it to call CompleteBackup</returns>
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

        /// <summary>
        /// This annotates when the backup is complete
        /// </summary>
        /// <param name="ID">ID of the record to update</param>
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

        /// <summary>
        /// All backups are returned as IUserJobs
        /// </summary>
        /// <returns>All Jobs in the database</returns>
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
