using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.Interfaces
{
    public interface ILoadState
    {
        StreamReader StartRestore(IUserJob JobToRestore);

    }
}
