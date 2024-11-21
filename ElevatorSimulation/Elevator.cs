using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public abstract class Elevator : IElevator
    {
        public int CurrentFloor { get; set; }
        public ElevatorDirection Direction { get; set; } = ElevatorDirection.Idle;
        public int PassengersCount { get; private set; }
        public int WeightLimit { get; private set; }
        public int PassengerWeight { get; set; } = Constants.PassengerWeight;
        public abstract ElevatorType ElevatorType { get; }

        public Elevator(int weightLimit)
        {
            WeightLimit = weightLimit;
        }

        // Move Elevator to a target floor and display status updates
        public async Task MoveAsync(int targetFloor)
        {
            if (targetFloor < 0 || targetFloor >= Constants.MaxFloors)
            {
                Console.WriteLine("Invalid floor. Please choose a floor within the valid range.");
                return;
            }

            if (CurrentFloor == targetFloor)
            {
                Console.WriteLine($"Elevator is already at floor {CurrentFloor}.");
                return;
            }

            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
            Console.WriteLine($"Elevator starting to move {Direction} to floor {targetFloor}...");

            while (CurrentFloor != targetFloor)
            {
                await Task.Delay(500);  // Simulate time delay between floors

                // Update current floor based on direction
                if (Direction == ElevatorDirection.Up)
                    CurrentFloor++;
                else
                    CurrentFloor--;

                // Check if the floor update is correct
                if (CurrentFloor == targetFloor)
                    break;
            }

            Direction = ElevatorDirection.Idle;
            Console.WriteLine($"Elevator has reached floor {targetFloor} and is now stationary.");
        }

        public async Task HandleTransportAsync(int requestedFloor, int targetFloor, int passengerCount)
        {
            Console.WriteLine("\nMoving to requested floor...");
            await MoveAsync(requestedFloor);

            if (passengerCount > 0)
            {
                Console.WriteLine("\nPicking up passengers...");
                await PickupPassengers(passengerCount);
            }

            if (targetFloor != requestedFloor)
            {
                Console.WriteLine("\nMoving to target floor...");
                await MoveAsync(targetFloor);
            }

            if (PassengersCount > 0)
            {
                Console.WriteLine("\nDropping off passengers...");
                await DropOffPassengers(passengerCount);
            }
            else
            {
                Console.WriteLine("\nNo passengers onboard to drop off.");
            }

            Console.WriteLine("\nElevator operation completed.");
        }

        public void DisplayStatus()
        {
            int totalWeight = PassengersCount * PassengerWeight;
            string weightStatus = totalWeight <= WeightLimit ? "Under weight limit" : "Overweight!";
            Console.WriteLine($"Elevator Type: {ElevatorType}, Floor: {CurrentFloor}, Direction: {Direction}, Passengers: {PassengersCount}/{Constants.MaxPassengers}, Weight: {totalWeight} kg ({weightStatus}), Max Capacity: {WeightLimit} kg");
        }

        // Handle passenger pickup and drop-off
        public Task PickupPassengers(int passengerCount)
        {
            if (CanCarryPassengers(passengerCount))
            {
                PassengersCount += passengerCount;
                Console.WriteLine($"{passengerCount} passengers picked up. Total passengers: {PassengersCount}");
            }
            else
            {
                Console.WriteLine("Cannot pick up passengers: Exceeds weight limit or capacity.");
            }
            return Task.CompletedTask;
        }

        public Task DropOffPassengers(int passengerCount)
        {
            if (passengerCount > PassengersCount)
            {
                Console.WriteLine("Error: Attempting to drop off more passengers than currently onboard.");
                return Task.CompletedTask;
            }

            PassengersCount -= passengerCount;
            Console.WriteLine($"{passengerCount} passengers dropped off. Remaining: {PassengersCount}");
            return Task.CompletedTask;
        }

        public bool CanCarryPassengers(int passengerCount)
        {
            int totalWeight = (PassengersCount + passengerCount) * PassengerWeight;
            return (PassengersCount + passengerCount) <= Constants.MaxPassengers && totalWeight <= WeightLimit;
        }
    }

}
