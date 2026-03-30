# 🧩 Order System Microservices

This project demonstrates a simple **microservices architecture** built with .NET, composed of two independent services:

* **Order Service** → manages orders (SQLite)
* **Inventory Service** → manages product stock (Redis)

The services communicate asynchronously using **RabbitMQ** and **MassTransit**.

---

## 🚀 Architecture Overview

```
[ Client / API ]
        ↓
[ Order Service ] ── publishes events ──▶ 🐇 RabbitMQ
                                                ↓
                                   [ Inventory Service ]
                                                ↓
                                             Redis
```

---

## 🛠️ Technologies Used

* .NET (ASP.NET Core Minimal APIs)
* SQLite (Order persistence)
* Redis (Inventory storage)
* RabbitMQ (message broker)
* MassTransit (message bus abstraction)
* Docker (optional for infrastructure)

---

## 📦 Microservices

### 📌 Order Service

* Responsible for:

  * Creating orders
  * Managing order lifecycle
* Database: **SQLite**
* Publishes events:

  * `OrderCreatedEvent`

#### Endpoints

* `POST /orders/createOrder`
* `GET /orders/confirmOrder/{id}`

---

### 📌 Inventory Service

* Responsible for:

  * Checking stock availability
  * Reserving stock
* Database: **Redis**
* Consumes events:

  * `OrderCreatedEvent`

#### Endpoints

* `POST /inventory/can-reserve`
* `POST /inventory/reserve`

---

## 🔄 Communication

Services communicate via **event-driven architecture**:

1. Order Service publishes `OrderCreatedEvent`
2. RabbitMQ routes the message
3. Inventory Service consumes the event
4. Inventory checks and reserves stock

---

## 📁 Project Structure

```
OrderSystem/
└── Services/
    ├── Order/
    │   ├── Order.Application
    │   ├── Order.Infrastructure
    │   └── Order.Presentation
	    └── Order.Domain
    │
    ├── Inventory/
    │   ├── Inventory.Application
    │   ├── Inventory.Infrastructure
    │   └── Inventory.Presentation
	    └── Inventory.Domain
    │
    └── Contracts/
        ├── Events/
        └── DTOs/
		└── Common/
```

---

## 🧪 Testing the Flow

1. Call Order API:

```http
POST /orders/createOrder
```

2. Order Service publishes event

3. Inventory Service:

   * Receives event
   * Checks stock
   * Reserves stock in Redis

4. Verify:

   * RabbitMQ queue activity
   * Redis updated values

---

## 📊 RabbitMQ Monitoring

Access RabbitMQ UI:

```
http://localhost:15672
```

Check:

* Queues (e.g., `inventory-order-created-queue`)
* Message rates
* Consumers

---

## 💡 Key Concepts Demonstrated

* Microservices architecture
* Event-driven communication
* Loose coupling via messaging
* Clean Architecture (Domain / Application / Infrastructure)
* Separation of concerns
* Independent data stores

---

## ⚠️ Notes

* Each microservice owns its own database
* No direct database sharing between services
* Communication is asynchronous via RabbitMQ

---

## 🚀 Future Improvements
* Add **API Gateway**
* Introduce **authentication/authorization**
* Add **logging & monitoring (OpenTelemetry, Serilog)**

---

## 👨‍💻 Author

Built as a learning project for microservices architecture with .NET.
