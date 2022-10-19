namespace Example.Domain.Entities;

public class BlogPost : EntityBase
{
    public Guid BlogPostId { get; set; }
    public string? Content { get; set; }
    public Author Author { get; set; }
    public byte[] RowVersion { get; set; }
}
