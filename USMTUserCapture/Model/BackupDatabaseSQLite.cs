﻿using USMTUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace USMTUserCapture.Model
{
    /// <summary>
    /// Backup database for recording backup jobs.  Currently dumps the
    /// data into a SQlite database for ease of management and portability.
    /// NOTICE: This was originally implemented with Microsoft SQL Compact but SQL
    /// Compact dropped support for opening databases over a network drive.
    /// As this tool requires running over the network, had to scrap it and move
    /// to SQLite
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
                Upgrade();
            }
            catch (Exception e)
            {

                MessageBox.Show("Something went really wrong with the Database");
                MessageBox.Show(e.ToString());
                Application.Current.Shutdown();
            }

        }

        private void Upgrade()
        {
            CreateVersionTable();
            CreateVersion2();
        }

        //Creates version table and sets version to one
        private void CreateVersionTable()
        {
            _connection.Open();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"select COUNT(name) from sqlite_master 
                                    WHERE type='table'
                                    AND name='Version';";
                var reader = command.ExecuteReader();
                reader.Read();
                //if there is no entry, this will equal true and create a version table
                bool tableCreated = reader.GetInt32(0) == 0;
                reader.Close(); //always close reader, even a connectin.close will not kill the lock on the db

                if (tableCreated)
                {
                    var createTable = _connection.CreateCommand();
                    createTable.CommandText = @"CREATE TABLE Version (DatabaseVersion INT );";
                    createTable.ExecuteNonQuery();
                    createTable.Dispose();
                    var insertVersion = _connection.CreateCommand();
                    insertVersion.CommandText = @"INSERT INTO Version([DatabaseVersion]) VALUES (1);";
                    insertVersion.ExecuteNonQuery();
                    insertVersion.Dispose();
                }
            }
            _connection.Close();
        }

        //creates CompletedBackups table where deleted backups go
        //Adds a size in GB column
        //Sets DB version to 2
        private void CreateVersion2()
        {
            _connection.Open();
            using (var dbVersion = _connection.CreateCommand())
            {
                dbVersion.CommandText = @"SELECT DatabaseVersion FROM Version WHERE ROWID = 1;";
                var reader = dbVersion.ExecuteReader();
                reader.Read();
                bool needsUpgrade = reader.GetInt32(0) == 1;
                reader.Close(); //always close reader, even a connectin.close will not kill the lock on the db

                if (needsUpgrade)
                {
                    var createCompletedBackupJobTable = _connection.CreateCommand();
                    createCompletedBackupJobTable.CommandText = @"CREATE TABLE CompletedJobs ( 
                                                                    ID             INTEGER,
                                                                    User           TEXT( 30 ),
                                                                    Computer       TEXT( 30 ),
                                                                    BackupLocation TEXT( 500 ),
                                                                    StartTime      DATETIME,
                                                                    EndTime        DATETIME,
                                                                    SizeMB         INTEGER
                                                                );";
                    createCompletedBackupJobTable.ExecuteNonQuery();
                    var addSizeColumn = _connection.CreateCommand();
                    addSizeColumn.CommandText = @"ALTER TABLE BackupEntries ADD COLUMN SizeMB INTEGER;";
                    addSizeColumn.ExecuteNonQuery();
                    var setDBVersion = _connection.CreateCommand();
                    setDBVersion.CommandText = @"UPDATE Version SET DatabaseVersion = 2 WHERE ROWID = 1";
                    setDBVersion.ExecuteNonQuery();
                }
            }
            _connection.Close();
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
            results.Close();
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
            reader.Close();
            command.Dispose();
            _connection.Close();

            return backups;
        }

        /// <summary>
        /// Moves the job from the BackupEntries to CompletedJobs.  Along with 
        /// </summary>
        /// <param name="job"></param>
        public void DeleteBackup(IUserJob job)
        {

            //figure out the size of the directory
            long sizeInBytes = 0;
            var dir = new DirectoryInfo(job.CurrentBackupLocation);
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                sizeInBytes += file.Length;
            }

            var usmtFolder = new DirectoryInfo(dir.FullName + "USMT\\");
            if (usmtFolder.Exists)
            {
                foreach (var file in usmtFolder.GetFiles())
                {
                    sizeInBytes += file.Length;
                }
            }

            
            _connection.Open();
            //add job to CompletedJobs
            using (var insertOldData = _connection.CreateCommand())
            {
                insertOldData.CommandText = @"INSERT INTO CompletedJobs ([ID],[User],[Computer],[BackupLocation],[StartTime],[EndTime],[SizeMB])
                                    VALUES (@id, @user, @computer, @backupLocation, @start, @end, @size)";
                insertOldData.Parameters.Add(new SQLiteParameter("@id", DbType.Int32));
                insertOldData.Parameters.Add(new SQLiteParameter("@user", DbType.String));
                insertOldData.Parameters.Add(new SQLiteParameter("@computer", DbType.String));
                insertOldData.Parameters.Add(new SQLiteParameter("@backupLocation", DbType.String));
                insertOldData.Parameters.Add(new SQLiteParameter("@start", DbType.DateTime));
                insertOldData.Parameters.Add(new SQLiteParameter("@end", DbType.DateTime));
                insertOldData.Parameters.Add(new SQLiteParameter("@size", DbType.Int32));

                insertOldData.Parameters["@id"].Value = job.ID;
                insertOldData.Parameters["@user"].Value = job.User;
                insertOldData.Parameters["@computer"].Value = job.Computer;
                insertOldData.Parameters["@backupLocation"].Value = job.BackupLocation;
                insertOldData.Parameters["@start"].Value = job.Start;
                insertOldData.Parameters["@end"].Value = job.End;
                insertOldData.Parameters["@size"].Value = sizeInBytes/1000000;

                insertOldData.Prepare();  //not needed

                insertOldData.ExecuteNonQuery();
                insertOldData.Dispose();

            }

            //delete job from backupEntries
            using (var deleteJob = _connection.CreateCommand())
            {
                deleteJob.CommandText = "DELETE FROM BackupEntries WHERE ID = @id;";
                deleteJob.Parameters.Add(new SQLiteParameter("@id", DbType.Int32));
                deleteJob.Parameters["@id"].Value = job.ID;

                deleteJob.ExecuteNonQuery();
                deleteJob.Dispose();
            }

            //delete the directory and do it recursivly
            Directory.Delete(job.CurrentBackupLocation, true);

            _connection.Close();
        }
    }
}
