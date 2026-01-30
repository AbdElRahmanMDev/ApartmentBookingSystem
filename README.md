
- **Domain** contains pure business rules with zero external dependencies.
- **Application** orchestrates use cases and workflows.
- **Infrastructure** provides persistence and external integrations.
- **API** exposes HTTP endpoints and middleware.

This protects the business logic from infrastructure changes.

---

## 🧱 Domain-Driven Design

### Aggregates
- Apartment
- Booking
- User

Each aggregate encapsulates business rules and enforces invariants.

### Entities
- Identity-based objects with lifecycle control.

### Value Objects
Immutable domain primitives that eliminate primitive obsession:
- Money
- Currency
- DateRange
- Address
- Name, Email

### Domain Events
- Domain changes emit events (e.g. UserCreatedDomainEvent)
- Enables future scalability toward event-driven architecture.

### Result Model
- Explicit success/failure modeling without exception-driven flow.

---

## 💾 Persistence Strategy (Hybrid)

### Entity Framework Core
- Aggregate persistence
- Change tracking and Unit of Work
- Fluent mappings
- Migrations
- PostgreSQL provider

### ADO.NET
- SqlConnectionFactory for optimized queries and projections
- Enables performance-oriented read scenarios.

This hybrid approach mirrors real production systems.

---

## 🔄 Transaction Management

- Unit of Work abstraction coordinates persistence.
- Atomic commits across aggregates.
- Prepared for extension with transactional messaging.

---

## 🌱 Database Lifecycle

### Migrations
- Schema versioning via EF Core migrations.

### Data Seeding
- Automatic seed execution on startup.
- Enables consistent local environments.

---

## 🧩 Dependency Injection

Each layer registers its own dependencies:
- Infrastructure registers persistence services.
- Application registers use cases.
- API composes the application.

Enforces inversion of control and modularity.

---

## 🌐 API Layer

- REST endpoints for booking workflows.
- DTO isolation from domain models.
- Custom middleware for request logging.
- Extension-based startup configuration.

---

## 🐳 Containerization

Docker Compose provides:

- API container
- PostgreSQL container

Benefits:
- Environment consistency
- Zero manual setup
- Easy onboarding

---

## ▶️ Running the Application

### Prerequisites

- Docker Desktop
- .NET 8 SDK

---

### Run with Docker

```bash
docker compose up --build
