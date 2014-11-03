using _335thUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.Model
{
 public class BackupJob : IUserJob{

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

        private string _currentBackupLocation;
        public string CurrentBackupLocation
        {
            get
            {
                return _currentBackupLocation;
            }
            set
            {
                _currentBackupLocation = value;
            }
        }
    }
