using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class Gruppa
    {
        public Gruppa()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public int Number { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
