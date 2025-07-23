namespace ToDoApi.Models;

public class ToDoItem
{
    public int Id { get; set; } // primary key
    public string? Title { get; set; } = String.Empty;
    public bool isComplete { get; set; }
}