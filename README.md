<h1 align="center">ConnectChain</h1>
<h3 align="center">ASP.NET Core Web API</h3>

<p align="center">
  <img src="https://readme-typing-svg.herokuapp.com?lines=ASP.NET+Core+Backend;CQRS+with+MediatR;In-Memory+Caching;Firebase+Cloud+Messaging&center=true&width=600&height=45">
</p>

<p align="center">
  <a href="https://connectchainapisdoc.apidog.io/" target="_blank">
    <img src="https://img.shields.io/badge/API%20Documentation-Apidog-blue?style=for-the-badge&logo=swagger&logoColor=white"/>
  </a>
</p>

---

## 🎯 Project Overview
**ConnectChain** is a backend-focused **B2B supply chain system** built to demonstrate
how modern **ASP.NET Core** applications can be structured using clean architectural
principles and clear separation of concerns.

The project is designed as a **backend-only system**, focusing on architecture,
performance, and maintainability rather than UI.

---

## 🧱 Architecture Overview

### Layer Responsibilities
- **Domain**
  - Core business entities
  - Business rules and invariants

- **Application**
  - CQRS Commands & Queries
  - MediatR Handlers
  - Interfaces and abstractions

- **Infrastructure**
  - Entity Framework Core
  - Database configuration
  - External services (FCM, ML.NET)

- **API**
  - HTTP endpoints
  - Request/response handling
  - Minimal logic (delegation only)

---

## 🔁 CQRS (Command Query Responsibility Segregation)
The project applies **CQRS** to clearly separate read and write operations:

- **Commands**
  - Handle state-changing operations
  - Encapsulate business logic

- **Queries**
  - Handle read-only operations
  - Optimized for data retrieval

Each command and query has its own **dedicated handler**, implemented using **MediatR**,
keeping controllers thin and focused.

---

## 🗃 Data Access

### Entity Framework Core
- Code-first approach
- Fluent API used for entity configuration
- Async database operations
- LINQ-based querying

EF Core is used exclusively inside the **Infrastructure layer**, fully isolated
from the API layer.

---

## ⚡ Caching Strategy

### In-Memory Caching (IMemoryCache)
- Implemented using `IMemoryCache`
- Applied to frequently accessed read operations
- Reduces database load and improves response time
- Suitable for non-distributed or single-instance environments

Caching logic is handled at the application level to keep controllers clean.

---

## 🧠 Machine Learning Integration (ML.NET)
- **ML.NET** is used to recommend suitable suppliers
- Model trained on historical supplier–category matching data
- ML logic is isolated from core business logic
- Improves matching accuracy without over-coupling the system

This demonstrates how ML can be integrated into a backend system
without polluting the domain layer.

---

## 🔔 Real-Time Notifications

### Firebase Cloud Messaging (FCM)
- Push notifications triggered on key business events
- Notification logic is decoupled from controllers
- Enables near real-time communication with clients

FCM integration is handled as an external service within the Infrastructure layer.

---

## 📄 API Documentation
Interactive API documentation is available via **Apidog**:

👉 **https://connectchainapisdoc.apidog.io/**

The documentation includes:
- All available endpoints
- Request & response schemas
- Authentication details
- Example payloads

---

## 🛠 Tech Stack

### Backend
<p>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg" width="36"/>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg" width="36"/>
</p>

- ASP.NET Core Web API  
- C#  
- MediatR  

---

### Persistence & Performance
<p>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" width="36"/>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/entityframework/entityframework-original.svg" width="36"/>
</p>

- SQL Server  
- Entity Framework Core  
- In-Memory Caching (`IMemoryCache`)  

---

### Architecture & Patterns
- CQRS  
- Repository Pattern  

---

### Integrations
<p>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/firebase/firebase-plain.svg" width="36"/>
</p>

- ML.NET  
- Firebase Cloud Messaging (FCM)

---

## 🧪 What This Project Demonstrates
- Structuring a real-world ASP.NET Core backend
- Using CQRS with MediatR
- Implementing in-memory caching responsibly
- Integrating ML and real-time notifications
- Writing maintainable and testable backend code
