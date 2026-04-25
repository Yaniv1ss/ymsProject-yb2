using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TasksList : List<Tasks>
    {
        public TasksList() { }
        public TasksList(IEnumerable<Tasks> list) : base(list) { }
        public TasksList(IEnumerable<BaseEntity> list) : base(list.Cast<Tasks>().ToList()) { }
    }
}
