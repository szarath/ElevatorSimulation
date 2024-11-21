using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    public class FreightElevator : Elevator
    {
        private Queue<int> requestedStops;

        public FreightElevator() : base(2000)
        {
            requestedStops = new Queue<int>();
        }

        public override ElevatorType ElevatorType => ElevatorType.Freight;

    }
}
