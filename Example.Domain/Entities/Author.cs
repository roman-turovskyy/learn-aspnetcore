namespace Example.Domain.Entities;

public class Author : EntityBase
{
    public Guid AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<BlogPost> Posts { get; set; } = new List<BlogPost>();
    public byte[] RowVersion { get; set; }
}