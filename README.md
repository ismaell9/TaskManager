# TaskManager
# 🗂️ TaskManager ASP.NET Core MVC App  A simple and clean task management web app using ASP.NET Core MVC with:  
- 🔐 JWT Authentication 
- 📝 Task CRUD 
- 📊 Task statuses: To Do, In Progress, Done 
- 👥 User profiles
- 🧼 Bootstrap 5 UI

  # TaskManager - ASP.NET Core MVC Project

TaskManager is a full-featured task management web application built using ASP.NET Core MVC, Entity Framework Core, and SQL Server.

---

## 🚀 Features

- ✅ JWT Authentication with session-based cookies
- ✅ Login, Register, and Profile Management
- ✅ Task CRUD (Create, Read, Update, Delete)
- ✅ Task filtering by user and status
- ✅ Responsive Bootstrap UI with card-based task layout
- ✅ Confirm delete modal
- ✅ Protected routes via `[Authorize]`
- ✅ Initial database seeding

---

## 🛠️ Technologies

- ASP.NET Core MVC (.NET 8+)
- Entity Framework Core
- SQL Server
- Bootstrap 5
- JWT (JSON Web Token)

---

## 📦 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/ismaell9/TaskManager.git
cd TaskManager
2. Configure the database
Update appsettings.json with your SQL Server connection string:

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TaskBoardDB;User ID=YourSqlUser;Password=YourSqlPassword;Trusted_Connection=True;TrustServerCertificate=True;"
}
3. Apply migrations and seed data
Run the following commands:

bash
Copy
Edit
dotnet ef database update
Seeding is handled automatically on application startup (see below).

4. Run the app
bash
Copy
Edit
dotnet run
Open your browser and navigate to https://localhost:5001.

👤 Default Users (Seeded)
Email	  |   Password 
alice@example.com	  |   Pass@123	
bob@example.com	  |   Pass@123	

Each user has 3 tasks pre-created with status:

To Do

In Progress

Done

🧪 Testing
Login as one of the users above

Create, edit, delete tasks

Try viewing profile and task filtering

Only view your own tasks


