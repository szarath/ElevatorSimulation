using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class GlassElevator : Elevator
    {
        public bool HasGlassWalls { get; set; } = true;

        public GlassElevator() : base(800) { }

        public override ElevatorType ElevatorType => ElevatorType.Glass;

        // Overriding MoveAsync method from the base class
        public override async Task MoveAsync(int targetFloor)
        {
            Console.WriteLine("Glass elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            // Simulate movement
            while (CurrentFloor != targetFloor)
            {
                await Task.Delay(400); // Simulate time delay between floors
                CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
                Console.WriteLine($"Glass elevator at floor {CurrentFloor}...");
            }

            Direction = ElevatorDirection.Idle;
            Console.WriteLine($"Glass elevator reached floor {CurrentFloor}.");

            // Drop off passengers at the target floor
            DropOffPassengers(PassengersCount);
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"Glass Elevator, Current Floor: {CurrentFloor}, Glass Walls: {HasGlassWalls}");
        }
    }
}
