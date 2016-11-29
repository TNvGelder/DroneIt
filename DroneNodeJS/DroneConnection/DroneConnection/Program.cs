using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneConnection {
    class Program {
        static void Main(string[] args) {
            Drone drone = new Drone("Thomas", "http://127.0.0.1:8000/");
            /*drone.Connect();
            drone.Turn(90);*/
            /*drone.Forward(2);
            drone.Backward(2);
            drone.Turn(270);
            drone.Turn(190);*/
            
            DroneConnectionProcessor dconp = new DroneConnectionProcessor(drone);
            DroneProcessor dp = new DroneProcessor(dconp);
            DroneCommandProcessor dcomp = new DroneCommandProcessor(dp);

            dcomp.AddCommand(new StartCommand(dp));
            dcomp.AddCommand(new StartCommand(dp));
            dcomp.AddCommand(new StartCommand(dp));
            dcomp.AddCommand(new StartCommand(dp));
            dcomp.AddCommand(new StartCommand(dp));
            dcomp.AddCommand(new StartCommand(dp));

            dcomp.Execute();

            Console.Read();
        }
    }
}
