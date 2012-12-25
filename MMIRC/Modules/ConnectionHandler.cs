using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMIRC
{
    public class ConnectionHandler : Module
    {
        public override int Init()
        {
            Connection.ConnectionAdded += ConnectionAdded;
            return MODULE_OKAY;
        }

        public override int OnTick(int Tick)
        {
            return MODULE_OKAY;
        }

        private void ConnectionAdded(object sender, ConnectionAddedEventArgs e)
        {
            Connection C = (Connection)sender;
            C.RawDataReceived += RawReceived;

            C.WriteLine("NICK " + Environment.UserName);
            C.WriteLine("USER " + Environment.UserName + " 0 * :MerbosMagic IRC Client");
        }

        private void RawReceived(object sender, RawReceivedEventArgs e)
        {
            if (e.Split.Length > 1)
                if (e.Split[0] == "PING")
                    ((Connection)sender).WriteLine("PONG " + e.Split[1]);
        }

        public override string Name()
        {
            return "ConnectionHandler";
        }

        public override string Author()
        {
            return "Merbo";
        }

        public override string Description()
        {
            return "Allows the starting of connections and responses to pings";
        }

        public override string Version()
        {
            return "1.0.0";
        }
    }
}
