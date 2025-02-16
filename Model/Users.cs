using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int RoleId { get; set; }
        public int GruppaId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Gruppa Gruppa { get; set; }
        public virtual Role Role { get; set; }
    }
}
