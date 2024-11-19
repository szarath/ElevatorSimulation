using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class HighSpeedElevator : Elevator
    {
        public int MaxSpeed { get; set; } = 100;  // Maximum speed in floors per second

        public override void Move(int targetFloor)
        {
            Console.WriteLine("High-speed elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
            CurrentFloor = targetFloor;
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"High-Speed Elevator, Max Speed: {MaxSpeed} floors/sec, Current Floor: {CurrentFloor}");
        }
    }

}
