using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TestEnity
    {
        public TestEnity(int id,string content)
        {
            Id=id;
            Content=content;
        }

        public int Id { get; }
        public string Content { get; }
    }
}
