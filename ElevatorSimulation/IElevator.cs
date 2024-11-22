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
        int TotalWeight { get; set; }
        ElevatorType ElevatorType { get; }
        string WeightStatus { get; set; }
        int MaxPassengers { get; }
        Task MoveAsync(int targetFloor);
        void DisplayStatus();
        Task PickupPassengers(int passengerCount);
        Task DropOffPassengers(int passengerCount);
        bool CanCarryPassengers(int passengerCount);
        Task HandleTransportAsync(int requestedFloor, int targetFloor, int passengerCount);
    }

}
