using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class HighSpeedElevator : Elevator
    {
        public int MaxSpeed { get; set; } = 100;  // Maximum speed in floors per second

        public HighSpeedElevator() : base(1500) { }

        public override ElevatorType ElevatorType => ElevatorType.HighSpeed;

        // Overriding MoveAsync method from the base class
        public override async Task MoveAsync(int targetFloor)
        {
            Console.WriteLine("High-speed elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            // Simulate movement (faster than passenger elevator)
            while (CurrentFloor != targetFloor)
            {
                await Task.Delay(200); // Simulate faster movement
                CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
                Console.WriteLine($"High-speed elevator at floor {CurrentFloor}...");
            }

            Direction = ElevatorDirection.Idle;
            Console.WriteLine($"High-speed elevator reached floor {CurrentFloor}.");

            // Drop off passengers at the target floor
            DropOffPassengers(PassengersCount);
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"High-Speed Elevator, Max Speed: {MaxSpeed} floors/sec, Current Floor: {CurrentFloor}");
        }
    }
}
