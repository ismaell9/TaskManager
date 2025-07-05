# TaskManager
# 🗂️ TaskManager ASP.NET Core MVC App  A simple and clean task management web app using ASP.NET Core MVC with:  
- 🔐 JWT Authentication 
- 📝 Task CRUD 
- 📊 Task statuses: To Do, In Progress, Done 
- 👥 User profiles
- 🧼 Bootstrap 5 UI

🚀 Features

- ✅ JWT Authentication with session-based cookies
- ✅ Login, Register, and Profile Management
- ✅ Task CRUD (Create, Read, Update, Delete)
- ✅ Task filtering by user and status
- ✅ Responsive Bootstrap UI with card-based task layout
- ✅ Confirm delete modal
- ✅ Protected routes via `[Authorize]`
- ✅ Initial database seeding

🛠️ Technologies

- ASP.NET Core MVC (.NET 8+)
- Entity Framework Core
- SQL Server
- Bootstrap 5
- JWT (JSON Web Token)

---

## 📦 Getting Started

### 1. Clone the repository

git clone https://github.com/ismaell9/TaskManager.git

### 2. Configure the database
Update appsettings.json with your SQL Server connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TaskBoardDB;User ID=YourSqlUser;Password=YourSqlPassword;Trusted_Connection=True;TrustServerCertificate=True;"
}
### 3. Apply migrations and seed data
Run the following command:

dotnet ef database update


Seeding is handled automatically on application startup.

### 4. Run the app

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
1. Login as one of the users above
2. Create, edit, delete tasks
3. Try viewing profile and task filtering
Note: you can Only view your own tasks


