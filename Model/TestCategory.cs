using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class TestCategory
    {
        public TestCategory()
        {
            TestGroups = new HashSet<TestGroups>();
            Tests = new HashSet<Tests>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TestGroups> TestGroups { get; set; }
        public virtual ICollection<Tests> Tests { get; set; }
    }
}
