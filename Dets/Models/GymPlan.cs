namespace Dets.Models;

public class GymExercise
{
    public string Exercise { get; set; } = "";
    public TimeSpan Break { get; set; }        // pausa tra le serie
    public int MinReps { get; set; }
    public int MaxReps { get; set; }
    public int Sets { get; set; }
    public string Type { get; set; } = "";
}

public class GymPlan
{
    public Dictionary<DayOfWeek, List<GymExercise>> Exercises { get; set; } = new();

    public void AddExercise(DayOfWeek day, GymExercise exercise)
    {
        if (!Exercises.TryGetValue(day, out var list))
        {
            list = new List<GymExercise>();
            Exercises[day] = list;
        }
        list.Add(exercise);
    }

    public void RemoveExercise(DayOfWeek day, GymExercise exercise)
    {
        if (Exercises.TryGetValue(day, out var list))
            list.Remove(exercise);
    }
}