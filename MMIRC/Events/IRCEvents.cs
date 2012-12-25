using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMIRC
{
    public class RawReceivedEventArgs : EventArgs
    {
        public readonly string Data;
        public readonly string[] Split;

        public RawReceivedEventArgs(string s, string[] ss)
        {
            this.Data = s;
            this.Split = ss;
        }
    }

    public class PrivmsgReceivedEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly string Nick, User, Host;
        public readonly string Target;

        public PrivmsgReceivedEventArgs(string msg, string nick, string usr, string host, string tgt)
        {
            this.Message = msg;
            this.Nick = nick;
            this.User = usr;
            this.Host = host;
            this.Target = tgt;
        }
    }

    public class NoticeReceivedEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly string Nick, User, Host;
        public readonly string Target;

        public NoticeReceivedEventArgs(string msg, string nick, string usr, string host, string tgt)
        {
            this.Message = msg;
            this.Nick = nick;
            this.User = usr;
            this.Host = host;
            this.Target = tgt;
        }
    }

    public class ModeReceivedEventArgs : EventArgs
    {
        public readonly string Mode;
        public readonly string Nick, User, Host;
        public readonly string Target;

        public ModeReceivedEventArgs(string m, string nick, string usr, string host, string tgt)
        {
            this.Mode = m;
            this.Nick = nick;
            this.User = usr;
            this.Host = host;
            this.Target = tgt;
        }
    }

    public class JoinReceivedEventArgs : EventArgs
    {
        public readonly string Nick, User, Host;
        public readonly string Target;

        public JoinReceivedEventArgs(string nick, string usr, string host, string tgt)
        {
            this.Nick = nick;
            this.User = usr;
            this.Host = host;
            this.Target = tgt;
        }
    }

    public class PartReceivedEventArgs : EventArgs
    {
        public readonly string PartMsg;
        public readonly string Nick, User, Host;
        public readonly string Target;

        public PartReceivedEventArgs(string nick, string usr, string host, string tgt, string m = null)
        {
            this.PartMsg = m;
            this.Nick = nick;
            this.User = usr;
            this.Host = host;
            this.Target = tgt;
        }
    }

    public class QuitReceivedEventArgs : EventArgs
    {
        public readonly string QuitMsg;
        public readonly string Nick, User, Host;

        public QuitReceivedEventArgs(string nick, string usr, string host, string m = null)
        {
            this.QuitMsg = m;
            this.Nick = nick;
            this.User = usr;
            this.Host = host;
        }
    }
}
