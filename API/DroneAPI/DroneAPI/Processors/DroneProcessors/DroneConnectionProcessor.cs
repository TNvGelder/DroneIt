using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quobject.SocketIoClientDotNet.Client;
using DroneAPI.Models;

/*
* @author : harmen hilvers
*/
namespace DroneAPI.Processors.DroneProcessors
{
    public class DroneConnectionProcessor
    {
        private Drone _drone { get; set; }
        private Socket _socket { get; set; }


        public DroneConnectionProcessor(Drone drone)
        {
            _drone = drone;
            _drone.busy = false;
        }

        public void Connect()
        {
            _drone.busy = true;
            if(_socket == null)
            {
                _socket = IO.Socket(_drone.NodeJsIp);
            }
            else
            {
                _socket.Connect();
            }

            _socket.On("done", () => {
                Console.WriteLine(_drone.Name + ", is done!");
                _drone.busy = false;
            });
        }

        public void sendData(string action, string data = "")
        {
            if (_drone.busy) return;

            _drone.busy = true;
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