using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USMTUserCapture.Interfaces
{
    /// <summary>
    /// Folder information
    /// </summary>
    public interface IFolderInformation : INotifyPropertyChanged
    {
        /// <summary>
        /// This is where the 335thUserCapture executable is located at
        /// </summary>
        string BaseFolder {get; set;}

        /// <summary>
        /// Location of the USMT binaries. This is in relation to BaseFolder
        /// Please add BaseFolder and USMTBinaryFolder for full path
        /// </summary>
        string USMTBinaryFolder { get; set; }

        /// <summary>
        /// Location of the User Backup folder. This is relative
        /// Please add BaseFolder and UserBackupFolder for full path
        /// </summary>
        string UserBackupFolder { get; set; }

        /// <summary>
        /// Location of the Log folder. This is relative
        /// Please add BaseFolder and LogFilesFolder for full path
        /// </summary>
        string LogFilesFolder { get; set; }

        /// <summary>
        /// Validate BaseFolder exist and not a bad error
        /// </summary>
        bool IsBaseFolderValid { get; }

        /// <summary>
        /// Creates the UserBackupFolder
        /// </summary>
        void CreateUserBackupFolder();
    }
}
