using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Tasks : BaseEntity
    {
        private Subject? subject_id;
        private string title = string.Empty;
        private string description = string.Empty;
        private DateTime dueDate;
        private bool is_done;
        private User? user_id;

        public Subject? Subject_id { get => subject_id; set => subject_id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public bool Is_done { get => is_done; set => is_done = value; }
        public User? User_id { get => user_id; set => user_id = value; }
    }
}
