using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Permissions
{

    public class Permission
    {
        public int Id { get; }
        public string Desctription { get; set; }

        public Permission(int id, string desctription)
        {
            Id = id;
            Desctription = desctription;
        }
    }
}
