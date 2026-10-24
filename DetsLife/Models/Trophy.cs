using System.Text.Json.Serialization;

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
    public Tier Tier { get; set; } = Tier.Bronze;     // legacy: non più usato dalla UI (vedi CurrentTier)
    public string Icon { get; set; } = "🏆";           // emoji di fallback
    public string? IconFile { get; set; }              // opzionale: forza un'immagine specifica (es. "crown.svg")

    // Condizione
    public ConditionType Condition { get; set; }
    public int Target { get; set; }
    public string? ActionName { get; set; }

    // Stato
    public int TimesEarned { get; set; }          // quante volte l'hai ottenuta
    public int Baseline { get; set; }             // valore già "consumato" dalle volte precedenti
    public DateTime? LastEarnedAt { get; set; }

    [JsonIgnore]
    public bool IsEarned => TimesEarned > 0;

    // ---------- IDEA 2: il metallo sale con le volte ottenute ----------
    // Modifica le soglie come preferisci.
    [JsonIgnore]
    public Tier CurrentTier => TimesEarned switch
    {
        >= 8 => Tier.Diamond,
        >= 5 => Tier.Platinum,
        >= 3 => Tier.Gold,
        >= 2 => Tier.Silver,
        _ => Tier.Bronze
    };

    // Classe CSS della medaglia: grigia finché non ottenuta, poi il colore del tier.
    [JsonIgnore]
    public string TierCss => IsEarned
        ? "tier-" + CurrentTier.ToString().ToLowerInvariant()
        : "tier-locked";

    // ---------- IDEA 1: la forma dipende dal tipo di obiettivo ----------
    // I file stanno in wwwroot/images/trophies/. Cambia qui i nomi per usare le tue coccarde.
    [JsonIgnore]
    public string BadgeFile => !string.IsNullOrWhiteSpace(IconFile)
        ? IconFile!
        : Condition switch
        {
            ConditionType.TotalPoints => "star.svg",
            ConditionType.DaysTracked => "calendar.svg",
            ConditionType.Streak => "flame.svg",
            ConditionType.ActionCount => "target.svg",
            ConditionType.DaysWithout => "shield.svg",
            _ => "medal.svg"
        };
}

/// <summary>Avviso "trofeo ottenuto" da mostrare nella pagina "/" finché non viene chiuso.</summary>
public class TrophyAlert
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "🏆";
    public string? BadgeFile { get; set; }        // copia da Trophy.BadgeFile quando crei l'alert
    public Tier Tier { get; set; }                // copia da Trophy.CurrentTier quando crei l'alert
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