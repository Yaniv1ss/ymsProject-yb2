using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SchedulesRemindersList : List<SchedulesRemainders>
    {
        public SchedulesRemindersList() { }
        public SchedulesRemindersList(IEnumerable<SchedulesRemainders> list) : base(list) { }
        public SchedulesRemindersList(IEnumerable<BaseEntity> list) : base(list.Cast<SchedulesRemainders>().ToList()) { }
    }
    
}
