using System;
using System.Collections.Generic;
using System.Text;

namespace Dets.Models
{
    internal class GymPlan
    {
        Dictionary<DayOfWeek, List<GymExercise>> Exercises { get; set; } = new Dictionary<DayOfWeek, List<GymExercise>>();

        private class GymExercise
        {
            string Exercise { get; set; } = "";
            TimeOnly Break { get; set; }
            (int, int) Reps { get; set; }
            int Sets { get; set; }
        }
    }
}
