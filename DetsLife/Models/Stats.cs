namespace DetsLife;

public record Stats(
    int TotalPoints,
    int DaysTracked,
    int CurrentStreak,
    Dictionary<string, int> ActionCounts,
    Dictionary<string, int> DaysSinceAction);