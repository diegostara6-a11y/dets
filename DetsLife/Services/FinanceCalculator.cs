using Dets.Models;

namespace Dets.Services;

/// <summary>
/// Calcoli di bilancio e punteggio risparmio. Nessuna dipendenza dalla UI.
/// Convenzioni:
///  - Amount è sempre positivo, il segno lo decide Type (Entrata / Spesa)
///  - le spese in categoria "Risparmio" NON contano come spese: sono soldi messi da parte
/// </summary>
public static class FinanceCalculator
{
    // Percentuale di risparmio che vale 100 punti su 100
    public const double TargetSavingsRate = 0.30;

    public static bool IsIncome(Transaction t) => t.Type == TransactionType.Entrata;

    public static bool IsSpending(Transaction t) =>
        t.Type == TransactionType.Spesa && t.Category != Category.Risparmio;

    public static decimal Income(IEnumerable<Transaction> tx) =>
        tx.Where(IsIncome).Sum(t => Math.Abs(t.Amount));

    public static decimal Expenses(IEnumerable<Transaction> tx) =>
        tx.Where(IsSpending).Sum(t => Math.Abs(t.Amount));

    /// <summary>Entrate - spese (include quindi i soldi messi in "Risparmio").</summary>
    public static decimal Balance(IEnumerable<Transaction> tx) => Income(tx) - Expenses(tx);

    public static List<(Category Category, decimal Total)> ExpensesByCategory(IEnumerable<Transaction> tx) =>
        tx.Where(IsSpending)
          .GroupBy(t => t.Category)
          .Select(g => (g.Key, g.Sum(t => Math.Abs(t.Amount))))
          .OrderByDescending(x => x.Item2)
          .ToList();

    /// <summary>Quota delle entrate che hai risparmiato (0.25 = 25%). Null se non ci sono entrate.</summary>
    public static double? SavingsRate(decimal income, decimal expenses)
    {
        if (income <= 0) return null;
        return (double)((income - expenses) / income);
    }

    /// <summary>
    /// Punteggio 0-100: 0 se non risparmi nulla (o sei in rosso),
    /// 100 se risparmi almeno il 30% delle entrate. Null se non ci sono entrate.
    /// </summary>
    public static int? Score(decimal income, decimal expenses)
    {
        var rate = SavingsRate(income, expenses);
        if (rate is null) return null;
        if (rate <= 0) return 0;
        return (int)Math.Round(Math.Min(rate.Value / TargetSavingsRate, 1.0) * 100);
    }

    public static string Label(int? score) => score switch
    {
        null => "Nessuna entrata registrata",
        >= 85 => "Maestro del risparmio",
        >= 65 => "Ottimo",
        >= 40 => "Nella media",
        >= 20 => "Da migliorare",
        _ => "Allarme rosso"
    };

    /// <summary>
    /// Punti "vita" da dare nello storico a fine mese: da -25 a +25.
    /// Score 50 = pareggio, sopra guadagni punti, sotto li perdi.
    /// </summary>
    public static int LifePoints(int? score) => score is null ? 0 : (score.Value - 50) / 2;
}
