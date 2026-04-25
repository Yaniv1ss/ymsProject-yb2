using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Schedules : BaseEntity
    {
        private Subject? subject_id;
        private User? user_id;
        private string day_of_the_week = string.Empty;
        private int hour;

        public Subject? Subject_id { get => subject_id; set => subject_id = value; }
        public User? User_id { get => user_id; set => user_id = value; }
        public string Day_of_the_week { get => day_of_the_week; set => day_of_the_week = value; }
        public int Hour { get => hour; set => hour = value; }
    }
}
