using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TasksRemindersList : List<TasksRemainders>
    {
        public TasksRemindersList() { }
        public TasksRemindersList(IEnumerable<TasksRemainders> list) : base(list) { }
        public TasksRemindersList(IEnumerable<BaseEntity> list) : base(list.Cast<TasksRemainders>().ToList()) { }
    }
}
