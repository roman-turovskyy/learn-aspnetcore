namespace Example.Common.Database.Enums;

public interface IReferenceByProductProvider
{
    List<ReferenceByProductCommon> GetRefrences(string? productShortName = null, string? entity = null, string? reference = null);
}
