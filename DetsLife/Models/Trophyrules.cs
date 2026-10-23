namespace DetsLife;

public static class TrophyRules
{
    /// <summary>Valore attuale della statistica collegata al trofeo.</summary>
    public static int Current(Trophy t, Stats s) => t.Condition switch
    {
        ConditionType.TotalPoints => s.TotalPoints,
        ConditionType.DaysTracked => s.DaysTracked,
        ConditionType.Streak => s.CurrentStreak,
        ConditionType.ActionCount => s.ActionCounts.GetValueOrDefault(t.ActionName ?? "", 0),
        ConditionType.DaysWithout => s.DaysSinceAction.GetValueOrDefault(t.ActionName ?? "", 0),
        _ => 0
    };

    /// <summary>Avanzamento verso la PROSSIMA volta (toglie quanto già consumato).</summary>
    public static int Progress(Trophy t, Stats s) =>
        Math.Max(0, Current(t, s) - t.Baseline);

    public static double Percent(Trophy t, Stats s) =>
        t.Target <= 0 ? 1 : Math.Min(1.0, (double)Progress(t, s) / t.Target);

    public static bool ShouldEarn(Trophy t, Stats s) =>
        t.Target > 0 && Progress(t, s) >= t.Target;
}