using DroneControl.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DroneControl
{
    class ConnectionSocket
    {
        private static volatile ConnectionSocket _instance;
        private static object syncRoot = new Object();
        private int _port { get; set; }
        private Thread th { get; set; }

        private ConnectionSocket() {
            _port = 11000;
            th = new Thread(StartListening);
            th.Start();
        }

        public static ConnectionSocket Instance {
            get {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null)
                            _instance = new ConnectionSocket();
                    }
                }
                return _instance;
            }
        }

        public void StartListening() {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1048576];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _port);

            Console.WriteLine(Dns.GetHostName());

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.
                while (true) {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    string data = null;

                    // An incoming connection needs to be processed.
                    while (true) {
                        bytes = new byte[1048576];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1) {
                            break;
                        }
                    }

                    // Show the data on the console.
                    Console.WriteLine("Text received : {0}", data);
                    Console.WriteLine(data.Substring(0, data.Length - 5));

                    List<Command> commandList = new JavaScriptSerializer().Deserialize<List<Command>>(data.Substring(0, data.Length - 5));
                    Queue<IDroneCommand> commandQueue = new Queue<IDroneCommand>();
                    CommandFactory commandFactory = new CommandFactory(null);

                    foreach (Command c in commandList) {
                        commandQueue.Enqueue(commandFactory.makeCommand(c.name, c.value));
                    }

                    // Echo the data back to the client.
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}
