using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class TestResults
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public string GivenAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int Score { get; set; }

        public virtual Tests Test { get; set; }
        public virtual Users User { get; set; }
    }
}
