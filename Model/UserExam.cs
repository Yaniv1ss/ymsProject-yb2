using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserExam : BaseEntity
    {
        private User user_id;
        private Exams exam_id;

        public User User_id { get => user_id; set => user_id = value; }
        public Exams Exam_id { get => exam_id; set => exam_id = value; }
    }
}
