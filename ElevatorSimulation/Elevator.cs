using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public abstract class Elevator : IElevator
    {
        public int CurrentFloor { get; set; }
        public ElevatorDirection Direction { get; set; } = ElevatorDirection.Idle;
        public int PassengersCount { get; set; }
        public int WeightLimit { get; set; }
        public int PassengerWeight { get; set; } = Constants.PassengerWeight;
        public abstract ElevatorType ElevatorType { get; }
        public int TotalWeight { get; set; }
        public string WeightStatus { get; set; }

        public int MaxPassengers
        {
            get => (int)Math.Round((WeightLimit / (double)PassengerWeight), 0);
        }

        public Elevator(int weightLimit)
        {
            WeightLimit = weightLimit;
        }

        public async Task MoveAsync(int targetFloor)
        {
            if (targetFloor < 0 || targetFloor >= Constants.MaxFloors)
            {
                Console.WriteLine($"Invalid floor. Please choose a floor between 0 and {Constants.MaxFloors - 1}.");
                return;
            }

            if (CurrentFloor == targetFloor)
            {
                Console.WriteLine($"Elevator is already at floor {CurrentFloor}.");
                return;
            }

            Direction = targetFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

            while (CurrentFloor != targetFloor)
            {
                await Task.Delay(500);

                if (Direction == ElevatorDirection.Up)
                    CurrentFloor++;
                else if (Direction == ElevatorDirection.Down)
                    CurrentFloor--;
                if (CurrentFloor == targetFloor)
                    break;
            }

            Direction = ElevatorDirection.Idle;
        }

        public async Task HandleTransportAsync(int requestedFloor, int targetFloor, int passengerCount)
        {
            await MoveAsync(requestedFloor);

            if (passengerCount > 0)
            {
                await PickupPassengers(passengerCount);
            }

            if (targetFloor != requestedFloor)
            {
                await MoveAsync(targetFloor);
            }

            if (PassengersCount > 0)
            {
                await DropOffPassengers(passengerCount > 0 ? passengerCount : PassengersCount);
            }
            else
            {
                Console.WriteLine("\nNo passengers onboard to drop off.");
            }

            Direction = ElevatorDirection.Idle;
        }

        public void DisplayStatus()
        {
            TotalWeight = PassengersCount * PassengerWeight;
            WeightStatus = TotalWeight <= WeightLimit ? "Under weight limit" : "Overweight!";
            Console.WriteLine($"Elevator Type: {ElevatorType}, Floor: {CurrentFloor}, Direction: {Direction}, Passengers: {PassengersCount}/{MaxPassengers}, Weight: {TotalWeight} kg ({WeightStatus}), Max Capacity: {WeightLimit} kg");
        }

        public Task PickupPassengers(int passengerCount)
        {
            if (CanCarryPassengers(passengerCount))
            {
                PassengersCount += passengerCount;
                TotalWeight = PassengersCount * PassengerWeight;
            }
            else
            {
                Console.WriteLine("Cannot pick up passengers: Exceeds weight limit or capacity.");
            }
            return Task.CompletedTask;
        }

        public async Task DropOffPassengers(int passengerCount)
        {
            if (passengerCount > PassengersCount)
            {
                Console.WriteLine("Error: Attempting to drop off more passengers than currently onboard.");
                return;
            }

            PassengersCount -= passengerCount;
            TotalWeight = PassengersCount * PassengerWeight;

            await Task.CompletedTask;
        }

        public bool CanCarryPassengers(int passengerCount)
        {
            TotalWeight = (PassengersCount + passengerCount) * PassengerWeight;
            return (PassengersCount + passengerCount) <= MaxPassengers && TotalWeight <= WeightLimit;
        }
    }
}
