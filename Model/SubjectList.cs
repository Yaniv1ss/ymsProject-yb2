using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
public class SubjectList : List<Subject>
    {
        public SubjectList() { }
        public SubjectList(IEnumerable<Subject> list) : base(list) { }
        public SubjectList(IEnumerable<BaseEntity> list) : base(list.Cast<Subject>().ToList()) { }
    }
}
