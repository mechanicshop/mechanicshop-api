<div align="center">

# MechanicShop

## [🔗 Live Demo](https://mechanic-shop-client.purpleforest-454b82e9.swedencentral.azurecontainerapps.io/)

**Full-Stack Auto Repair Shop Management System**  
Built with **.NET 10** · **Angular 21 (SSR)** · **PostgreSQL** · **SignalR** · **Azure Container Apps**

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-21-DD0031?style=flat-square&logo=angular)](https://angular.dev/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-4169E1?style=flat-square&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)

> **⚠️ RED FLAG — Scale-to-Zero:** The live demo runs on Azure Container Apps with **scale-to-zero**. The first request after inactivity may take **up to 10 minutes** while both the API and Client containers cold-start. Please be patient.

</div>

---

## 📑 Table of Contents

- [🏗️ Architecture Overview](#️-architecture-overview)
- [✨ Key Features](#-key-features)
- [🛠️ Tech Stack](#️-tech-stack)
- [🚦 Getting Started](#-getting-started)
- [☁️ Infrastructure](#️-infrastructure)

---

## 🏗️ Architecture Overview

MechanicShop is designed using **Clean Architecture** principles with a strict dependency inversion flow. The deployment runs **two separate containers** on Azure Container Apps — one for the .NET API and one for the Angular SSR client.

```
┌────────────────────────────────────────────────────────┐
│                    Presentation                        │
│       API Controllers · Angular Client (SSR)           │
├────────────────────────────────────────────────────────┤
│                    Application                         │
│       Commands / Queries (CQRS) · MediatR              │
│   FluentValidation · Pipeline Behaviours               │
├────────────────────────────────────────────────────────┤
│                      Domain                            │
│     Entities · Value Objects · Result<T> Monad         │
├────────────────────────────────────────────────────────┤
│                   Infrastructure                       │
│   EF Core · PostgreSQL · Hybrid Cache · SignalR        │
│   OpenTelemetry · Serilog · QuestPDF (Invoices)        │
└────────────────────────────────────────────────────────┘
```

- **CQRS via MediatR** with pipeline behaviours for validation, caching, logging, and performance monitoring.
- **Result Pattern** — Enforces error handling as control flow using a custom `Result<T>` monad with strongly typed errors.
- **Angular SSR** — Server-side rendering for improved SEO and initial load performance.
- **SignalR** — Real-time push notifications for work order updates across connected clients.

---

## ✨ Key Features

- **🔐 Role-Based Access Control (RBAC)** — **Managers** (full CRUD, invoicing, dashboard) and **Labor** (work order state updates, daily schedule).
- **📋 Customer & Vehicle Management** — Register customers with multiple vehicles; search, sort, and filter.
- **🔧 Repair Task Catalog** — Predefined tasks with labor costs, estimated durations, and associated parts.
- **📝 Work Order Lifecycle** — Full state machine (`Scheduled → InProgress → Completed / Cancelled`) with labor assignment, spot allocation (A–D), and repair task grouping.
- **📅 Daily Scheduling** — View shop schedule with timezone support, conflict detection for spots, labor, and vehicles.
- **💰 Billing & Invoicing** — Issue invoices for completed work orders with auto-calculated totals, tax, discounts, and **PDF generation**.
- **📊 Dashboard** — Real-time KPIs: order status counts, revenue breakdown, profit margin, completion rate, and more.
- **⚡ Real-Time Updates** — SignalR-powered live push when work orders change.
- **🕐 Automatic Cleanup** — Background service cancels no-show bookings past the grace period.
- **🔒 Rate Limiting & Caching** — Sliding-window rate limiter, multi-tier hybrid cache (in-memory + PostgreSQL distributed).
- **📈 Observability** — Structured logging (Serilog), distributed tracing (OpenTelemetry), Prometheus metrics.

---

## 🛠️ Tech Stack

| Layer | Technologies |
|---|---|
| **Backend** | .NET 10, EF Core 10, ASP.NET Core Identity, MediatR, FluentValidation, SignalR, QuestPDF |
| **Frontend** | Angular 21 (SSR, Signals), NgRx Signal Store, Angular Material, Express |
| **Databases & Cache** | PostgreSQL 18, Hybrid Cache (PostgreSQL L2 backend) |
| **Observability** | OpenTelemetry, Serilog, Seq, Prometheus, Grafana Loki |
| **Infrastructure** | Docker, Docker Compose, Terraform, Azure Container Apps |

---

## 🚦 Getting Started

### ⚡ Quick Start (Docker)

```bash
git clone https://github.com/MoamenElbarqy/MechanicShop.git
cd MechanicShop
docker compose up -d
```

| Service | URL |
|---|---|
| 🌐 Web App | [`http://localhost:4200`](http://localhost:4200) |
| 📘 Swagger API | [`http://localhost:8080/swagger`](http://localhost:8080/swagger) |
| 📙 Scalar API | [`http://localhost:8080/scalar`](http://localhost:8080/scalar) |

### 🔐 Demo Credentials

The database is pre-seeded with the following accounts:

| Role | Email | Password |
|---|---|---|
| 👑 Manager | `pm@localhost` | `pm@localhost` |
| 🔧 Labor | `john.labor@localhost` | `john.labor@localhost` |
| 🔧 Labor | `peter.labor@localhost` | `peter.labor@localhost` |
| 🔧 Labor | `kevin.labor@localhost` | `kevin.labor@localhost` |
| 🔧 Labor | `suzan.labor@localhost` | `suzan.labor@localhost` |

> The manager account has full access to customers, work orders, repair tasks, invoicing, and dashboard. Labor accounts can view schedules and update work order states.

---

## ☁️ Infrastructure

The cloud infrastructure is fully declared with **Terraform** and deployed to **Azure Container Apps** in `swedencentral`:

- **Two Containers** — API (internal ingress) and Angular Client (external ingress, port 4000) run as separate container apps.
- **Scale-to-Zero** — Both services scale to `0` when idle, minimizing cost. Cold starts take ~10 minutes.
- **GitHub Container Registry** — Images are published to `ghcr.io/moamenelbarqy/mechanic-shop/`.
- **GitOps Lifecycle** — Terraform ignores image and environment changes, keeping infrastructure separate from application CI/CD.
