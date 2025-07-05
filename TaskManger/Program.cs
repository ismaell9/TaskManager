using Microsoft.EntityFrameworkCore;
using System;
using TaskManger.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManger.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();

var key = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };

    // Read token from cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse(); 
            context.Response.Redirect("/Auth/Login");
            return Task.CompletedTask;
        }
    };
});


var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");



using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<TaskManagerContext>();

        context.Database.Migrate();

        if (!context.Users.Any())
        {
            var user1 = new User { Email = "alice@example.com", Username = "Alice", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass@123"), CreatedAt = DateTime.UtcNow };
            var user2 = new User { Email = "bob@example.com", Username = "Bob", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass@123"), CreatedAt = DateTime.UtcNow };

            context.Users.AddRange(user1, user2);
            context.SaveChanges();

            var tasks = new List<TaskItem>
        {
            new TaskItem { Title = "Alice Task 1", Description = "To Do", Status = "To Do", UserId = user1.Id, DueDate = DateTime.UtcNow.AddDays(3) },
            new TaskItem { Title = "Alice Task 2", Description = "In Progress", Status = "In Progress", UserId = user1.Id, DueDate = DateTime.UtcNow.AddDays(4) },
            new TaskItem { Title = "Alice Task 3", Description = "Done", Status = "Done", UserId = user1.Id, DueDate = DateTime.UtcNow.AddDays(5) },

            new TaskItem { Title = "Bob Task 1", Description = "To Do", Status = "To Do", UserId = user2.Id, DueDate = DateTime.UtcNow.AddDays(3) },
            new TaskItem { Title = "Bob Task 2", Description = "In Progress", Status = "In Progress", UserId = user2.Id, DueDate = DateTime.UtcNow.AddDays(4) },
            new TaskItem { Title = "Bob Task 3", Description = "Done", Status = "Done", UserId = user2.Id, DueDate = DateTime.UtcNow.AddDays(5) },
        };

            context.TaskItems.AddRange(tasks);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database seeding failed: " + ex.Message);
    }
}
app.Run();
