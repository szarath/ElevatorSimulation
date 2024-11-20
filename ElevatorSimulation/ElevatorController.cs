using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class ElevatorController
    {
        private List<IElevator> elevators;
        private Queue<(int floor, int passengerCount, ElevatorType elevatorType)> queuedRequests = new();

        public ElevatorController(List<IElevator> elevators)
        {
            this.elevators = elevators;
        }

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
            
                await closestElevator.HandleTransportAsync(requestedFloor, targetFloor, passengerCount);
            }
            else
            {
                Console.WriteLine("No elevators available for this request.");
            }
        }

        public void AddFloorRequest(int floor, int passengerCount, ElevatorType elevatorType)
        {
            queuedRequests.Enqueue((floor, passengerCount, elevatorType));
            Console.WriteLine($"Request added for {elevatorType} at floor {floor}.");
        }

        public async Task ProcessQueuedRequestsAsync()
        {
            Console.WriteLine("\nProcessing queued requests...");
            while (queuedRequests.Count > 0)
            {
                var (floor, passengerCount, elevatorType) = queuedRequests.Dequeue();

                var availableElevator = elevators.FirstOrDefault(e => e.ElevatorType == elevatorType);

                if (availableElevator != null)
                {
                    Console.WriteLine($"\nSelected Elevator: {availableElevator.ElevatorType} at floor {availableElevator.CurrentFloor}");
            
                    await availableElevator.HandleTransportAsync(floor, 0, passengerCount);
                }
                else
                {
                    Console.WriteLine($"\nNo available elevator of type {elevatorType} for the request.");
                }
            }
            Console.WriteLine("\nAll queued requests processed. Press Enter to return to the menu...");
            Console.ReadLine();
        }

        public void DisplayElevatorStatuses()
        {
            Console.WriteLine("\nElevator Statuses:");
            foreach (var elevator in elevators)
            {
                elevator.DisplayStatus();
            }
        }

        public async Task DisplayElevatorStatusesAsync()
        {
           
            DisplayElevatorStatuses();
            await Task.CompletedTask;
        }
    }
}
