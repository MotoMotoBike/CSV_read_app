using System.Collections.Generic;

namespace CSVReadApp.Models
{
    public partial class Catalog
    {
        public string IdKey { get; set; }
        public string Name { get; set; }
        public string CatalogId { get; set; }

        public virtual Catalog CatalogNavigation { get; set; }
        public virtual ICollection<Catalog> InverseCatalogNavigation { get; set; }
        public virtual ICollection<Repository> Repository { get; set; }

        public Catalog(string idKey, string name)
        {
            IdKey = idKey;
            Name = name;
            InverseCatalogNavigation = new HashSet<Catalog>();
        }
        
        public Catalog(string idKey, string name, string catalogId)
        {
            IdKey = idKey;
            Name = name;
            CatalogId = catalogId;
            InverseCatalogNavigation = new HashSet<Catalog>();
        }
    }
}
