namespace Example.Common.Database.Enums;

public interface IReferenceByProductProvider
{
    List<ReferenceByProductCommon> GetReferences(string? productShortName = null, string? entity = null, string? reference = null);
}
