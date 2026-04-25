using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   
   
        public class ExamsRemaindersList : List<ExamsRemainders>
        {
            public ExamsRemaindersList() { }
            public ExamsRemaindersList(IEnumerable<ExamsRemainders> list) : base(list) { }
            public ExamsRemaindersList(IEnumerable<BaseEntity> list) : base(list.Cast<ExamsRemainders>().ToList()) { }
        }
    
}
