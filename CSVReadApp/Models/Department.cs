using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSVReadApp.Models
{
    public partial class Department
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public virtual ICollection<RepositoryDepartment> RepositoryDepartment { get; set; }

        public Department()
        {
            RepositoryDepartment = new HashSet<RepositoryDepartment>();
        }

        public Department(string name)
        {
            Name = name;
        }
    }
}
