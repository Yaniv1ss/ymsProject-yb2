using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PeopleList : List<People>
    {
        public PeopleList() { }
        public PeopleList(IEnumerable<People> list) : base(list) { }
        public PeopleList(IEnumerable<BaseEntity> list) : base(list.Cast<People>().ToList()) { }
    }

}
    

