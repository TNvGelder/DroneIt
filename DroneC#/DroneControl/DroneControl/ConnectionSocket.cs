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

/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl
{
    /// <summary>
    /// Bind a socket and wait for connection
    /// </summary>
    public class ConnectionSocket
    {
        private static volatile ConnectionSocket _instance;
        private static object syncRoot = new Object();
        private int _port { get; set; }
        private Thread th { get; set; }

        /// <summary>
        /// Set port and makes the thread
        /// </summary>
        private ConnectionSocket() {
            _port = 11000;
            th = new Thread(StartListening);
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

        /// <summary>
        /// Start the thread and start listening on the port
        /// </summary>
        public void Start() {
            th.Start();
        }

        private void StartListening() {
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
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

                    // An incoming connection needs to be processed.
                    string data = receiveData(handler);

                    // Show the data on the console.
                    Console.WriteLine("Data received : {0}", data.Substring(0, data.Length - 5));

                    List<Command> commandList = stringToCommands(data);
                    executeCommands(commandList);
                    
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            
            Console.Read();
        }

        /// <summary>
        /// Receiving data from the socket recusief
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        /// <returns>string</returns>
        private string receiveData(Socket handler, string data = null) {
            byte[] bytes = new byte[1048576];
            int bytesRec = handler.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            if (data.IndexOf("<EOF>") > -1) {
                return data;
            }
            return receiveData(handler, data);
        }

        /// <summary>
        /// String to a list with commands
        /// </summary>
        /// <param name="data"></param>
        /// <returns>List<Command></returns>
        private List<Command> stringToCommands(string data) {
            return new JavaScriptSerializer().Deserialize<List<Command>>(data.Substring(0, data.Length - 5));
        }

        /// <summary>
        /// Makes command factory and drone command processor
        /// Adds commands to the drone command processor
        /// And execute commands
        /// </summary>
        /// <param name="commandList"></param>
        private void executeCommands(List<Command> commandList) {
            CommandFactory commandFactory = new CommandFactory(DroneController.Instance);
            DroneCommandProcessor droneCommandProcessor = new DroneCommandProcessor();

            foreach (Command c in commandList) {
                droneCommandProcessor.AddCommand(commandFactory.makeCommand(c.name, c.value));
            }
            droneCommandProcessor.Execute();
            droneCommandProcessor.Undo();

            ApiConnection.Instance.UpdateQualityCheck("Done");
            Sound.Instance.Harmen();
            Console.WriteLine("Done");
        }
    }
}
