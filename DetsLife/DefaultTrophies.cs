namespace DetsLife;

public static class DefaultTrophies
{
    private static Trophy T(string name, string desc, ConditionType cond, int target,
                            string icon, string? action = null) => new()
                            {
                                Name = name,
                                Description = desc,
                                Condition = cond,
                                Target = target,
                                Icon = icon,
                                ActionName = action
                            };

    // ATTENZIONE: i nomi in ActionName devono essere IDENTICI a HabitCatalog.Actions.
    public static List<Trophy> Create() => new()
    {
        // --- Punti totali (ogni X punti) ---
        T("Si parte",        "Raggiungi 100 punti",              ConditionType.TotalPoints, 100,  "⭐"),
        T("Mezzo migliaio",  "Raggiungi 500 punti",              ConditionType.TotalPoints, 500,  "🌟"),
        T("Quota mille",     "Raggiungi 1000 punti",             ConditionType.TotalPoints, 1000, "💫"),

        // --- Giorni di utilizzo ---
        T("Una settimana con me", "Usa l'app per 7 giorni",      ConditionType.DaysTracked, 7,   "📅"),
        T("Un mese insieme",      "Usa l'app per 30 giorni",     ConditionType.DaysTracked, 30,  "🗓️"),
        T("Cento giorni",         "Usa l'app per 100 giorni",    ConditionType.DaysTracked, 100, "🎉"),

        // --- Serie di giorni positivi ---
        T("Tre di fila",       "3 giorni consecutivi positivi",  ConditionType.Streak, 3,  "🔥"),
        T("Settimana perfetta","7 giorni consecutivi positivi",  ConditionType.Streak, 7,  "🔥"),
        T("Mese perfetto",     "30 giorni consecutivi positivi", ConditionType.Streak, 30, "🔥"),

        // --- Azioni ripetute ---
        T("Secchione",         "Studia 10 volte",                ConditionType.ActionCount, 10, "📚", "Studiare"),
        T("Mano ferma",        "Studia 50 volte",                ConditionType.ActionCount, 50, "✍️", "Studiare"),
        T("Topo di biblioteca","Leggi 10 volte",                 ConditionType.ActionCount, 10, "📖", "Leggere un libro"),
        T("Costanza",          "Vai in palestra 10 volte",       ConditionType.ActionCount, 10, "💪", "Palestra"),
        T("Casa in ordine",    "Riordina 10 volte",              ConditionType.ActionCount, 10, "🧹", "Riordinare"),
        T("Creatore",          "Lavora al tuo progetto 10 volte",ConditionType.ActionCount, 10, "🛠️", "Progetto personale"),

        // --- Giorni senza fare un'azione ---
        T("Respiro libero",    "3 giorni senza sigarette",       ConditionType.DaysWithout, 3,  "🚭", "Sigarette"),
        T("Settimana pulita",  "7 giorni senza sigarette",       ConditionType.DaysWithout, 7,  "🫁", "Sigarette"),
        T("Un mese senza fumo","30 giorni senza sigarette",      ConditionType.DaysWithout, 30, "🏅", "Sigarette"),
        T("Fiato leggero",     "7 giorni senza terea",           ConditionType.DaysWithout, 7,  "🌬️", "Terea"),
        T("Libero dalla terea","30 giorni senza terea",          ConditionType.DaysWithout, 30, "🏆", "Terea"),
        T("Sobrio",            "7 giorni senza serate con alcol",ConditionType.DaysWithout, 7,  "💧", "Serate con alcol"),
        T("Mente lucida",      "30 giorni senza serate con alcol",ConditionType.DaysWithout, 30, "🧠", "Serate con alcol"),
        T("Detox digitale",    "3 giorni senza scroll infinito", ConditionType.DaysWithout, 3,  "📵", "Scroll infinito"),
        T("Salute dei risparmi","14 giorni senza acquisti impulsivi", ConditionType.DaysWithout, 14, "💰", "Acquisto impulsivo"),
    };

    /// <summary>
    /// Allinea i trofei salvati con quelli predefiniti (per nome):
    /// aggiunge quelli mancanti e aggiorna descrizione, condizione, target, icona e ActionName
    /// dei già esistenti, SENZA toccare i progressi (TimesEarned, Baseline, LastEarnedAt).
    /// Ritorna true se qualcosa è cambiato (allora salva).
    /// </summary>
    public static bool EnsureDefaults(TrophyData data)
    {
        var changed = false;

        foreach (var d in Create())
        {
            var existing = data.Trophies.FirstOrDefault(t => t.Name == d.Name);

            if (existing is null)
            {
                data.Trophies.Add(d);
                changed = true;
                continue;
            }

            if (existing.Description != d.Description ||
                existing.Condition != d.Condition ||
                existing.Target != d.Target ||
                existing.Icon != d.Icon ||
                existing.ActionName != d.ActionName)
            {
                existing.Description = d.Description;
                existing.Condition = d.Condition;
                existing.Target = d.Target;
                existing.Icon = d.Icon;
                existing.ActionName = d.ActionName;
                changed = true;
            }
        }

        return changed;
    }
}