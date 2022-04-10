using System;
using System.Collections.Generic;

namespace DAL.Models
{
    /// <summary>
    /// Bicycle assembly diagrams.
    /// </summary>
    public partial class Illustration
    {
        public Illustration()
        {
            ProductModelIllustration = new HashSet<ProductModelIllustration>();
        }

        /// <summary>
        /// Primary key for Illustration records.
        /// </summary>
        public int IllustrationId { get; set; }
        /// <summary>
        /// Illustrations used in manufacturing instructions. Stored as XML.
        /// </summary>
        public string? Diagram { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ProductModelIllustration> ProductModelIllustration { get; set; }
    }
}
