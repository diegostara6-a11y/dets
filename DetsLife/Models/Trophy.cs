namespace DetsLife;

public enum Tier { Bronze, Silver, Gold, Platinum, Diamond }

public enum ConditionType
{
    TotalPoints,    // ogni X punti raggiunti
    DaysTracked,    // ogni X giorni in cui usi l'app
    Streak,         // X giorni consecutivi con punteggio del giorno positivo
    ActionCount,    // ogni X volte che fai una certa azione
    DaysWithout     // ogni X giorni senza fare una certa azione
}

public class Trophy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    // Coccarda
    public Tier Tier { get; set; } = Tier.Bronze;
    public string Icon { get; set; } = "🏆";

    // Condizione
    public ConditionType Condition { get; set; }
    public int Target { get; set; }
    public string? ActionName { get; set; }

    // Stato
    public int TimesEarned { get; set; }          // quante volte l'hai ottenuta
    public int Baseline { get; set; }             // valore già "consumato" dalle volte precedenti
    public DateTime? LastEarnedAt { get; set; }
    public bool IsEarned => TimesEarned > 0;
}

/// <summary>Avviso "trofeo ottenuto" da mostrare nella pagina "/" finché non viene chiuso.</summary>
public class TrophyAlert
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "🏆";
    public Tier Tier { get; set; }
    public int TimesEarned { get; set; }
    public DateTime At { get; set; } = DateTime.Now;
}

/// <summary>Tutto ciò che viene salvato su disco per i trofei.</summary>
public class TrophyData
{
    public List<Trophy> Trophies { get; set; } = new();
    public Dictionary<string, int> ActionCounts { get; set; } = new();
    public Dictionary<string, DateTime> LastDone { get; set; } = new();
    public Dictionary<string, int> DailyPoints { get; set; } = new();   // chiave "yyyy-MM-dd"
    public int MaxPoints { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today;
    public List<TrophyAlert> Alerts { get; set; } = new();
}