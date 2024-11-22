using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class ElevatorController
    {
        private List<IElevator> elevators;
        private Dictionary<IElevator, Queue<(int requestedFloor, int targetFloor, int passengerCount)>> elevatorRequests;
        private Dictionary<IElevator, Task> elevatorTasks;

        public bool IsProcessing => elevatorTasks.Values.Any(task => !task.IsCompleted);

        public ElevatorController(List<IElevator> elevators)
        {
            this.elevators = elevators;
            elevatorRequests = elevators.ToDictionary(elevator => elevator, _ => new Queue<(int, int, int)>());
            elevatorTasks = elevators.ToDictionary(elevator => elevator, _ => Task.CompletedTask);
        }

        public async Task RequestElevatorAsync(int requestedFloor, int targetFloor, int passengerCount, ElevatorType elevatorType)
        {
            var elevator = elevators
                .Where(e => e.ElevatorType == elevatorType)
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .FirstOrDefault();

            if (elevator == null)
            {
                Console.WriteLine("No available elevator of the requested type.");
                return;
            }

            if (elevator.CurrentFloor != requestedFloor)
            {
                elevator.Direction = requestedFloor > elevator.CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
                await MoveElevatorToFloorAsync(elevator, requestedFloor);
                elevator.Direction = ElevatorDirection.Idle;
                DisplayElevatorStatuses();
            }
            else
            {
                Console.WriteLine($"Elevator is already at the requested floor {requestedFloor}. No movement needed.");
            }
        
            while (passengerCount > 0)
            {
                if (!elevator.CanCarryPassengers(passengerCount))
                {              
                    int allowablePassengers = elevator.MaxPassengers - elevator.PassengersCount;
                    await elevator.PickupPassengers(allowablePassengers);
                    passengerCount -= allowablePassengers;
                }
                else
                {
                    await elevator.PickupPassengers(passengerCount);
                    passengerCount = 0; 
                }
                DisplayElevatorStatuses();
            }

            if (elevator.CurrentFloor != targetFloor)
            {
                elevator.Direction = targetFloor > elevator.CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
                await MoveElevatorToFloorAsync(elevator, targetFloor);
                elevator.Direction = ElevatorDirection.Idle;
                DisplayElevatorStatuses();
            }

            await elevator.DropOffPassengers(elevator.PassengersCount);
            DisplayElevatorStatuses();

            elevator.Direction = ElevatorDirection.Idle;
            DisplayElevatorStatuses();
        }

        public async Task ProcessQueuedRequestsAsync()
        {
            var processingTasks = new List<Task>();

            foreach (var elevator in elevators)
            {
                if (elevatorRequests[elevator].Any())
                {
                    if (elevatorTasks[elevator].IsCompleted)
                    {
                        elevatorTasks[elevator] = ProcessElevatorRequestsAsync(elevator);
                    }

                    processingTasks.Add(elevatorTasks[elevator]);
                }
            }

            await Task.WhenAll(processingTasks);

            DisplayElevatorStatuses();
        }

        private async Task ProcessElevatorRequestsAsync(IElevator elevator)
        {
            while (elevatorRequests[elevator].Any())
            {
                var (requestedFloor, targetFloor, passengerCount) = elevatorRequests[elevator].Dequeue();

                while (passengerCount > 0)
                {
                    if (!elevator.CanCarryPassengers(passengerCount))
                    {
                        var elevatorInstance = elevator as Elevator;
                        int allowablePassengers = elevatorInstance?.MaxPassengers - elevator.PassengersCount ?? 0;
                        Console.WriteLine($"Capacity exceeded. Picking up {allowablePassengers} passengers first.");
                        await elevator.PickupPassengers(allowablePassengers);
                        passengerCount -= allowablePassengers;
                    }
                    else
                    {
                        await elevator.PickupPassengers(passengerCount);
                        passengerCount = 0;
                    }
                    DisplayElevatorStatuses();
                }

                if (elevator.CurrentFloor != requestedFloor)
                {
                    elevator.Direction = requestedFloor > elevator.CurrentFloor
                        ? ElevatorDirection.Up
                        : ElevatorDirection.Down;

                    Console.WriteLine($"Moving to requested floor {requestedFloor}...");
                    await MoveElevatorToFloorAsync(elevator, requestedFloor);
                    elevator.Direction = ElevatorDirection.Idle;
                }

                await elevator.DropOffPassengers(elevator.PassengersCount);
                DisplayElevatorStatuses();

                if (targetFloor != requestedFloor)
                {
                    elevator.Direction = targetFloor > elevator.CurrentFloor
                        ? ElevatorDirection.Up
                        : ElevatorDirection.Down;

                    Console.WriteLine($"Moving to target floor {targetFloor}...");
                    await MoveElevatorToFloorAsync(elevator, targetFloor);
                    elevator.Direction = ElevatorDirection.Idle;
                    DisplayElevatorStatuses();
                }

                await elevator.DropOffPassengers(elevator.PassengersCount);
                DisplayElevatorStatuses();

                if (!elevatorRequests[elevator].Any() && elevator.CurrentFloor != 0)
                {
                    await MoveElevatorToFloorAsync(elevator, 0);
                    elevator.Direction = ElevatorDirection.Idle;
                    DisplayElevatorStatuses();
                }
            }
        }

        private async Task MoveElevatorToFloorAsync(IElevator elevator, int targetFloor)
        {
            elevator.Direction = targetFloor > elevator.CurrentFloor
                ? ElevatorDirection.Up
                : ElevatorDirection.Down;

            while (elevator.CurrentFloor != targetFloor)
            {
                elevator.CurrentFloor += (elevator.CurrentFloor < targetFloor) ? 1 : -1;
                DisplayElevatorStatuses();
                await Task.Delay(500);
            }

            elevator.Direction = ElevatorDirection.Idle;
            DisplayElevatorStatuses();
        }

        public void AddFloorRequest(int floor, int passengerCount, ElevatorType elevatorType)
        {
            var elevator = elevators
                .Where(e => e.ElevatorType == elevatorType)
                .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
                .FirstOrDefault();

            if (elevator == null)
            {
                Console.WriteLine("No available elevator of the requested type.");
                return;
            }

            elevatorRequests[elevator].Enqueue((floor, floor, passengerCount));
        }

        public void DisplayElevatorStatuses()
        {
            Console.Clear();
            Console.WriteLine("\nElevator Statuses:");
            foreach (var elevator in elevators)
            {
                elevator.TotalWeight = elevator.PassengersCount * Constants.PassengerWeight;
                elevator.WeightStatus = elevator.TotalWeight <= elevator.WeightLimit ? "Under weight limit" : "Overweight!";
                elevator.DisplayStatus();
            }
        }

        public async Task DisplayElevatorStatusesAsync()
        {
            while (IsProcessing)
            {
                DisplayElevatorStatuses();
                await Task.Delay(1000);
            }

            DisplayElevatorStatuses();
        }
    }
}
