namespace ElevatorSimulation.Tests
{
    public class ElevatorControllerTests
    {
        private readonly Mock<IElevator> _mockPassengerElevator;
        private readonly Mock<IElevator> _mockFreightElevator;
        private readonly ElevatorController _controller;

        public ElevatorControllerTests()
        {
            _mockPassengerElevator = new Mock<IElevator>();
            _mockFreightElevator = new Mock<IElevator>();

            var elevators = new List<IElevator>
            {
                _mockPassengerElevator.Object,
                _mockFreightElevator.Object
            };

            _controller = new ElevatorController(elevators);
        }

        [Fact]
        public async Task RequestElevatorAsync_Should_Move_To_Requested_Floor_And_Pickup_Passengers()
        {
            // Arrange
            var requestedFloor = 5;
            var targetFloor = 10;
            var passengers = 3;

            _mockPassengerElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(true);
            _mockPassengerElevator.Setup(e => e.CurrentFloor).Returns(1);
            _mockPassengerElevator.Setup(e => e.PassengersCount).Returns(passengers);

            _mockPassengerElevator.Setup(e => e.PickupPassengers(passengers)).Returns(Task.CompletedTask);
            _mockPassengerElevator.Setup(e => e.DropOffPassengers(passengers)).Returns(Task.CompletedTask);

            _mockPassengerElevator.Setup(e => e.HandleTransportAsync(requestedFloor, targetFloor, passengers))
                .Returns(Task.CompletedTask)
                .Callback<int, int, int>((rf, tf, pc) =>
                {
                    if (pc > 0)
                    {
                        _mockPassengerElevator.Object.PickupPassengers(pc).Wait();
                    }

                    if (_mockPassengerElevator.Object.PassengersCount > 0)
                    {
                        _mockPassengerElevator.Object.DropOffPassengers(pc).Wait();
                    }
                });

            // Act
            await _controller.RequestElevatorAsync(requestedFloor, targetFloor, passengers, ElevatorType.Passenger);

            // Assert
            _mockPassengerElevator.Verify(e => e.HandleTransportAsync(requestedFloor, targetFloor, passengers), Times.Once); 
            _mockPassengerElevator.Verify(e => e.PickupPassengers(passengers), Times.Once);
            _mockPassengerElevator.Verify(e => e.DropOffPassengers(passengers), Times.Once);
        }


        [Fact]
        public async Task RequestElevatorAsync_Should_Not_Move_When_No_Elevators_Can_Carry_Passengers()
        {
            // Arrange
            var requestedFloor = 5;
            var targetFloor = 10;
            var passengers = 10; // Exceeds capacity

            _mockPassengerElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(false);
            _mockPassengerElevator.Setup(e => e.HandleTransportAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            _mockFreightElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(false);
            _mockFreightElevator.Setup(e => e.HandleTransportAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            await _controller.RequestElevatorAsync(requestedFloor, targetFloor, passengers, ElevatorType.Passenger);

            // Assert
            _mockPassengerElevator.Verify(e => e.HandleTransportAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockFreightElevator.Verify(e => e.HandleTransportAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
        [Fact]
        public async Task RequestElevatorAsync_Should_Return_No_Available_Elevator_When_None_Fits_Capacity()
        {
            // Arrange
            var requestedFloor = 5;
            var targetFloor = 10;
            var passengers = 10;

            _mockPassengerElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(false);
            _mockFreightElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(false); 

            // Act
            await _controller.RequestElevatorAsync(requestedFloor, targetFloor, passengers, ElevatorType.Passenger);

            // Assert
            _mockPassengerElevator.Verify(e => e.MoveAsync(It.IsAny<int>()), Times.Never);
            _mockFreightElevator.Verify(e => e.MoveAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RequestElevatorAsync_Should_Not_Move_When_No_Elevators_Available()
        {
            // Arrange
            var requestedFloor = 5;
            var targetFloor = 10;
            var passengers = 3;

            _mockPassengerElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(false);
            _mockFreightElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(false);

            // Act
            await _controller.RequestElevatorAsync(requestedFloor, targetFloor, passengers, ElevatorType.Passenger);

            // Assert
            _mockPassengerElevator.Verify(e => e.MoveAsync(It.IsAny<int>()), Times.Never);
            _mockFreightElevator.Verify(e => e.MoveAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DisplayElevatorStatusesAsync_Should_Print_Status_For_Elevators()
        {
            // Arrange
            _mockPassengerElevator.Setup(e => e.CurrentFloor).Returns(1);
            _mockFreightElevator.Setup(e => e.CurrentFloor).Returns(2);

            // Act
            await _controller.DisplayElevatorStatusesAsync();

            // Assert
            _mockPassengerElevator.Verify(e => e.DisplayStatus(), Times.Once);
            _mockFreightElevator.Verify(e => e.DisplayStatus(), Times.Once);
        }
    }
}
