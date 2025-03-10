using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class Tests
    {
        public Tests()
        {
            Answers = new HashSet<Answers>();
            TestResults = new HashSet<TestResults>();
        }

        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }

        public virtual Subjects Subject { get; set; }
        public virtual ICollection<Answers> Answers { get; set; }
        public virtual ICollection<TestResults> TestResults { get; set; }
    }
}
