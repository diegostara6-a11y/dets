using Dets.Models;

namespace Dets.Services;

public class HistoryService
{
    private const string Key = "history";
    private const string ArchiveKey = "history_archive";
    private const int KeepDays = 90;

    private readonly ISavingService saving;
    private List<HistoryEntry> recent = new();
    private List<DayStat> archive = new();
    private bool loaded;

    public HistoryService(ISavingService saving)
    {
        this.saving = saving;
    }

    private async Task EnsureLoadedAsync()
    {
        if (loaded) return;
        recent = await saving.LoadAsync<List<HistoryEntry>>(Key) ?? new();
        archive = await saving.LoadAsync<List<DayStat>>(ArchiveKey) ?? new();
        loaded = true;
        await CompactAsync();
    }

    // Sposta gli eventi più vecchi di KeepDays nell'archivio giornaliero
    private async Task CompactAsync()
    {
        var cutoff = DateTime.Today.AddDays(-KeepDays);
        var old = recent.Where(e => e.Date < cutoff).ToList();
        if (old.Count == 0) return;

        foreach (var g in old.GroupBy(e => (Day: e.Date.Date, e.Name)))
        {
            var existing = archive.FirstOrDefault(a => a.Day == g.Key.Day && a.Name == g.Key.Name);
            if (existing is null)
            {
                archive.Add(new DayStat
                {
                    Day = g.Key.Day,
                    Name = g.Key.Name,
                    Count = g.Count(),
                    Points = g.Sum(e => e.Points)
                });
            }
            else
            {
                existing.Count += g.Count();
                existing.Points += g.Sum(e => e.Points);
            }
        }

        recent.RemoveAll(e => e.Date < cutoff);
        await saving.SaveAsync(ArchiveKey, archive);
        await saving.SaveAsync(Key, recent);
    }

    public async Task AddAsync(string name, int points)
    {
        await EnsureLoadedAsync();
        recent.Add(new HistoryEntry { Name = name, Points = points });
        await saving.SaveAsync(Key, recent);
    }

    // Eventi dettagliati (ultimi 90 giorni)
    public async Task<List<HistoryEntry>> GetRecentAsync()
    {
        await EnsureLoadedAsync();
        return recent.ToList();
    }

    // Statistiche per giorno e azione: archivio + eventi recenti
    public async Task<List<DayStat>> GetDailyAsync()
    {
        await EnsureLoadedAsync();
        var fromRecent = recent
            .GroupBy(e => (Day: e.Date.Date, e.Name))
            .Select(g => new DayStat
            {
                Day = g.Key.Day,
                Name = g.Key.Name,
                Count = g.Count(),
                Points = g.Sum(e => e.Points)
            });
        return archive.Concat(fromRecent).ToList();
    }

    public async Task ClearAsync()
    {
        recent = new();
        archive = new();
        loaded = true;
        await saving.DeleteAsync(Key);
        await saving.DeleteAsync(ArchiveKey);
    }

    // Elimina un evento recente e sottrae i suoi punti dal totale salvato
    public async Task<bool> RemoveAsync(Guid id)
    {
        await EnsureLoadedAsync();
        var entry = recent.FirstOrDefault(e => e.Id == id);
        if (entry is null) return false;

        recent.Remove(entry);
        await saving.SaveAsync(Key, recent);

        int total = await saving.LoadAsync<int>("points");
        await saving.SaveAsync("points", total - entry.Points);
        return true;
    }
}