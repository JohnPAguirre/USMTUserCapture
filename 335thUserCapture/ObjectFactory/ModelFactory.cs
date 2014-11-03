using _335thUserCapture.Interfaces;
using _335thUserCapture.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.ObjectFactory
{
    public class ModelFactory
    {
        //BaseInformation implements both IFolderInformation and IUserSelected and will pass that info along to all revelant viewcontainers
        private static BaseInformation _BaseInformation;
        private static IFolderInformation _baseFolders;
        private static IUserSelected _userSelected;
        private static IUsersInfo _computerUsers;
        private static BackupDatabaseSQLite _db;
        private static ILoadState _loadState;



        static public IFolderInformation CreateBaseLocationsFolder
        {
            get
            {
                if (_baseFolders == null)
                    _baseFolders = ModelFactory.CreateBaseInformation;
                return _baseFolders;
            }
        }

        static public IUsersInfo CreateUserList
        {
            get
            {
                if (_computerUsers == null)
                    _computerUsers = new ComputerUsersFromFolders();
                return _computerUsers;
            }
        }

        static public IUserSelected CreateUserSelected
        {
            get
            {
                if (_userSelected == null)
                    _userSelected = ModelFactory.CreateBaseInformation;
                return _userSelected;
            }
        }

        static public BaseInformation CreateBaseInformation
        {
            get
            {
                if (_BaseInformation == null)
                    _BaseInformation = new BaseInformation();
                return _BaseInformation;
            }
        }

        static public ISaveBackupInformation CreatWriteDB
        {
            get
            {
                if (_db == null)
                    _db = new BackupDatabaseSQLite(ModelFactory.CreateBaseLocationsFolder);
                return _db;
            }
        }

        static public IGetBackupInformation CreateReadDB
        {
            get
            {
                if (_db == null)
                    _db = new BackupDatabaseSQLite(ModelFactory.CreateBaseLocationsFolder);
                return _db;
            }
        }

        public static ILoadState CreateLoadState
        {
            get
            {
                if (_loadState==null)
                {
                    _loadState = new LoadState(ModelFactory.CreateBaseLocationsFolder);
                }
                return _loadState;

            }
        }
    }
}