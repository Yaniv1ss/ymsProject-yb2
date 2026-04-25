using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserExamList : List<UserExam>
    {
        public UserExamList() { }
        public UserExamList(IEnumerable<UserExam> list) : base(list) { }
        public UserExamList(IEnumerable<BaseEntity> list) : base(list.Cast<UserExam>().ToList()) { }
    }
}
