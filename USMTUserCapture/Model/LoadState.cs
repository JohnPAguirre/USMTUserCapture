using USMTUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USMTUserCapture.Model
{
    /// <summary>
    /// Implements LoadState as the restore mechanism for the application
    /// </summary>
    public class LoadState : ILoadState
    {
        private IFolderInformation _folders;
        private Process _loadState;

        public LoadState(IFolderInformation folders)
        {
            _folders = folders;
        }


        public StreamReader StartRestore(IUserJob JobToRestore)
        {
            _loadState = new Process();
            _loadState.StartInfo.UseShellExecute = false;
            _loadState.StartInfo.CreateNoWindow = true;
            _loadState.StartInfo.RedirectStandardOutput = true;
            _loadState.StartInfo.StandardOutputEncoding = Encoding.Unicode;
            _loadState.StartInfo.WorkingDirectory = _folders.BaseFolder + _folders.USMTBinaryFolder;
            _loadState.StartInfo.FileName = _folders.BaseFolder + _folders.USMTBinaryFolder + "loadstate.exe";
            string arguments = String.Format("{0} /l:\"{0}loadstate.log\" /progress:\"{0}LoadStateProgress.log\" " +
                "/i:\"{1}migapp.xml\" /i:\"{1}miguser.xml\" /c",
                _folders.BaseFolder + JobToRestore.BackupLocation,
                _folders.BaseFolder + _folders.USMTBinaryFolder);
            _loadState.StartInfo.Arguments = arguments;
            _loadState.Start();
            return _loadState.StandardOutput;
        }
    }
}
