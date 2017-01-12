using DroneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace DroneAPI.Processors.DroneProcessors {
    public class DroneCommandSender {
        private static DroneCommandSender instance;
        public static DroneCommandSender Instance
        {
            get
            {
                if (instance == null) {
                    instance = new DroneCommandSender();
                }
                return instance;
            }
        }
        private Socket _sender;
        private Queue<Command> _commands;

        private DroneCommandSender() { }

        public void SendData(Queue<Command> commands) {
            _commands = commands;

            // Connect to a remote device.
            try {
                connect();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void connect() {
            // Establish the remote endpoint for the socket.
            // This example uses port 11000 on the local computer.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.
            try {
                _sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}", _sender.RemoteEndPoint.ToString());

                // Sending the data
                send();

            } catch (ArgumentNullException ane) {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            } catch (SocketException se) {
                Console.WriteLine("SocketException : {0}", se.ToString());
            } catch (Exception e) {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        private void send() {
            // Encode the data string into a byte array.
            string msgJson = new JavaScriptSerializer().Serialize(_commands.ToList<Command>());
            Console.WriteLine(msgJson);
            byte[] msg = Encoding.ASCII.GetBytes(msgJson + "<EOF>");

            // Send the data through the socket.
            int bytesSent = _sender.Send(msg);

            // Data buffer for incoming data.
            byte[] bytes = new byte[1048576];

            // Receive the response from the remote device.
            int bytesRec = _sender.Receive(bytes);

            Console.WriteLine("Server message: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

            disconnect();
        }

        private void disconnect() {
            // Release the socket.
            _sender.Shutdown(SocketShutdown.Both);
            _sender.Close();
        }
    }
}