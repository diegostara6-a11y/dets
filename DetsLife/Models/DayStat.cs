namespace Dets.Models;

public class DayStat
{
    public DateTime Day { get; set; }
    public string Name { get; set; } = "";
    public int Count { get; set; }
    public int Points { get; set; }
}