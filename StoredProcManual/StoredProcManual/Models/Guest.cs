using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoredProcManual.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}