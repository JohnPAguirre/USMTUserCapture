using USMTUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace USMTUserCapture.Model
{
    /// <summary>
    /// Runs scanstate on the computer
    /// </summary>
    public class ScanState : IScanState, IDisposable
    {
        //process information
        private Process _scanState;
        //output stream
        private StreamReader _output;

        //internal variable to signal when output is ready
        private EventWaitHandle _isReady;

        /// <summary>
        /// Builds this and starts the process
        /// </summary>
        /// <param name="user">User to backup</param>
        /// <param name="folders">Holds the important folders that are required</param>
        public ScanState(string user, IFolderInformation folders)
        {
            _isReady = new EventWaitHandle(false, EventResetMode.ManualReset);
            

            _scanState = new Process();
            _scanState.StartInfo.UseShellExecute = false;
            _scanState.StartInfo.CreateNoWindow = true;
            _scanState.StartInfo.RedirectStandardOutput = true;
            _scanState.StartInfo.StandardOutputEncoding = Encoding.Unicode;
            //_scanState.StartInfo.Verb = "runas";
            _scanState.StartInfo.WorkingDirectory = folders.BaseFolder + folders.USMTBinaryFolder;
            _scanState.StartInfo.FileName = folders.BaseFolder + folders.USMTBinaryFolder + "scanstate.exe";
            string arguments = String.Format("{0} /l:\"{0}scanstate.log\" /progress:\"{0}ScanStateProgress.log\" " + 
                "/i:\"{1}migapp.xml\" /i:\"{1}miguser.xml\" " + 
                "/ue:*\\* /ui:*\\{2} /c /efs:copyraw", 
                folders.BaseFolder + folders.UserBackupFolder, 
                folders.BaseFolder + folders.USMTBinaryFolder,
                user);
            _scanState.StartInfo.Arguments = arguments;
            Task.Run(() =>
            {
                _scanState.Start();
                _isReady.Set();
            });
            _isReady.WaitOne();
            _output = _scanState.StandardOutput;
        }

        public StreamReader Output
        {
            get
            {
                if (_output == null)
                    throw new ApplicationException("For some reason USMT failed ot start");
                return _output;
            }
        }

        public int Progress
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        //Original String called from backup script created 2012/10/22
        //& "C:\Program Files\USMT\Binaries\v4\x86\scanstate.exe" I:\$back_user /i:"C:\Program Files\USMT\Binaries\v4\x86\miguser.xml" /i:"C:\Program Files\USMT\Binaries\v4\x86\migapp.xml" /ue:*\* /ui:"ar\$back_user" /c /efs:copyraw

        public void Dispose()
        {
            _scanState.Close();
        }
    }
}
