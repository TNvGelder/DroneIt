using AR.Drone.Avionics;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace DroneControl {
    class Program {
        static void Main(string[] args) {
            //ConnectionSocket cs = ConnectionSocket.Instance;

            DroneController dc = new DroneController();
            dc.Start();

            Console.Read();
        }

        //DroneController ctrl = new DroneController();
        //ctrl.Start(1000);
        //
        //ctrl.Turn(20);
        //
        //ctrl.Stop(1000);
            
    }
}
