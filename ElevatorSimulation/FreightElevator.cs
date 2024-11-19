using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class FreightElevator : Elevator
    {
        public override void Move(int targetFloor)
        {
            Console.WriteLine("Freight elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
            CurrentFloor = targetFloor;
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"Freight Elevator, Current Floor: {CurrentFloor}");
        }
    }

}
