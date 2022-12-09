using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persons
{
    
    public class Person
    {
        // пока храним в строке пароль, но надо хранить hash
        public Person(int id, string name,string password, Role? role)
        {
            Id=id;
            Name=name;
            Password=password;
            Role=role;
        }

        public int Id { get; }
        public string Name { get; set; }
        public string Password { get; }
        public Role? Role { get; set; }
    }
}
