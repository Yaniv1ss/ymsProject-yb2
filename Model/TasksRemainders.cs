using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TasksRemainders : BaseEntity
    {
        private Subject subject_id;
        public Subject Subject_id { get => subject_id; set => subject_id = value; }
        
    }
}
