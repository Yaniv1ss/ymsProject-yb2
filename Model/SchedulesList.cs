using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SchedulesList : List<Schedules>
    {
        public SchedulesList() { }
        public SchedulesList(IEnumerable<Schedules> list) : base(list) { }
        public SchedulesList(IEnumerable<BaseEntity> list) : base(list.Cast<Schedules>().ToList()) { }
    }
}
