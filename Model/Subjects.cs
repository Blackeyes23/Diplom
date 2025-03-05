using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class Subjects
    {
        public Subjects()
        {
            Tests = new HashSet<Tests>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tests> Tests { get; set; }
    }
}
