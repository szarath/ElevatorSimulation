using System;
using System.Collections.Generic;
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

        public override async Task MoveAsync(int targetFloor)
        {
            Console.WriteLine("Freight elevator moving...");
            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            requestedStops.Enqueue(targetFloor); // Add target floor to requested stops

            while (requestedStops.Count > 0)
            {
                int stopFloor = requestedStops.Peek();
                while (CurrentFloor != stopFloor)
                {
                    await Task.Delay(600); // Simulate slower movement for freight elevators
                    CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
                    Console.WriteLine($"Freight elevator at floor {CurrentFloor}...");
                }

                Console.WriteLine($"Freight elevator reached floor {CurrentFloor}.");
                requestedStops.Dequeue(); // Remove stop from queue
                await DropOffPassengers(PassengersCount); // Drop all passengers
            }

            Direction = ElevatorDirection.Idle;
        }

        public override void DisplayElevatorDetails()
        {
            Console.WriteLine($"Freight Elevator, Current Floor: {CurrentFloor}, Max Load: {WeightLimit}");
        }
    }
}
