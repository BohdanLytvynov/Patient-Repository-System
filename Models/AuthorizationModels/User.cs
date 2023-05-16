using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AuthorizationModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surename { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        

        public User(Guid id, string name, string surename, string lastname)
        {
            Id = id;

            Name = name;

            Surename = surename;

            Lastname = lastname;
        }

        public User()
        {

        }
    }
}
