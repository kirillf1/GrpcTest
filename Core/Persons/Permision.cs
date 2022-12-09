using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persons
{
    // Можно разделить на разрешения глобальные например, можно редачить и создать
    // класс разрешений 
    public class Permision
    {
        public int Id { get; }
        public string Desctription { get; set; }

        public Permision(int id,string desctription)
        {
            Id = id;
            Desctription = desctription;
        }
    }
}
