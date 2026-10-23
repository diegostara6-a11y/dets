namespace DetsLife;

/// <summary>
/// Elenco condiviso delle azioni: usato da Home (punti) e da Goals (premio casuale).
/// </summary>
public static class HabitCatalog
{
    public static readonly List<HabitAction> Actions = new()
    {
        /* Cose negative */
        new() { Name = "Videogiochi", Description = "Giocare per più di 2 ore", Value = -15, Weight = 4 },
        new() { Name = "Scroll infinito", Description = "Più di un'ora sui social senza uno scopo", Value = -25, Weight = 3 },
        new() { Name = "Salto palestra", Description = "Saltare un allenamento programmato", Value = -20, Weight = 2 },
        new() { Name = "Acquisto impulsivo", Description = "Comprare qualcosa di non necessario", Value = -25, Weight = 1 },
        new() { Name = "Pasto fuori casa", Description = "Mangiare fuori con spesa minima di 5€", Value = -5, Weight = 5 },
        new() { Name = "Terea", Description = "Comprare un pacchetto di terea", Value = -55, Weight = 3 },
        new() { Name = "Sigarette", Description = "Comprare un pacchetto di sigarette", Value = -75, Weight = 2 },
        new() { Name = "Serate con alcol", Description = "Uscite di sera dove si beve con amici", Value = -35, Weight = 5 },

        /* Cose positive */
        new() { Name = "Palestra", Description = "Andare in palestra per almeno 1 ora", Value = 10, Weight = 0 },
        new() { Name = "Studiare", Description = "Studiare per almeno 1.5 ore", Value = 15, Weight = 0 },
        new() { Name = "Leggere un libro", Description = "Leggere per almeno 15 minuti", Value = 10, Weight = 0 },
        new() { Name = "Niente social", Description = "Usare i social per meno di 120 minuti", Value = 12, Weight = 0 },
        new() { Name = "Riordinare", Description = "Mettere in ordine camera o scrivania", Value = 5, Weight = 0 },
        new() { Name = "Aiuto in casa", Description = "Fare una faccenda senza che te la chiedano", Value = 5, Weight = 0 },
        new() { Name = "Tempo con la famiglia", Description = "Cenare o passare del tempo con i genitori o i fratelli", Value = 5, Weight = 0 },
        new() { Name = "Strumento musicale", Description = "Suonare o esercitarsi per almeno 20 minuti", Value = 8, Weight = 0 },
        new() { Name = "Progetto personale", Description = "Lavorare per 1.5 ore a un progetto tuo (app, disegno, video, scrittura)", Value = 15, Weight = 0 },
    };
}