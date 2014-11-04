using System;
using _335thUserCapture.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Management;
using _335thUserCapture.Interfaces;
using _335thUserCapture.ViewModel.RestoreFromInteralStore;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;


namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ListAllUsers()
        {
            var users = new ComputerUsersFromFolders();
            var validUsers = new List<string>(Directory.EnumerateDirectories(@"C:\Users\"));
            for (int i = 0; i < users.Count; i++)
            {
                Debug.WriteLine(users.GetFullPath(i));
                Assert.AreEqual(users.GetFullPath(i), validUsers[i]);
            }
        }

        [TestMethod]
        public void ListAllUsersArray()
        {
            var users = new ComputerUsersFromFolders().AllUsers;
            var directory = new DirectoryInfo(@"C:\Users\")
                .GetDirectories();
            for (int i = 0; i < users.Length - 1; i++)
            {
                Debug.WriteLine(users[i]);
                Assert.AreEqual(users[i], directory[i].Name);
            }
        }

        /*[TestMethod]
        public void ScanStateOutputTest()
        {
            var scanstate = new ScanState("demetria.palmer", new BaseInformation());
            char[] buffer = new char[1];

            string output = "";
            while (scanstate.Output.EndOfStream == false)
            {
                scanstate.Output.ReadAsync(buffer, 0, 1);
                output = output + buffer[0];
            }
            Debug.WriteLine(output.Contains('\0'.ToString()));
            Debug.WriteLine(stripSlash0(output));
            Assert.AreNotEqual("", output);

        }*/

        [TestMethod]
        public void RestoreFromInteralStoreTest()
        {
            var backupInfo = new TestGetBckupInfo();
            var loadState = new TestLoadState();
            var ViewModel = new RestoreFromInternalStoreViewModel(backupInfo, loadState);
            //verify All Backup Jobs
            Assert.IsTrue(backupInfo.ListValid(ViewModel.AllBackupJobs));
            foreach (var job in ViewModel.AllBackupJobs)
            {
                Debug.WriteLine(job);
            }
            //Verify with SelectedBackupJob set to null that i cannot execute
            Assert.IsTrue(!ViewModel.StartBackup.CanExecute(null));
            //Verify CanExecute Event handler did its job
            bool canExecuteEventHandler = false;
            ViewModel.StartBackup.CanExecuteChanged += ((object a, EventArgs e) =>
            {
                canExecuteEventHandler = true;
            });
            //Very SelectedBackupJob
            ViewModel.SelectedBackupJob = ViewModel.AllBackupJobs[0];
            Assert.IsTrue(backupInfo.ValidateItem(ViewModel.SelectedBackupJob));
            //Verify StartBackup
            ViewModel.StartBackup.Execute(null);
            Assert.IsTrue(loadState.Touched);
            //Verify Output and Output Event Handler
            bool outputEventHandler = false;
            ViewModel.PropertyChanged += ((object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Output"){
                    outputEventHandler = true;
                }
            });
            string str = "This is a test";
            byte[] bytes = new byte[(str.Length * sizeof(char))];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            loadState.InternalStream.Write(bytes,0,bytes.Length);
            loadState.InternalStream.Position = 0;
            Thread.Sleep(100);
            Assert.IsTrue(canExecuteEventHandler);
            Assert.IsTrue(outputEventHandler);
            Assert.AreEqual(ViewModel.Output, str);
        }

        public string stripSlash0(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (!input.Substring(i, 1).Equals('\0'.ToString()))
                {
                    output += input.Substring(i, 1);
                }
            }
            return output;
        }

    }
        //old, delete 
        /*
        [TestMethod]
        public void TestUserGet()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
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

        }*/
    /* old delete
    public class Win32_UserAccount
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
    }*/
}
