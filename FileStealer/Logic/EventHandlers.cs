using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class CountFilesEventArgs : EventArgs
    {
        public ulong Count { get; private set; }

        public CountFilesEventArgs(ulong count)
        {
            Count = count;
        }
    }

    public class DriveInformationEventArgs : EventArgs
    {
        public string Information { get; private set; }

        public DriveInformationEventArgs(string information)
        {
            this.Information = information;
        }
    }
}
