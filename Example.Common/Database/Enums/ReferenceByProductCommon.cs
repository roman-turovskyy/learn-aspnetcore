namespace Example.Common.Database.Enums;

public class ReferenceByProductCommon
{
    public Guid Id { get; set; }
    public string Entity { get; set; }
    public string Reference { get; set; }
    // TODO: do we need Description?
    //public string Description { get; set; }
    public string Code { get; set; }
    // TODO: do we need SortOrder?
    //public int SortOrder{ get; set; }
    public string ProductShortName { get; set; }
}