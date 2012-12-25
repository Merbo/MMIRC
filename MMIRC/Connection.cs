using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace MMIRC
{
    public class Connection
    {
        //Private stuff
        private TcpClient IRC;
        private Stream IRCStream;
        private StreamReader Reader;
        private StreamWriter Writer;

        //Info about the connection
        public readonly string Server;
        public readonly int Port;
        public readonly bool SSL;
        public readonly string HumanReadableName;

        //Events
        public static event EventHandler<ConnectionAddedEventArgs> ConnectionAdded;
        public event EventHandler<RawReceivedEventArgs> RawDataReceived;
        public event EventHandler<PrivmsgReceivedEventArgs> PrivmsgReceived;
        public event EventHandler<NoticeReceivedEventArgs> NoticeReceived;
        public event EventHandler<ModeReceivedEventArgs> ModeReceived;
        public event EventHandler<JoinReceivedEventArgs> JoinReceived;
        public event EventHandler<PartReceivedEventArgs> PartReceived;
        public event EventHandler<QuitReceivedEventArgs> QuitReceived;

        /// <summary>
        /// Prepares a connection
        /// </summary>
        /// <param name="server">The server to connect on</param>
        /// <param name="port">The port to connect on</param>
        /// <param name="ssl">Do we want SSL?</param>
        /// <param name="hrn">A human readable string for the network</param>
        public Connection(string server, int port, bool ssl, string hrn)
        {
            this.Server = server;
            this.Port = port;
            this.SSL = ssl;
            this.HumanReadableName = hrn;
        }

        /// <summary>
        /// Connects to the network
        /// </summary>
        public void Connect()
        {
            IRC = new TcpClient(Server, Port);
            if (SSL)
            {
                IRCStream = new SslStream(IRC.GetStream(), false,
                    new RemoteCertificateValidationCallback(ValidateServerCert),
                    false ? new LocalCertificateSelectionCallback(SelectLocalCert) : null);
                ((SslStream)IRCStream).AuthenticateAsClient(Server);
            }
            else
                IRCStream = IRC.GetStream();

            Reader = new StreamReader(IRCStream);
            Writer = new StreamWriter(IRCStream);

            StartRead();

            EventHandler<ConnectionAddedEventArgs> Handler = ConnectionAdded;

            if (Handler != null)
                Handler(this, new ConnectionAddedEventArgs());
        }

        public bool ValidateServerCert(object sender, X509Certificate Cert, X509Chain Chain, SslPolicyErrors Errors)
        {
            return true;
        }

        public X509Certificate SelectLocalCert(object sender, string name, X509CertificateCollection Collection, X509Certificate Cert, string[] names)
        {
            return null;
        }

        /// <summary>
        /// Begins reading from IRC
        /// </summary>
        private void StartRead()
        {
            new Thread(() =>
                {
                    while (true)
                    {
                        if (Reader.Peek() != 0)
                        {
                            string Input = Reader.ReadLine();
                            string[] Split = Input.Split(' ');

                            EventHandler<RawReceivedEventArgs> RawHandler = RawDataReceived;
                            if (RawHandler != null)
                                RawHandler(this, new RawReceivedEventArgs(Input, Split));


                            if (Split.Length > 3)
                            {
                                if (Split[1] == "PRIVMSG")
                                {
                                    string[] NickSplit = Split[0].Split('!');
                                    string[] HostSplit = Split[0].Split('@');

                                    EventHandler<PrivmsgReceivedEventArgs> Handler = PrivmsgReceived;
                                    if (Handler != null)
                                        Handler(this, new PrivmsgReceivedEventArgs(
                                            string.Join(" ", Split, 3, Split.Length - 3).Remove(0, 1),
                                            NickSplit[0].Remove(0, 1),
                                            NickSplit[1].Replace(HostSplit[1], "").Replace("@", ""),
                                            HostSplit[1],
                                            Split[2]));

                                }
                                else if (Split[1] == "NOTICE")
                                {
                                    string[] NickSplit = Split[0].Split('!');
                                    string[] HostSplit = Split[0].Split('@');

                                    EventHandler<NoticeReceivedEventArgs> Handler = NoticeReceived;
                                    if (Handler != null)
                                        Handler(this, new NoticeReceivedEventArgs(
                                            string.Join(" ", Split, 3, Split.Length - 3).Remove(0, 1),
                                            NickSplit[0].Remove(0, 1),
                                            NickSplit[1].Replace(HostSplit[1], "").Replace("@", ""),
                                            HostSplit[1],
                                            Split[2]));
                                }
                                else if (Split[1] == "MODE")
                                {
                                    string[] NickSplit = Split[0].Split('!');
                                    string[] HostSplit = Split[0].Split('@');

                                    EventHandler<ModeReceivedEventArgs> Handler = ModeReceived;
                                    if (Handler != null)
                                        Handler(this, new ModeReceivedEventArgs(
                                            string.Join(" ", Split, 3, Split.Length - 3).Remove(0, 1),
                                            NickSplit[0].Remove(0, 1),
                                            NickSplit[1].Replace(HostSplit[1], "").Replace("@", ""),
                                            HostSplit[1],
                                            Split[2]));
                                }
                                else if (Split[1] == "JOIN")
                                {
                                    string[] NickSplit = Split[0].Split('!');
                                    string[] HostSplit = Split[0].Split('@');

                                    EventHandler<JoinReceivedEventArgs> Handler = JoinReceived;
                                    if (Handler != null)
                                        Handler(this, new JoinReceivedEventArgs(
                                            NickSplit[0].Remove(0, 1),
                                            NickSplit[1].Replace(HostSplit[1], "").Replace("@", ""),
                                            HostSplit[1],
                                            Split[2]));
                                }
                                else if (Split[1] == "PART")
                                {
                                    string[] NickSplit = Split[0].Split('!');
                                    string[] HostSplit = Split[0].Split('@');

                                    EventHandler<PartReceivedEventArgs> Handler = PartReceived;
                                    if (Handler != null)
                                        Handler(this, new PartReceivedEventArgs(
                                            NickSplit[0].Remove(0, 1),
                                            NickSplit[1].Replace(HostSplit[1], "").Replace("@", ""),
                                            HostSplit[1],
                                            Split[2],
                                            string.Join(" ", Split, 3, Split.Length - 3).Remove(0, 1)));
                                }
                                else if (Split[1] == "QUIT")
                                {
                                    string[] NickSplit = Split[0].Split('!');
                                    string[] HostSplit = Split[0].Split('@');

                                    EventHandler<QuitReceivedEventArgs> Handler = QuitReceived;
                                    if (Handler != null)
                                        Handler(this, new QuitReceivedEventArgs(
                                            NickSplit[0].Remove(0, 1),
                                            NickSplit[1].Replace(HostSplit[1], "").Replace("@", ""),
                                            HostSplit[1],
                                            string.Join(" ", Split, 3, Split.Length - 3).Remove(0, 1)));
                                }
                            }
                        }
                        else
                            break;
                    }

                }).Start();
        }

        /// <summary>
        /// Write to IRC
        /// </summary>
        /// <param name="Data">The data to write</param>
        public void WriteLine(string Data)
        {
            Writer.WriteLine(Data);
            Writer.Flush();
        }
    }

    public class ConnectionAddedEventArgs : EventArgs
    {
        public ConnectionAddedEventArgs()
        {
        }
    }
}
