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
        int PassengersCount { get;  }
        int WeightLimit { get;  }
        ElevatorType ElevatorType { get; }
        Task MoveAsync(int targetFloor);
        void DisplayStatus();
        Task PickupPassengers(int passengerCount);
        Task DropOffPassengers(int passengerCount);
        bool CanCarryPassengers(int passengerCount);
        void DisplayElevatorDetails();
    }

}
