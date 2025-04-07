# Slot Machine Application Assessment 🎰

## Project Overview

This is a .NET-based slot machine application that demonstrates modern software development practices and architectural patterns. The solution is structured to showcase clean architecture principles and domain-driven design.

## Technical Assessment Components

### Architecture & Design
- **Clean Architecture Implementation**
  - Domain-driven design principles
  - Clear separation of concerns
  - Dependency injection pattern
  - Repository pattern for data access

### Project Structure Analysis
- `Blazesoft.SlotMachine.Api`
  - RESTful API implementation
  - Controller organization
  - Request/Response handling
  - Middleware configuration

- `Blazesoft.SlotMachine.Domain`
  - Core business logic
  - Domain models and entities
  - Business rules and validation
  - Interface definitions

- `Blazesoft.SlotMachine.Common`
  - Shared utilities
  - Cross-cutting concerns
  - Common interfaces
  - Extension methods

- `Blazesoft.SlotMachine.Tests`
  - Unit test coverage
  - Integration tests
  - Test data generation
  - Mock implementations

### Technical Stack Assessment
- .NET 8.0
- MongoDB 7.0
- REST API Architecture
- Clean Architecture Pattern
- Domain-Driven Design

### Features Implementation Status
- ✅ Core Slot Machine Logic
- ✅ MongoDB Integration
- ✅ API Endpoints
- ✅ Basic Testing
- ⚠️ Authorization (Comment Out)
- ⚠️ State Management (Interface Only)

## Setup Instructions

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022
- Git
- MongoDB Community Server 7.0

### Environment Setup
1. Clone the repository:
   ```bash
   git clone [repository-url]
   ```

2. MongoDB Configuration:
   - Install MongoDB Community Server
   - Configure service: `net start MongoDB`
   - Verify connection using MongoDB Compass
   - Default connection string: `mongodb://localhost:27017`

3. Solution Setup:
   - Open `SlotMachine.sln` in Visual Studio
   - Restore NuGet packages
   - Build solution (F6)
   - Set `Blazesoft.SlotMachine.Api` as startup project

### Running Tests

#### Unit Tests
1. Open Test Explorer in Visual Studio (Test > Test Explorer)
2. Select the test project `Blazesoft.SlotMachine.Tests`
3. Click "Run All Tests" or select specific test categories:
   - Game Logic Tests
   - Repository Tests
   - Service Tests

#### Integration Tests
1. Ensure MongoDB is running
2. In Test Explorer, look for tests under the "Integration" category
3. Run integration tests separately from unit tests

### Testing the API

#### Using Swagger UI
1. Run the API project
2. Navigate to `https://localhost:5001/swagger` (or your configured port)
3. Available endpoints:
   - `POST /api/machine/spin` - Spin the machine
   - `POST /api/machine/update-balance` - Get current game state
   - `POST /api/machine/player` - Create a new player

#### Using Postman/curl
1. Create a new player:
   ```bash
   curl -X POST "https://localhost:5001/api/player/create" \
   -H "Content-Type: application/json" \
   -d '{"name": "TestPlayer", "initialBalance": 1000}'
   ```

2. Play a game:
   ```bash
   curl -X POST "https://localhost:5001/api/game/play" \
   -H "Content-Type: application/json" \
   -d '{"playerId": "player-id", "betAmount": 10}'
   ```

3. Check player state:
   ```bash
   curl -X GET "https://localhost:5001/api/player/player-id"
   ```

#### Test Cases to Verify
1. Player Creation
   - Create player with valid data
   - Verify balance is set correctly
   - Check for duplicate player handling

2. Game Play
   - Place valid bet
   - Verify win/loss calculation
   - Check balance updates
   - Test maximum bet limits
   - Verify game state persistence

3. Error Handling
   - Invalid bet amounts
   - Non-existent player
   - Insufficient balance
   - Invalid game state

## Technical Evaluation Points

### Code Quality
- Clean code principles
- SOLID principles implementation
- Design patterns usage
- Error handling approach

### Testing Strategy
- Unit test coverage
- Integration test implementation
- Test data management
- Mocking strategy

### Database Implementation
- MongoDB integration
- Data access patterns
- Connection management
- Query optimization

### Future Considerations
- Authorization implementation
- State management system
- Performance optimization
- Scalability improvements

## Assessment Notes
- The project demonstrates strong adherence to clean architecture principles
- MongoDB integration follows best practices
- Testing coverage could be expanded
- Some features are currently interface-only and need implementation

## License
[Add your license information here]

