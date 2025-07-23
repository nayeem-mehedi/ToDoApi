using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    
    private readonly TodoDbContext _context;

    public TodoController(TodoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAll()
    {
        return await _context.ToDoItems.ToListAsync();
        // use .AsNoTracking() for read only DB queries
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItem>> Get(int id)
    {
        var todo = await _context.ToDoItems.FindAsync(id);
        return todo == null ? NotFound() : Ok(todo);
    }


    [HttpPost]
    public async Task<ActionResult<ToDoItem>> Create(ToDoItem item)
    {
        // _context.ToDoItems.AddAsync() // useful when SeqHiLo as ID or custom DB-based ID generator

        _context.ToDoItems.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<ToDoItem>> Update(int id, ToDoItem updatedItem)
    {
        if (id != updatedItem.Id) return BadRequest();

        _context.Entry(updatedItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(updatedItem);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<ToDoItem>> MarkComplete(int id)
    {
        var todoItem = await _context.ToDoItems.FindAsync(id);
        if (todoItem == null) return NotFound();
        
        todoItem.isComplete = true;
        await _context.SaveChangesAsync();
        
        return Ok(todoItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null) return NotFound();

        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}