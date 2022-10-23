namespace Example.Domain.Entities;

public class ReferenceByProduct
{
    public Guid ReferenceByProductId { get; set; }

    public string Entity { get; set; }

    public string Reference { get; set; }

    public string Description { get; set; }

    public string Code { get; set; }

    public int SortOrder { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    //public List<ReferenceChild> Children { get; set; }

    //public ReferenceByProduct()
    //{
    //    Children = new List<ReferenceChild>();
    //}
}