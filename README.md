# 💍 Wedding Planner

A full-stack wedding planning application built with **ASP.NET Core Web API** and **Angular**.  
The app helps wedding organizers manage weddings and their related tasks in a clear and user-friendly way.

---

## ✨ Features

### Weddings
- Create, edit, and delete weddings
- View wedding details (title, date, location)
- Wedding status:
  - **Planned** – no tasks yet
  - **In Progress** – tasks exist but not all completed
  - **Completed** – all tasks completed
- Visual status indicators in the UI

### Tasks
- Add tasks to a wedding
- Mark tasks as completed
- Delete tasks
- Tasks are always linked to a specific wedding

---

## 🧠 Business Logic

- Wedding status is derived from its tasks:
  - No tasks → `Planned`
  - Some tasks incomplete → `InProgress`
  - All tasks completed → `Completed`
- Completed weddings are treated as read-only in the UI

> ⚠️ **Note:** In a real-world scenario, marking a wedding as `Completed` should be a user-confirmed action rather than an automatic effect. This is planned as a future improvement.

---

## 🛠️ Tech Stack

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- RESTful architecture

### Frontend
- Angular (standalone components)
- Angular Router
- HttpClient
- Basic custom CSS for UI polish

---

## 🚀 Getting Started

### Backend
1. Open the solution in Visual Studio
2. Run database migrations
3. Start the API

### Frontend
1. Navigate to the Angular project folder
2. Run:
   ```bash
   ng serve
3. Open http://localhost:4200

📌 Future Improvements
 - User confirmation before marking a wedding as completed
 - Archive section for completed weddings
 - Authentication & authorization
 - Unit and integration tests
 - Improved UI/UX styling

 👩‍💻 Author
 - Built as a learning project to practice full-stack development with ASP.NET Core and Angular.
