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
                Console.Clear();
                await controller.DisplayElevatorStatusesAsync();

                // Show the menu options
                Console.WriteLine("\nElevator Control Panel");
                Console.WriteLine("1. Request Elevator to Transport Passengers");
                Console.WriteLine("2. Add Floor Request (without immediate movement)");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await HandleElevatorRequestAsync(controller);
                        break;
                    case "2":
                        HandleFloorRequest(controller);
                        break;
                    case "3":
                        isRunning = false; // Exit the loop
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again.");
                        Console.ReadLine();
                        break;
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

            Console.Clear();
            Console.WriteLine("Elevator is moving...");
            await controller.DisplayElevatorStatusesAsync();
        }

        private static void HandleFloorRequest(ElevatorController controller)
        {
            Console.Clear();
            Console.WriteLine("Add Floor Request");

            int floor = GetValidInput("Enter the requested floor (0 to 19): ", 0, 19);
            int passengerCount = GetValidInput("Enter the number of passengers: ", 1, int.MaxValue);
            ElevatorType elevatorType = ChooseElevatorTypeAsync().Result;

            if (elevatorType == ElevatorType.None) return;

            controller.AddFloorRequest(floor, passengerCount, elevatorType);

            Console.Clear();
            Console.WriteLine("Floor request added successfully.");
            controller.DisplayElevatorStatusesAsync().Wait();
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

            if (choice == 5) return ElevatorType.None;

            return (ElevatorType)(choice - 1);
        }

        private static int GetValidInput(string prompt, int minValue, int maxValue)
        {
            int value;
            do
            {
                Console.Write(prompt);
                if (!int.TryParse(Console.ReadLine(), out value) || value < minValue || value > maxValue)
                {
                    Console.WriteLine($"Please enter a valid number between {minValue} and {maxValue}.");
                }
            } while (value < minValue || value > maxValue);
            return value;
        }
    }
}
