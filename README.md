# ElevatorSimulation

## Description

**ElevatorSimulation** is a C#-based simulation project designed to model a real-world elevator control system. The application simulates the behavior of multiple elevators (passenger and freight types) responding to floor requests, managing passenger capacity, and ensuring that the closest available elevator is chosen to fulfill the request. This project is ideal for understanding how a basic elevator system works, as well as demonstrating object-oriented programming concepts like inheritance, interfaces, and unit testing in C#.

### Key Features:
- **Request Elevators**: Passengers can request elevators, and the system will move the selected elevator to the requested floor.
- **Closest Elevator Selection**: When multiple elevators are available, the system chooses the closest elevator based on the current position relative to the requested floor.
- **Elevator Capacity**: Supports managing passenger and freight elevators, ensuring that each elevator can carry a specific number of passengers.
- **Queue and Process Requests**: You can add floor requests without immediate movement. The system will queue them, and then process all requests at once, ensuring elevators respond to each request in sequence.
- **Automated Testing**: The project includes a suite of unit tests to ensure functionality works as expected using **Moq** and **XUnit**.

## Technologies Used
- **C#** (Core programming language)
- **Moq** (Mocking framework for unit tests)
- **XUnit** (Unit testing framework)
- **GitHub Actions** (Continuous Integration/Continuous Deployment)
- **.NET 6** (Long-Term Support)

## Prerequisites

To run or contribute to this project, ensure the following tools are installed:
- **.NET 6 SDK** (LTS version) - [Download .NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6)
- **Visual Studio** (or any IDE supporting C#)
- **Git** - Version control system for cloning and pushing changes

## Setup

### 1. Clone the Repository

To get started, clone the repository to your local machine:

```bash
git clone https://github.com/szarath/ElevatorSimulation.git
cd ElevatorSimulation
