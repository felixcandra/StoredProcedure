using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StoredProcManual.Models
{
    public class MyContext : DbContext
    {
        public MyContext() : base("MyContext") { }
        public DbSet<StoredProcManual.Models.Guest> Guests { get; set; }
    }
}