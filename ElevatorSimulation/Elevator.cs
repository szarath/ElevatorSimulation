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

        // Asynchronous method to simulate elevator movement
        public virtual async Task MoveAsync(int targetFloor)
        {
            if (targetFloor < 1 || targetFloor > Constants.MaxFloors)
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

            // Simulate elevator moving with delay
            while (CurrentFloor != targetFloor)
            {
                await Task.Delay(500); // Asynchronous delay for floor transition
                CurrentFloor += (Direction == ElevatorDirection.Up ? 1 : -1);
                Console.WriteLine($"Elevator at floor {CurrentFloor}...");
            }

            Direction = ElevatorDirection.Idle;
            Console.WriteLine($"Elevator has reached floor {targetFloor} and is now stationary.");

            // Drop off passengers when the elevator reaches the target floor
            DropOffPassengers(PassengersCount);
        }

        public abstract void DisplayElevatorDetails();

        public bool CanCarryPassengers(int passengerCount)
        {
            int totalWeight = (PassengersCount + passengerCount) * PassengerWeight;
            return PassengersCount + passengerCount <= Constants.MaxPassengers && totalWeight <= WeightLimit;
        }

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
            return Task.CompletedTask;  // Return a completed task as we're not actually performing async operations
        }

        public Task DropOffPassengers(int passengerCount)
        {
            if (passengerCount > PassengersCount)
            {
                Console.WriteLine("Error: Attempting to drop off more passengers than currently onboard.");
                return Task.CompletedTask;  // Return a completed task as an early exit
            }

            PassengersCount -= passengerCount;
            Console.WriteLine($"{passengerCount} passengers dropped off. Remaining: {PassengersCount}");
            return Task.CompletedTask;  // Return a completed task after performing the operation
        }


        public void DisplayStatus()
        {
            int totalWeight = PassengersCount * PassengerWeight;
            string weightStatus = totalWeight <= WeightLimit ? "Under weight limit" : "Overweight!";
            Console.WriteLine($"Elevator Type: {ElevatorType}, Floor: {CurrentFloor}, Direction: {Direction}, Passengers: {PassengersCount}/{Constants.MaxPassengers}, Weight: {totalWeight} kg ({weightStatus}), Max Capacity: {WeightLimit} kg");
        }

    }
}
