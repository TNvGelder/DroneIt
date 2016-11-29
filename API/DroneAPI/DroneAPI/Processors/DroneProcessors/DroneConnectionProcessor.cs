using System;
using Quobject.SocketIoClientDotNet.Client;
using DroneAPI.Models;

/*
* @author : harmen hilvers
*/
namespace DroneAPI.Processors.DroneProcessors
{
    public class DroneConnectionProcessor
    {
        public Drone Drone { get; private set; }
        private Socket _socket { get; set; }

        public DroneConnectionProcessor(Drone drone)
        {
            Drone = drone;
            Drone.Busy = false;
        }

        public void Connect()
        {
            if(_socket == null)
            {
                _socket = IO.Socket(Drone.NodeJsIp);
            }
            else
            {
                _socket.Connect();
            }

            _socket.On("done", () => {
                Console.WriteLine(Drone.Name + ", is done!");
                Drone.Busy = false;
            });
        }

        public void sendData(string action, string data = "")
        {
            Drone.Busy = true;
            string[] arrayData = { action, data };
            _socket.Emit("drone", arrayData);
        }     

        public void Disconnect()
        {
            if (_socket == null)
            {
                _socket.Close();
            }
        }
    }
}