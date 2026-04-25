using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SchedulesRemainders : BaseEntity
    {
        private Schedules schedulesId;

        public Schedules SchedulesId { get => schedulesId; set => schedulesId = value; }
    }
}
