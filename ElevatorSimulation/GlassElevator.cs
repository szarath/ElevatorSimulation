using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class GlassElevator : Elevator
    {
        public bool HasGlassWalls { get; set; } = true;

        public GlassElevator() : base(800) { }

        public override ElevatorType ElevatorType => ElevatorType.Glass;

    }
}
