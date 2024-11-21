namespace ElevatorSimulation.Tests
{
    public class ElevatorTests
    {
        [Fact]
        public async Task MoveAsync_ShouldMoveToTargetFloor()
        {
            // Arrange
            var elevator = new PassengerElevator { CurrentFloor = 0 };

            // Act
            await elevator.MoveAsync(5);

            // Assert
            Assert.Equal(5, elevator.CurrentFloor);
        }

        [Fact]
        public async Task HandleTransportAsync_ShouldPickupAndDropOffPassengers()
        {
            // Arrange
            var elevator = new PassengerElevator { CurrentFloor = 0 };

            // Act
            await elevator.HandleTransportAsync(2, 5, 3);

            // Assert
            Assert.Equal(5, elevator.CurrentFloor);
            Assert.Equal(0, elevator.PassengersCount);
        }

        [Fact]
        public void CanCarryPassengers_ShouldReturnFalseIfOverCapacity()
        {
            // Arrange
            var elevator = new PassengerElevator();

            // Act
            var canCarry = elevator.CanCarryPassengers(20);

            // Assert
            Assert.False(canCarry);
        }

        [Fact]
        public void CanCarryPassengers_ShouldReturnTrueIfWithinCapacity()
        {
            // Arrange
            var elevator = new PassengerElevator();

            // Act
            var canCarry = elevator.CanCarryPassengers(5);

            // Assert
            Assert.True(canCarry);
        }

        [Fact]
        public async Task MoveAsync_ShouldNotMoveToInvalidFloor()
        {
            // Arrange
            var elevator = new PassengerElevator { CurrentFloor = 0 };

            // Act
            await elevator.MoveAsync(25);

            // Assert
            Assert.Equal(0, elevator.CurrentFloor);
        }
    }
}