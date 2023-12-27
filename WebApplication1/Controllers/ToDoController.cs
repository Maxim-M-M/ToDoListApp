using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


public class ToDoController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<ToDoController> _logger;

    public ToDoController(AppDbContext context, ILogger<ToDoController> logger)
    {
        _context = context;
        _logger = logger;
    }

    
    public IActionResult Read()
    {
       
        var activeTodos = _context.ToDos.Where(t => !t.IsCompleted).ToList();
        return View(activeTodos);



    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,DateCompleted,IsCompleted")] ToDo toDo)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Read));
            }
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error while saving todo item to the database.");
            ModelState.AddModelError("", "Could not save the record. Please try again.");
        }

        return View(toDo);
    }


    public IActionResult CompletedTasks()
    {
        var completedTasks = _context.ToDos.Where(t => t.IsCompleted).ToList();
        return View(completedTasks);
    }

    [HttpPost]
    public IActionResult MarkAsCompleted(int id)
    {
        var todoItem = _context.ToDos.Find(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.IsCompleted = !todoItem.IsCompleted;
        todoItem.DateClosed = DateTime.Now;

        _context.SaveChanges();

        return Json(new { success = true, isCompleted = todoItem.IsCompleted });
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var todo = _context.ToDos.Find(id);
        return View(todo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ToDo todo)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Update(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Read));
            }
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error while updating todo item in the database.");
            ModelState.AddModelError("", "Could not update the record. Please try again.");
        }

        return View(todo);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var todo = _context.ToDos.Find(id);

        if (todo == null)
        {
            return NotFound();
        }

        _context.ToDos.Remove(todo);

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Read));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error while deleting todo item from the database.");
            ModelState.AddModelError("", "Could not delete the record. Please try again.");
            return View("Delete", todo);
        }
    }



}
