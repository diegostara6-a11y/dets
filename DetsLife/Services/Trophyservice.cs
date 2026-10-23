using Dets.Services;

namespace DetsLife;

public class TrophyService
{
    private const string StorageKey = "trophydata";

    private readonly ISavingService _saving;
    private TrophyData? _data;

    public TrophyService(ISavingService saving) => _saving = saving;

    // ---------- Dati ----------

    public async Task<TrophyData> GetDataAsync()
    {
        if (_data is not null) return _data;

        _data = await _saving.LoadAsync<TrophyData>(StorageKey);

        if (_data is null || _data.Trophies.Count == 0)
        {
            _data = new TrophyData
            {
                Trophies = TrophyCatalog.Defaults(),
                StartDate = DateTime.Today
            };
            await SaveAsync();
        }

        return _data;
    }

    private Task SaveAsync() => _saving.SaveAsync(StorageKey, _data!);

    // ---------- Registrazione azioni ----------

    /// <summary>Da chiamare ogni volta che l'utente preme un'azione in Home.</summary>
    public async Task RecordAsync(string actionName, int value, int totalPoints)
    {
        var d = await GetDataAsync();
        var today = DateTime.Today;
        var key = DayKey(today);

        d.DailyPoints[key] = d.DailyPoints.GetValueOrDefault(key) + value;
        d.ActionCounts[actionName] = d.ActionCounts.GetValueOrDefault(actionName) + 1;
        d.LastDone[actionName] = today;
        d.MaxPoints = Math.Max(d.MaxPoints, totalPoints);

        await CheckCoreAsync(d, forceSave: true);
    }

    // ---------- Controllo trofei ----------

    /// <summary>
    /// Controlla se qualche trofeo è stato completato (serve anche solo per il passare del tempo,
    /// es. "30 giorni senza sigarette"). Si può chiamare quando si aprono le pagine.
    /// </summary>
    public async Task CheckAsync()
    {
        var d = await GetDataAsync();
        await CheckCoreAsync(d, forceSave: false);
    }

    private async Task CheckCoreAsync(TrophyData d, bool forceSave)
    {
        var stats = BuildStats(d);
        bool changed = forceSave;

        foreach (var t in d.Trophies)
        {
            // Serie interrotta / azione rifatta: si riparte da zero
            if (TrophyRules.Current(t, stats) < t.Baseline)
            {
                t.Baseline = 0;
                changed = true;
            }

            // while: se i punti saltano di tanto, può scattare più di una volta
            while (TrophyRules.ShouldEarn(t, stats))
            {
                t.TimesEarned++;
                t.Baseline += t.Target;
                t.LastEarnedAt = DateTime.Now;

                d.Alerts.Add(new TrophyAlert
                {
                    Name = t.Name,
                    Icon = t.Icon,
                    Tier = t.Tier,
                    TimesEarned = t.TimesEarned,
                    At = DateTime.Now
                });

                changed = true;
            }
        }

        if (changed) await SaveAsync();
    }

    // ---------- Statistiche ----------

    public async Task<Stats> GetStatsAsync() => BuildStats(await GetDataAsync());

    private static Stats BuildStats(TrophyData d)
    {
        var today = DateTime.Today;

        // Streak: giorni consecutivi con punteggio del giorno > 0.
        // Se oggi non hai ancora fatto nulla, la serie non è interrotta: si parte da ieri.
        var day = today;
        if (!d.DailyPoints.ContainsKey(DayKey(day)))
            day = day.AddDays(-1);

        int streak = 0;
        while (d.DailyPoints.TryGetValue(DayKey(day), out var pts) && pts > 0)
        {
            streak++;
            day = day.AddDays(-1);
        }

        // Giorni dall'ultima volta che hai fatto ogni azione
        // (se non l'hai mai fatta: dal primo giorno in cui hai usato i trofei)
        var since = new Dictionary<string, int>();
        foreach (var a in HabitCatalog.Actions)
        {
            var from = d.LastDone.TryGetValue(a.Name, out var last) ? last : d.StartDate;
            since[a.Name] = Math.Max(0, (today - from.Date).Days);
        }

        return new Stats(
            TotalPoints: d.MaxPoints,
            DaysTracked: d.DailyPoints.Count,
            CurrentStreak: streak,
            ActionCounts: d.ActionCounts,
            DaysSinceAction: since);
    }

    private static string DayKey(DateTime date) => date.ToString("yyyy-MM-dd");

    // ---------- Avvisi ----------

    public async Task<List<TrophyAlert>> GetAlertsAsync()
    {
        var d = await GetDataAsync();
        return d.Alerts.ToList();
    }

    public async Task DismissAlertAsync(Guid id)
    {
        var d = await GetDataAsync();
        d.Alerts.RemoveAll(a => a.Id == id);
        await SaveAsync();
    }
}