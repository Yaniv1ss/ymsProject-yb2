using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserSubject : BaseEntity
    {
        private User user_id;
        private Subject subject_id;

        public User User_id { get => user_id; set => user_id = value; }
        public Subject Subject_id { get => subject_id; set => subject_id = value; }
    }
}
