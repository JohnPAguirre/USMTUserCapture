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
        /// Used to annotate the backup job is started
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

    /// <summary>
    /// All backups are returned
    /// </summary>
    public interface IGetBackupInformation
    {
        /// <summary>
        /// return all backups
        /// </summary>
        /// <returns>All performed backups in order from newest to oldest</returns>
        List<IUserJob> AllBackups();
    }

    /// <summary>
    /// A user job
    /// </summary>
    public interface IUserJob
    {
        /// <summary>
        /// ID as annotated in the Database
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// User name. If no user is specified this value should be set to *
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Computer name
        /// </summary>
        string Computer { get; set; }

        /// <summary>
        /// Backup location in relation with the 335thUserCapture Executable.  This 
        /// does not store the full path!!
        /// </summary>
        string BackupLocation { get; set; }

        /// <summary>
        /// Full qualified path.  Please generate this by adding the path of the 335th executable and backuplocation
        /// </summary>
        string CurrentBackupLocation { get; set; }

        /// <summary>
        /// DateTime backup was started
        /// </summary>
        DateTime Start { get; set; }

        /// <summary>
        /// DateTime backup was completed
        /// </summary>
        DateTime End { get; set; }
    }
}
