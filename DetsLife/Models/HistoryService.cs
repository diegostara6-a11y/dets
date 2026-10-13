public class HistoryEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Date { get; set; } = DateTime.Now;
    public string Name { get; set; } = "";
    public int Points { get; set; }
}