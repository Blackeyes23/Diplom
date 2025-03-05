using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Diplom.Model
{
    public partial class Documents
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public DateTime? UploadDate { get; set; }
        public int UploaderId { get; set; }

        public virtual Users Uploader { get; set; }
    }
}
