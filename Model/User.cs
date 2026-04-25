using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User : People
    {
        private string email = string.Empty;
        private int goal;
        private int gradeGoal;
        private int streak;
        private DateTime? lastTaskDate;

        public string Email { get => email; set => email = value; }
        public int Goal { get => goal; set => goal = value; }
        public int GradeGoal { get => gradeGoal; set => gradeGoal = value; }
        public int Streak { get => streak; set => streak = value; }
        public DateTime? LastTaskDate { get => lastTaskDate; set => lastTaskDate = value; }
    }
}
