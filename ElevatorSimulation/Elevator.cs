using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public abstract class Elevator : IElevator
    {
        public int CurrentFloor { get; set; }
        public ElevatorDirection Direction { get; set; }
        public int PassengersCount { get; set; }
        public int WeightLimit { get; set; } = Constants.WeightLimit;

        public abstract void Move(int targetFloor);
        public abstract void DisplayElevatorDetails();  // Custom display for each elevator type

        public bool CanCarryPassengers(int passengerCount)
        {
            int totalWeight = passengerCount * 75; // Assuming each passenger weighs 75kg
            return PassengersCount + passengerCount <= Constants.MaxPassengers && totalWeight <= WeightLimit;
        }

        public void PickupPassengers(int passengerCount)
        {
            if (CanCarryPassengers(passengerCount))
            {
                PassengersCount += passengerCount;
            }
        }

        public void DropOffPassengers(int passengerCount)
        {
            PassengersCount -= passengerCount;
        }

        public void DisplayStatus()
        {
            Console.WriteLine($"Elevator at Floor {CurrentFloor}, Direction: {Direction}, Passengers: {PassengersCount}");
        }
    }

}
