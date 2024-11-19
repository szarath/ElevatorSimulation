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
            _mockPassengerElevator.Setup(e => e.MoveAsync(requestedFloor)).Returns(Task.CompletedTask);
            _mockPassengerElevator.Setup(e => e.PickupPassengers(passengers)).Verifiable();
            _mockPassengerElevator.Setup(e => e.MoveAsync(targetFloor)).Returns(Task.CompletedTask);
            _mockPassengerElevator.Setup(e => e.DropOffPassengers(passengers)).Verifiable();

            // Act
            await _controller.RequestElevatorAsync(requestedFloor, targetFloor, passengers, ElevatorType.Passenger);

            // Assert
            _mockPassengerElevator.Verify(e => e.MoveAsync(requestedFloor), Times.Once);
            _mockPassengerElevator.Verify(e => e.PickupPassengers(passengers), Times.Once);
            _mockPassengerElevator.Verify(e => e.MoveAsync(targetFloor), Times.Once);
            _mockPassengerElevator.Verify(e => e.DropOffPassengers(passengers), Times.Once);
        }

        [Fact]
        public async Task RequestElevatorAsync_Should_Choose_Elevator_That_Can_Carry_Passengers_And_Is_On_Requested_Floor()
        {
            // Arrange
            var requestedFloor = 5;
            var targetFloor = 10;
            var passengers = 3;

            _mockPassengerElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(true);  
            _mockPassengerElevator.Setup(e => e.CurrentFloor).Returns(requestedFloor);  

            _mockFreightElevator.Setup(e => e.CanCarryPassengers(passengers)).Returns(true); 
            _mockFreightElevator.Setup(e => e.CurrentFloor).Returns(8); 

            _mockPassengerElevator.Setup(e => e.MoveAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            _mockFreightElevator.Setup(e => e.MoveAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _controller.RequestElevatorAsync(requestedFloor, targetFloor, passengers, ElevatorType.Passenger);

            // Assert
            _mockPassengerElevator.Verify(e => e.MoveAsync(requestedFloor), Times.Once);  
            _mockPassengerElevator.Verify(e => e.MoveAsync(targetFloor), Times.Once);    

            _mockFreightElevator.Verify(e => e.MoveAsync(It.IsAny<int>()), Times.Never);  
        }


        [Fact]
        public async Task RequestElevatorAsync_Should_Return_No_Available_Elevator_When_None_Fits_Capacity()
        {
            // Arrange
            var requestedFloor = 5;
            var targetFloor = 10;
            var passengers = 10; // Exceeding capacity

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
