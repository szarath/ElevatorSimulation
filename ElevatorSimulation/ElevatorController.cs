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
        public bool isProcessing = false;
        public bool isQueueEmpty = true;
        public ElevatorController(List<IElevator> elevators)
        {
            this.elevators = elevators;
        }

        public async Task RequestElevatorAsync(int requestedFloor, int targetFloor, int passengerCount, ElevatorType elevatorType)
        {
            if (isProcessing) return;

            isProcessing = true;

            try
            {
                var availableElevators = elevators
                    .Where(e => e.ElevatorType == elevatorType && e.CanCarryPassengers(passengerCount))
                    .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                    .ToList();

                if (availableElevators.Any())
                {
                    var closestElevator = availableElevators.First();
                    Console.WriteLine($"Selected Elevator: {closestElevator.ElevatorType} at floor {closestElevator.CurrentFloor}");
                   
                    var statusTask = DisplayElevatorStatusesAsync();
                 
                    await closestElevator.HandleTransportAsync(requestedFloor, targetFloor, passengerCount);
                 
                    isProcessing = false;
                    await statusTask;
                }
                else
                {
                    Console.WriteLine("No elevators available for this request.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                isProcessing = false;
                Console.WriteLine("\nElevator operation completed.");
                Console.WriteLine("Press Enter to return to the menu...");
                Console.ReadLine();
            }
        }

        public void AddFloorRequest(int floor, int passengerCount, ElevatorType elevatorType)
        {
            if (isProcessing) return;

            queuedRequests.Enqueue((floor, passengerCount, elevatorType));
            Console.WriteLine($"Request added for {elevatorType} at floor {floor}.");
        }

        public async Task ProcessQueuedRequestsAsync()
        {
            if (isProcessing) return;

            isProcessing = true;
            isQueueEmpty = false;
            Console.WriteLine("\nProcessing queued requests...");

            var statusTask = DisplayElevatorStatusesAsync();
            var tasks = new List<Task>();

            while (queuedRequests.Count > 0)
            {
                var (floor, passengerCount, elevatorType) = queuedRequests.Dequeue();
                var availableElevator = elevators.FirstOrDefault(e => e.ElevatorType == elevatorType);

                if (availableElevator != null)
                {
                    Console.WriteLine($"\nSelected Elevator: {availableElevator.ElevatorType} at floor {availableElevator.CurrentFloor}");

                    tasks.Add(availableElevator.HandleTransportAsync(floor, 0, passengerCount));
                }
                else
                {
                    Console.WriteLine($"\nNo available elevator of type {elevatorType} for the request.");
                }
            }

            await Task.WhenAll(tasks);

            isQueueEmpty = true;
            isProcessing = false;

            await statusTask;

            Console.WriteLine("\nAll queued requests processed.");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }

        public void DisplayElevatorStatuses()
        {
            Console.Clear();
            Console.WriteLine("\nElevator Statuses:");
            foreach (var elevator in elevators)
            {
                elevator.DisplayStatus();
            }
        }

        public async Task DisplayElevatorStatusesAsync()
        {
            while (isProcessing)
            {
                Console.Clear();
                Console.WriteLine("\nElevator Statuses:");
                foreach (var elevator in elevators)
                {
                    elevator.DisplayStatus();
                }
                await Task.Delay(1000);
            }
        }

    }
}