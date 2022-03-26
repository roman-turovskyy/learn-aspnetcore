using System;
using System.Collections.Generic;

namespace DAL.Models
{
    /// <summary>
    /// Product descriptions in several languages.
    /// </summary>
    public partial class ProductDescription
    {
        public ProductDescription()
        {
            ProductModelProductDescriptionCulture = new HashSet<ProductModelProductDescriptionCulture>();
        }

        /// <summary>
        /// Primary key for ProductDescription records.
        /// </summary>
        public int ProductDescriptionId { get; set; }
        /// <summary>
        /// Description of the product.
        /// </summary>
        public string Description { get; set; } = null!;
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCulture { get; set; }
    }
}
