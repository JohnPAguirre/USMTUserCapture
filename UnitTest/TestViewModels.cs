using Microsoft.VisualStudio.TestTools.UnitTesting;
using _335thUserCapture.ViewModel.RestoreFromInteralStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _335thUserCapture.Interfaces;
using System.IO;

namespace UnitTest
{
    [TestClass]
    class TestViewModels
    {
        [TestMethod]
        public void RestoreFromInteralStoreTest()
        {
            var backupInfo = new TestGetBckupInfo();
            var loadState = new TestLoadState();
            var ViewModel = new RestoreFromInternalStoreViewModel(backupInfo,loadState);
            Assert.IsTrue(backupInfo.ListValid(ViewModel.AllBackupJobs));

        }
    }

    class TestLoadState : ILoadState
    {
        public IUserJob PassedUserJob;
        public bool Touched = false;
        public MemoryStream InternalStream;

        public StreamReader StartRestore(IUserJob JobToRestore)
        {
 	        this.PassedUserJob = JobToRestore;
            this.Touched = true;
            InternalStream = new MemoryStream();
            var InternalReader = new StreamReader(InternalStream);

            return InternalReader;
        }
    }

    class TestGetBckupInfo : IGetBackupInformation
    {
        private List<IUserJob> _results;

        public TestGetBckupInfo(){
            _results = new List<IUserJob>();
            var temp = new TestUserJob
            {
                ID = 1,
                User = "john.p.aguirre",
                Computer = "ZGA011NBAAAA",
                BackupLocation = @"\\55.139.192.240\Backup\blah\blah\blah",
                Start = DateTime.Parse("10/25/2014 10:53PM")
            };
            _results.Add(temp);
            temp = new TestUserJob
            {
                ID = 2,
                User = "Billy.Bob",
                Computer = "ZGA011NBBBBB",
                BackupLocation = @"\\55.139.192.240\Backup\blah\blah\blah",
                Start = DateTime.Parse("10/25/2014 11:42PM")
            };
            _results.Add(temp);
            
        }

        public List<IUserJob> AllBackups()
        {
            return _results;
        }

        public bool ListValid(List<IUserJob> toValidate)
        {
           return object.Equals(_results,toValidate);
        }

        public bool ValidateItem(IUserJob toValidate)
        {
            bool results = false;
            for (int i = 0; i < _results.Count; i++){
                if (object.Equals(_results[i],toValidate))
                    results = true;
            }
            return results;
        }
    }

    class TestUserJob : IUserJob
    {
        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _user;
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        private string _computer;
        public string Computer
        {
            get
            {
                return _computer;
            }
            set
            {
                _computer = value;
            }
        }

        private string _backupLocation;
        public string BackupLocation
        {
            get
            {
                return _backupLocation;
            }
            set
            {
                _backupLocation = value;
            }
        }

        private DateTime _start;
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }

        private DateTime _end;
        public DateTime End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
            }
        }

        public override string ToString()
        {
            return "User:" + this.User + "\r\nComputer:" + this.Computer + "\r\nStart time:" + Start.ToLocalTime();
        }


        public string CurrentBackupLocation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

    
}
