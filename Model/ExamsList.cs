using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ExamsList : List<Exams>
    {
        public ExamsList() { }
        public ExamsList(IEnumerable<Exams> list) : base(list) { }
        public ExamsList(IEnumerable<BaseEntity> list) : base(list.Cast<Exams>().ToList()) { }
    }
}
