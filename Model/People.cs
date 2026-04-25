using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class People : BaseEntity
    {
        private string pass = string.Empty;
        private string username = string.Empty;

        public string Pass { get => pass; set => pass = value; }
        public string Username { get => username; set => username = value; }
    }
}
