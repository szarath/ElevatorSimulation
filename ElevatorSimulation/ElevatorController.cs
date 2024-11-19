using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class ElevatorController
    {
        private List<IElevator> elevators;

        public ElevatorController(List<IElevator> elevators)
        {
            this.elevators = elevators;
        }

        // Requests an elevator to pick up passengers at a specified floor and move them to a target floor
        public async Task RequestElevatorAsync(int requestedFloor, int targetFloor, int passengerCount, ElevatorType elevatorType)
        {
            var availableElevators = elevators
                .Where(e => e.ElevatorType == elevatorType && e.CanCarryPassengers(passengerCount))
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .ToList();

            if (availableElevators.Any())
            {
                var closestElevator = availableElevators.First();
                Console.WriteLine($"Selected Elevator: {closestElevator.ElevatorType} at floor {closestElevator.CurrentFloor}");

                await closestElevator.MoveAsync(requestedFloor);  // Should be called here
                await closestElevator.PickupPassengers(passengerCount);
                await closestElevator.MoveAsync(targetFloor);  // Should be called here
                await closestElevator.DropOffPassengers(passengerCount);
            }
            else
            {
                Console.WriteLine("No elevators available for this request.");
            }
        }



        // Adds a request for an elevator without immediately moving passengers
        public void AddFloorRequest(int floor, int passengerCount, ElevatorType elevatorType)
        {
            Console.WriteLine($"Request added for elevator type {elevatorType} to pick up passengers at floor {floor}.");
        }

        // Display current elevator statuses
        public async Task DisplayElevatorStatusesAsync()
        {
            Console.WriteLine("\nElevator Statuses:");
            foreach (var elevator in elevators)
            {
                elevator.DisplayStatus();
            }
        }
    }
}
