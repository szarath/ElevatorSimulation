using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class FreightElevator : Elevator
    {
        private Queue<int> requestedStops; // Track floors where passengers need to get off

        public FreightElevator() : base(2000)
        {
            requestedStops = new Queue<int>();
        }

        public override ElevatorType ElevatorType => ElevatorType.Freight;

        // Overriding MoveAsync method from the base class
        public override async Task MoveAsync(int targetFloor)
        {
            Console.WriteLine("Freight elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            // Add the requested floor to the list of stops
            requestedStops.Enqueue(targetFloor);

            // Simulate movement
            while (requestedStops.Count > 0)
            {
                // Get the next requested stop
                int stopFloor = requestedStops.Peek();

                // Move towards the target floor
                while (CurrentFloor != stopFloor)
                {
                    await Task.Delay(600); // Simulate slower movement for freight elevators
                    CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
                    Console.WriteLine($"Freight elevator at floor {CurrentFloor}...");
                }

                // At the stop floor
                Console.WriteLine($"Freight elevator reached floor {CurrentFloor}.");
                requestedStops.Dequeue(); // Remove the current stop from the queue

                // Drop off passengers at the target floor if any
                DropOffPassengers(PassengersCount); // Drop all passengers at the current floor
            }

            Direction = ElevatorDirection.Idle;
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"Freight Elevator, Current Floor: {CurrentFloor}, Max Load: {WeightLimit}");
        }
    }
}
