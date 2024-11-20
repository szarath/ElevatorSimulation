using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class GlassElevator : Elevator
    {
        public bool HasGlassWalls { get; set; } = true;

        public GlassElevator() : base(800) { }

        public override ElevatorType ElevatorType => ElevatorType.Glass;

        public override async Task MoveAsync(int targetFloor)
        {
            Console.WriteLine("Glass elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            while (CurrentFloor != targetFloor)
            {
                await Task.Delay(400); // Simulate time delay between floors
                CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
                Console.WriteLine($"Glass elevator at floor {CurrentFloor}...");
            }

            Direction = ElevatorDirection.Idle;
            Console.WriteLine($"Glass elevator reached floor {CurrentFloor}.");
            await DropOffPassengers(PassengersCount);
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"Glass Elevator, Current Floor: {CurrentFloor}, Glass Walls: {HasGlassWalls}");
        }
    }
}
