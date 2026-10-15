namespace Dets.Models;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int Day { get; set; } = DateTime.Today.Day;
    public int Month { get; set; } = DateTime.Today.Month;
    public int Year { get; set; } = DateTime.Today.Year;

    public DateTime Date => new DateTime(Year, Month, Day);
}