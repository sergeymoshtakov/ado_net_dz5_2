using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entity
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public string Surname { get; set; } = null;
        public Boolean Gender { get; set; } 
        public string Status { get; set; } = null;
        public DateTime Birthday { get; set; }
        public string Picture { get; set; } = null;
    }
}
