using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserSubjectList : List<UserSubject>
    {
        public UserSubjectList() { }
        public UserSubjectList(IEnumerable<UserSubject> list) : base(list) { }
        public UserSubjectList(IEnumerable<BaseEntity> list) : base(list.Cast<UserSubject>().ToList()) { }
    }
}
