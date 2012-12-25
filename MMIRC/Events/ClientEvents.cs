using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMIRC
{
    public class JoinEventArgs : EventArgs
    {
        public readonly string Channel;

        public JoinEventArgs(string chan)
        {
            Channel = chan;
        }
    }

    public class PartEventArgs : EventArgs
    {
        public readonly string Channel;

        public PartEventArgs(string chan)
        {
            Channel = chan;
        }
    }
}
