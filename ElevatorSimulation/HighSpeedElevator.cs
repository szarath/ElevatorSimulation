using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class HighSpeedElevator : Elevator
    {
        public int MaxSpeed { get; set; } = 100;

        public HighSpeedElevator() : base(1500) { }

        public override ElevatorType ElevatorType => ElevatorType.HighSpeed;

    }
}
