using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP2hwServer
{
    internal class Post
    {
        public int Id { get; set; }
        public int ZipCode { get; set; }
        public string Street { get; set; }

        public override string ToString()
        {
            return $"{Id}, {ZipCode}, {Street}";
        }
    }
}
