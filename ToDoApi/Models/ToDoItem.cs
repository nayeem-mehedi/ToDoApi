namespace ToDoApi.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool isComplete { get; set; }
}