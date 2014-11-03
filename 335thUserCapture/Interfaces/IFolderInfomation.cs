using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.Interfaces
{
    public interface IFolderInformation : INotifyPropertyChanged
    {
        string BaseFolder {get; set;}
        string USMTBinaryFolder { get; set; }
        string UserBackupFolder { get; set; }
        string LogFilesFolder { get; set; }

        bool IsBaseFolderValid { get; set; }
        void CreateUserBackupFolder();
    }
}
