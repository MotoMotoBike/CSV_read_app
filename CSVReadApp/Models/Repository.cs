using System;
using System.Collections.Generic;

namespace CSVReadApp.Models
{
    public partial class Repository
    {
        public string IdKey { get; set; }

        public string Name { get; set; }

        public string CatalogId { get; set; }

        public virtual Catalog Catalog { get; set; }

        public virtual ICollection<RepositoryDepartment> RepositoryDepartment { get; set; }

        public Repository()
        {
            RepositoryDepartment = new HashSet<RepositoryDepartment>();
        }

        public Repository(string idKey, string name, string catalogId)
        {
            RepositoryDepartment = new HashSet<RepositoryDepartment>();

            IdKey = idKey;
            Name = name;
            CatalogId = catalogId;
        }
    }
}
