namespace DetsLife;

/// <summary>
/// Trofei di partenza. I nomi in ActionName devono coincidere con quelli di HabitCatalog.
/// Vengono creati solo la prima volta: dopo sono salvati su disco.
/// </summary>
public static class TrophyCatalog
{
    public static List<Trophy> Defaults() => new()
    {
        new() { Name = "Fedelissimo", Description = "Usa l'app per 7 giorni", Tier = Tier.Bronze, Icon = "📅",
                Condition = ConditionType.DaysTracked, Target = 7 },

        new() { Name = "Palestrato", Description = "Vai in palestra 10 volte", Tier = Tier.Bronze, Icon = "💪",
                Condition = ConditionType.ActionCount, ActionName = "Palestra", Target = 10 },

        new() { Name = "Secchione", Description = "Studia 20 volte", Tier = Tier.Silver, Icon = "🎓",
                Condition = ConditionType.ActionCount, ActionName = "Studiare", Target = 20 },

        new() { Name = "Topo di biblioteca", Description = "Leggi 15 volte", Tier = Tier.Silver, Icon = "📚",
                Condition = ConditionType.ActionCount, ActionName = "Leggere un libro", Target = 15 },

        new() { Name = "Costanza", Description = "7 giorni di fila con punteggio positivo", Tier = Tier.Gold, Icon = "🔥",
                Condition = ConditionType.Streak, Target = 7 },

        new() { Name = "Centurione", Description = "Raggiungi 100 punti", Tier = Tier.Gold, Icon = "💯",
                Condition = ConditionType.TotalPoints, Target = 100 },

        new() { Name = "Mese pulito", Description = "30 giorni senza sigarette", Tier = Tier.Gold, Icon = "🚭",
                Condition = ConditionType.DaysWithout, ActionName = "Sigarette", Target = 30 },

        new() { Name = "Sobrio", Description = "30 giorni senza serate con alcol", Tier = Tier.Platinum, Icon = "🥤",
                Condition = ConditionType.DaysWithout, ActionName = "Serate con alcol", Target = 30 },

        new() { Name = "Leggenda", Description = "Raggiungi 500 punti", Tier = Tier.Diamond, Icon = "👑",
                Condition = ConditionType.TotalPoints, Target = 500 },
    };
}