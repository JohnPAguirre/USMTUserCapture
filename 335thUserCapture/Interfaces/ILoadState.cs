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
        /// <summary>
        /// Restores one user while returning a stream of output
        /// </summary>
        /// <param name="JobToRestore">User to restore</param>
        /// <returns>Output of the tool used</returns>
        StreamReader StartRestore(IUserJob JobToRestore);

    }
}
