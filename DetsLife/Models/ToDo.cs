namespace Dets.Models;

public class Todo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public int Day { get; set; } = DateTime.Today.Day;
    public int Month { get; set; } = DateTime.Today.Month;
    public int Year { get; set; } = DateTime.Today.Year;
    public int Difficulty { get; set; } = 1;
    public DateTime Date => new DateTime(Year, Month, Day);
}