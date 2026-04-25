using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class Exams : BaseEntity
   {
        private Subject subject_id;
        private string title;
        private string exam_date;
        private int grade;

        public Subject Subject_id { get => subject_id; set => subject_id = value; }
        public string Title { get => title; set => title = value; }
        public string Exam_date { get => exam_date; set => exam_date = value; }
        public int Grade { get => grade; set => grade = value; }
    }
}
