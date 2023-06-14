using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NP2hwServer
{
    internal class PostContext : DbContext
    {
        public PostContext() : base("DbConnection") { }
        public DbSet<Post> Posts { get; set; }
    }
}
