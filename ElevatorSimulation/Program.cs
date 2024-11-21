using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ElevatorController controller = InitializeController();

            bool isRunning = true;

            while (isRunning)
            {
                if (!controller.isProcessing)
                {
                    Console.Clear();
                    await controller.DisplayElevatorStatusesAsync();

                    Console.WriteLine("\nElevator Control Panel");
                    Console.WriteLine("1. Request Elevator to Transport Passengers");
                    Console.WriteLine("2. Add Floor Request (without immediate movement)");
                    Console.WriteLine("3. Process All Queued Requests");
                    Console.WriteLine("4. Exit");
                    Console.Write("Enter your choice: ");

                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            await HandleElevatorRequestAsync(controller);
                            break;
                        case "2":
                            await HandleFloorRequestAsync(controller);
                            break;
                        case "3":
                            await ProcessQueuedRequestsAsync(controller);
                            break;
                        case "4":
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press Enter to try again.");
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Processing request... Please wait.");
                    await Task.Delay(1000);
                }
            }

            Console.WriteLine("Exiting the program. Goodbye!");
        }

        private static ElevatorController InitializeController()
        {
            List<IElevator> elevators = new List<IElevator>
            {
                new PassengerElevator { CurrentFloor = 0, Direction = ElevatorDirection.Idle },
                new FreightElevator { CurrentFloor = 5, Direction = ElevatorDirection.Idle },
                new HighSpeedElevator { CurrentFloor = 3, Direction = ElevatorDirection.Down },
                new GlassElevator { CurrentFloor = 0, Direction = ElevatorDirection.Up }
            };

            return new ElevatorController(elevators);
        }

        private static async Task HandleElevatorRequestAsync(ElevatorController controller)
        {
            Console.Clear();
            Console.WriteLine("Elevator Request");

            int requestedFloor = GetValidInput("Enter the requested floor (0 to 19): ", 0, 19);
            int targetFloor = GetValidInput("Enter the target floor (0 to 19): ", 0, 19);
            int passengerCount = GetValidInput("Enter the number of passengers: ", 1, int.MaxValue);
            ElevatorType elevatorType = await ChooseElevatorTypeAsync();

            if (elevatorType == ElevatorType.None) return;

            await controller.RequestElevatorAsync(requestedFloor, targetFloor, passengerCount, elevatorType);
        }

        private static async Task HandleFloorRequestAsync(ElevatorController controller)
        {
            Console.Clear();
            Console.WriteLine("Add Floor Request");

            int floor = GetValidInput("Enter the requested floor (0 to 19): ", 0, 19);
            int passengerCount = GetValidInput("Enter the number of passengers: ", 1, int.MaxValue);
            ElevatorType elevatorType = await ChooseElevatorTypeAsync();

            if (elevatorType == ElevatorType.None) return;

            controller.AddFloorRequest(floor, passengerCount, elevatorType);

            Console.Clear();
            Console.WriteLine("Floor request added successfully.");
        }

        private static async Task ProcessQueuedRequestsAsync(ElevatorController controller)
        {
            Console.Clear();
            await controller.ProcessQueuedRequestsAsync();
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        private static async Task<ElevatorType> ChooseElevatorTypeAsync()
        {
            Console.WriteLine("Choose Elevator Type:");
            Console.WriteLine("1. Passenger Elevator");
            Console.WriteLine("2. Freight Elevator");
            Console.WriteLine("3. High-Speed Elevator");
            Console.WriteLine("4. Glass Elevator");
            Console.WriteLine("5. Return to Main Menu");

            int choice = GetValidInput("Enter elevator type (1-5): ", 1, 5);

            return choice switch
            {
                1 => ElevatorType.Passenger,
                2 => ElevatorType.Freight,
                3 => ElevatorType.HighSpeed,
                4 => ElevatorType.Glass,
                _ => ElevatorType.None,
            };
        }

        private static int GetValidInput(string prompt, int minValue, int maxValue)
        {
            int value;
            do
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value >= minValue && value <= maxValue)
                {
                    break;
                }
                Console.WriteLine($"Invalid input. Please enter a value between {minValue} and {maxValue}.");
            } while (true);

            return value;
        }
    }
}
