using System;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading.Tasks;

namespace DroneConnection {
    public class Drone {
        public string Name { get; set; }
        private int _degrees { get; set; }
        public string NodeJsIp { get; set; }
        private Socket _socket { get; set; }
        public bool busy { get; set; }
        public bool Flying { get; set; }

        public Drone(string name, string connectionString) {
            Name = name;
            NodeJsIp = connectionString;
            busy = false;
        }

        public void Connect() {
            if (_socket == null) {
                _socket = IO.Socket(NodeJsIp);
            } else {
                _socket.Connect();
            }
            Start();
            Console.WriteLine("Starting drone wait 30 sec");
            System.Threading.Thread.Sleep(30000);
            /*busy = true;
            waitForDrone();*/

            _socket.On("done", () => {
                Console.WriteLine(Name + ", is done!");
                busy = false;
            });
        }

        private void sendData(string action, string data = "") {
            busy = true;
            string[] arrayData = { action, data };
            _socket.Emit("drone", arrayData);
            waitForDrone();
        }

        private async void waitForDrone() {
            while (busy) {
                await Task.Delay(25);
            }
        }

        public void Disconnect() {
            if (_socket == null) {
                _socket.Close();
            }
        }

        public void Turn(int direction) {
            //direction = direction > 180 ? direction - 360 : direction;
            sendData("turn", direction.ToString());
        }

        public void Forward(int fields) {
            sendData("forward", fields.ToString());
        }

        public void Backward(int fields) {
            sendData("backward", fields.ToString());
        }

        public void Left(int fields) {
            sendData("left", fields.ToString());
        }

        public void Right(int fields) {
            sendData("right", fields.ToString());
        }

        public void Rise(int meters) {
            sendData("rise", meters.ToString());
        }

        public void Fall(int meters) {
            sendData("fall", meters.ToString());
        }

        public void Start() {
            sendData("start");
        }

        public void Land() {
            sendData("land");
        }
    }
}
