using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public interface IElevator
    {
        int CurrentFloor { get; set; }
        ElevatorDirection Direction { get; set; }
        int PassengersCount { get; set; }
        int WeightLimit { get; set; }

        void Move(int targetFloor);
        void DisplayStatus();
        void PickupPassengers(int passengerCount);
        void DropOffPassengers(int passengerCount);
        bool CanCarryPassengers(int passengerCount);
        void DisplayElevatorDetails();
    }

}
