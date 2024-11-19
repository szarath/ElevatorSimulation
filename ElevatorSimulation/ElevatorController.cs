using ElevatorSimulation;

public class ElevatorController
{
    private List<IElevator> elevators;
    private Dictionary<int, Queue<int>> floorRequests;

    public ElevatorController(List<IElevator> elevators)
    {
        this.elevators = elevators;
        floorRequests = new Dictionary<int, Queue<int>>();
        for (int i = 0; i < Constants.MaxFloors; i++)
        {
            floorRequests[i] = new Queue<int>();
        }
    }

    public void RequestElevator(int requestedFloor, int passengerCount, ElevatorType elevatorType)
    {
        var closestElevator = elevators
            .Where(e => e.GetType().Name.Contains(elevatorType.ToString()) && e.CanCarryPassengers(passengerCount))
            .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
            .FirstOrDefault();

        if (closestElevator != null)
        {
            closestElevator.Move(requestedFloor);
            closestElevator.PickupPassengers(passengerCount);
            Console.WriteLine($"Elevator {closestElevator.GetType().Name} moving to floor {requestedFloor} with {passengerCount} passengers.");
        }
        else
        {
            Console.WriteLine("No elevator available for this request.");
        }
    }

    public void AddFloorRequest(int floor, int passengerCount)
    {
        if (floor >= 0 && floor < Constants.MaxFloors)
        {
            floorRequests[floor].Enqueue(passengerCount);
            Console.WriteLine($"Request added for floor {floor} with {passengerCount} passengers.");
        }
        else
        {
            Console.WriteLine("Invalid floor request.");
        }
    }

    public void DispatchElevators()
    {
        foreach (var floorRequest in floorRequests)
        {
            if (floorRequest.Value.Count > 0)
            {
                Console.WriteLine($"Floor {floorRequest.Key} has {floorRequest.Value.Count} pending requests.");
                foreach (var passengers in floorRequest.Value)
                {
                    RequestElevator(floorRequest.Key, passengers, ElevatorType.Passenger);  // Default to Passenger for now
                }
            }
        }
    }

    public void DisplayElevatorStatuses()
    {
        foreach (var elevator in elevators)
        {
            elevator.DisplayStatus();
            elevator.DisplayElevatorDetails();
        }
    }
}
