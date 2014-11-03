using _335thUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _335thUserCapture.Model
{
    /// <summary>
    /// Runs scanstate on the computer
    /// </summary>
    public class ScanState : IDisposable
    {
        //process information
        Process _scanState;
        //output stream
        StreamReader _output;

        //internal variable to signal when output is ready
        EventWaitHandle _isReady;

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
            string arguments = String.Format("{0} /i:\"{1}migapp.xml\" /i:\"{1}miguser.xml\" /ue:*\\* /ui:*\\{2} /c /efs:copyraw", 
                folders.BaseFolder + folders.UserBackupFolder, 
                folders.BaseFolder + folders.USMTBinaryFolder,
                user);
            _scanState.StartInfo.Arguments = arguments;
            //We have to start USMT in a different thread or it locks up the GUI.  To do this we have this following method await the process
            this.StartBackup();

        }

        /// <summary>
        /// This method should be executed in a different thread.  Please only call this from the StartProcess Thread
        /// TODO: Possibly refactor
        /// </summary>
        private async void StartBackup()
        {
            await Task.Run((Func<bool>)_scanState.Start);
            _output = _scanState.StandardOutput;
            _isReady.Set();
        }

        /// <summary>
        /// This funciton will wait until _output is set to something and then proceed
        /// </summary>
        public async Task Ready()
        {
            await Task.Run((Func<bool>)_isReady.WaitOne);
        }

        public StreamReader Output
        {
            get
            {
                if (_output == null)
                    throw new InvalidOperationException("Please wait until Ready function is complete before reading this variable");
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



        //& "C:\Program Files\USMT\Binaries\v4\x86\scanstate.exe" I:\$back_user /i:"C:\Program Files\USMT\Binaries\v4\x86\miguser.xml" /i:"C:\Program Files\USMT\Binaries\v4\x86\migapp.xml" /ue:*\* /ui:"ar\$back_user" /c /efs:copyraw

        public void Dispose()
        {
            _scanState.Close();
        }
    }
}
