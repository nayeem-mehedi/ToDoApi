using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private static readonly List<ToDoItem> todos = new()
    {
        new ToDoItem { Id = 1, Title = "Learn ASP .NET", isComplete = false }
    };

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItem>> yoyo()
    {
        return Ok(todos);
    }


    [HttpGet("{id}")]
    public ActionResult<ToDoItem> Get(int id)
    {
        var todo = todos.FirstOrDefault(t => t.Id == id);
        if (todo == null) return NotFound();

        return Ok(todo);
    }


    [HttpPost]
    public ActionResult<ToDoItem> Create(ToDoItem item)
    {
        item.Id = todos.Count + 1;
        todos.Add(item);

        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }


    [HttpPut("{id}")]
    public ActionResult<ToDoItem> Update(int id, ToDoItem updatedItem)
    {
        var index = todos.FindIndex(t => t.Id == id);
        if (index == -1) return NotFound();

        todos[index] = updatedItem;
        return Ok(updatedItem);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = todos.FirstOrDefault(t => t.Id == id);
        if (item == null) return NotFound();

        todos.Remove(item);
        return NoContent();
    }
}