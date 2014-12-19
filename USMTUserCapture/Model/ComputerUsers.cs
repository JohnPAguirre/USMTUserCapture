using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using USMTUserCapture.Interfaces;

namespace USMTUserCapture.Model
{
    /// <summary>
    /// List all current computer proviles listed under C:\Users.  Allows for iteration and current folder size
    /// </summary>
    public class ComputerUsersFromFolders : IUsersInfo
    {
        private List<DirectoryInfo> _test;

        /// <summary>
        /// Enumarates all users in the user directory
        /// </summary>
        public ComputerUsersFromFolders()
        {
            _test = new List<DirectoryInfo>(
                (
                new DirectoryInfo(@"C:\Users")
                ).EnumerateDirectories()
                );

        }

        /// <summary>
        /// returns the full path of the user
        /// </summary>
        /// <param name="key">Must be below the Count parameter</param>
        /// <returns>Full Path of the user folder</returns>
        public string GetFullPath(int key)
        {
            if (key + 1 > _test.Count || key < 0)
                throw new IndexOutOfRangeException();
            return _test[key].FullName;
        }

        /// <summary>
        /// returns the name of the user
        /// </summary>
        /// <param name="key">Must be below the Count parameter</param>
        /// <returns>name of the user folder</returns>
        public string GetUserName(int key)
        {
            if (key + 1 > _test.Count || key < 0)
                throw new IndexOutOfRangeException();
            return _test[key].Name;
        }

        /// <summary>
        /// How many entries there are in the user directory
        /// </summary>
        public int Count
        {
            get
            {
                return _test.Count;
            }
        }

        public string[] AllUsers
        {
            get
            {
                var users = new string[_test.Count];
                for (int i = 0; i <= _test.Count-1; i++)
                {
                    users[i] = _test[i].Name;
                }
                return users;
            }
        }
    }
}
