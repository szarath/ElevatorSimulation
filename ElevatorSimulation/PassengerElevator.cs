using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class PassengerElevator : Elevator
    {
        public PassengerElevator() : base(1000) { }

        public override ElevatorType ElevatorType => ElevatorType.Passenger;

        // Overriding MoveAsync method from the base class
        public override async Task MoveAsync(int targetFloor)
        {
            Console.WriteLine("Passenger elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            // Simulate movement
            while (CurrentFloor != targetFloor)
            {
                await Task.Delay(400); // Simulate time delay between floors
                CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
                Console.WriteLine($"Passenger elevator at floor {CurrentFloor}...");
            }

            Direction = ElevatorDirection.Idle;
            Console.WriteLine($"Passenger elevator reached floor {CurrentFloor}.");

            // Drop off passengers at the target floor
            DropOffPassengers(PassengersCount);
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"Passenger Elevator, Current Floor: {CurrentFloor}, Direction: {Direction}, Passengers: {PassengersCount}");
        }
    }
}
