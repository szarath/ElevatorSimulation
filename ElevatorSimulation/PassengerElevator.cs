using System;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class PassengerElevator : Elevator
    {
        public PassengerElevator() : base(1000) { }

        public override ElevatorType ElevatorType => ElevatorType.Passenger;

    }
}
