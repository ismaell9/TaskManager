using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManger.Models;

[Authorize]
public class TaskController : Controller
{
    private readonly TaskManagerContext _context;

    public TaskController(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string search, string status)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);
        var query = _context.TaskItems.Where(x=>x.UserId == userId).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t =>
                t.Title.Contains(search) ||
                t.Description.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(status) && status != "All")
        {
            query = query.Where(t => t.Status == status);
        }

        var tasks = await query.OrderBy(t => t.DueDate).ToListAsync();

        // Pass search/status to the view for keeping selected values
        ViewBag.Search = search;
        ViewBag.Status = status;

        return View(tasks);
    }


    // GET: /Task/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Task/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItem task)
    {
        ModelState.Remove(nameof(task.UserId));
        ModelState.Remove(nameof(task.User));
        if (ModelState.IsValid)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            task.UserId = int.Parse(userIdClaim.Value);
            task.CreatedAt = DateTime.UtcNow;

            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(task);
    }


    // GET: /Task/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var task = await _context.TaskItems.FindAsync(id);
        if (task == null) return NotFound();

        return View(task);
    }

    // POST: /Task/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskItem task)
    {
        if (id != task.Id) return NotFound();

        ModelState.Remove(nameof(task.UserId));
        ModelState.Remove(nameof(task.User));
        if (ModelState.IsValid)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();

                task.UserId = int.Parse(userIdClaim.Value);
                task.UpdatedAt = DateTime.Now;
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TaskItems.Any(e => e.Id == task.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(task);
    }


    // GET: /Task/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var task = await _context.TaskItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (task == null) return NotFound();

        return View(task);
    }

    // POST: /Task/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task != null)
        {
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

}
