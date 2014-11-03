using System;
using System.Collections.Generic;

namespace _335thUserCapture.Interfaces
{
    /// <summary>
    /// Interface to save and retrieve all information from a saved location
    /// </summary>
    public interface ISaveBackupInformation
    {
        /// <summary>
        /// Used to start the backup job
        /// </summary>
        /// <param name="user">User that is backed up.  If null, no value is inserted.</param>
        /// <param name="computer">Computer name</param>
        /// <param name="backupLocation">Name of the folder for the backup EX: 201410151220</param>
        /// <returns>The ID of the generated row</returns>
        int SaveBackupInfo(string user, string computer, string backupLocation);
        /// <summary>
        /// Signals that the backup job is complete
        /// </summary>
        /// <param name="ID">The int returned from SaveBackupInfo</param>
        void CompletedBackup(int ID);
        /// <summary>
        /// return all backups
        /// </summary>
        /// <returns>All performed backups in order from newest to oldest</returns>
        List<IUserJob> AllBackups();
    }

    public interface IGetBackupInformation
    {
        /// <summary>
        /// return all backups
        /// </summary>
        /// <returns>All performed backups in order from newest to oldest</returns>
        List<IUserJob> AllBackups();
    }

    public interface IUserJob
    {
        int ID { get; set; }
        string User { get; set; }
        string Computer { get; set; }
        string BackupLocation { get; set; }
        string CurrentBackupLocation { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
    }
}
