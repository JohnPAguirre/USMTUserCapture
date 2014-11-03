using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.Interfaces
{
    public interface IUserSelected : INotifyPropertyChanged
    {
        string SelectedUser { get; set; }
        bool IsUserValid { get; set; }
    }
}
