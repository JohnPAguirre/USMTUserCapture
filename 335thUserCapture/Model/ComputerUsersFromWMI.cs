using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _335thUserCapture.Interfaces;
using System.Management;

namespace _335thUserCapture.Model
{
    /// <summary>
    /// This only loads local users, will not work for our enviroment
    /// </summary>
    class ComputerUsersFromWMI : IUsersInfo
    {
        public List<string> _allUsers;

        public ComputerUsersFromWMI(){
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount where Domain='" + Environment.GetEnvironmentVariable("COMPUTERNAME") + "'");
            ManagementObjectCollection collection = searcher.Get();

            var items = new List<Win32_UserAccount>();
            foreach (ManagementObject obj in collection)
            {
                var item = new Win32_UserAccount();
                item.AccountType = (uint?)obj["AccountType"];
                item.Caption = (string)obj["Caption"];
                item.Description = (string)obj["Description"];
                item.Disabled = (bool?)obj["Disabled"];
                item.Domain = (string)obj["Domain"];
                item.FullName = (string)obj["FullName"];
                item.InstallDate = (DateTime?)obj["InstallDate"];
                item.LocalAccount = (bool?)obj["LocalAccount"];
                item.Lockout = (bool?)obj["Lockout"];
                item.Name = (string)obj["Name"];
                item.PasswordChangeable = (bool?)obj["PasswordChangeable"];
                item.PasswordExpires = (bool?)obj["PasswordExpires"];
                item.PasswordRequired = (bool?)obj["PasswordRequired"];
                item.SID = (string)obj["SID"];
                item.SIDType = (byte?)obj["SIDType"];
                item.Status = (string)obj["Status"];

                items.Add(item);
            }

            _allUsers = new List<string>();

            foreach (var obj in items)
            {
                if (obj.LocalAccount != null)
                {
                    _allUsers.Add(obj.Name);
                }
            }
        }
        public string[] AllUsers
        {
            get {
                if (_allUsers == null)
                    return null;
                string[] users = new string[_allUsers.Count];
                for (int i = 0; i < _allUsers.Count; i++) {
                    users[i] = _allUsers[i];
                }
                return users;

                //return _allUsers;

            }
        }
    }

    class Win32_UserAccount
    {
        public uint? AccountType;
        public string Caption;
        public string Description;
        public bool? Disabled;
        public string Domain;
        public string FullName;
        public DateTime? InstallDate;
        public bool? LocalAccount;
        public bool? Lockout;
        public string Name;
        public bool? PasswordChangeable;
        public bool? PasswordExpires;
        public bool? PasswordRequired;
        public string SID;
        public byte? SIDType;
        public string Status;
    }


}
