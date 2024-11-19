using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class GlassElevator : Elevator
    {
        public bool HasGlassWalls { get; set; } = true;

        public override void Move(int targetFloor)
        {
            Console.WriteLine("Glass elevator moving with glass walls...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
            CurrentFloor = targetFloor;
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"Glass Elevator, Current Floor: {CurrentFloor}, Glass Walls: {HasGlassWalls}");
        }
    }

}
