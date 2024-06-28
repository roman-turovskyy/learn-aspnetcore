namespace Example.Common.Database.Enums;

public static class ReferenceByProductProviderStaticHost
{
    private static IReferenceByProductProvider _referenceByProductProvider;

    public static List<ReferenceByProductCommon> ReferenceByProductCache
    {
        get
        {
            if (_referenceByProductProvider is null)
                throw new InvalidCastException("Did you forget to call SetReferenceByProductProvider() in your DI setup code?");

            return _referenceByProductProvider.GetReferences();
        }
    }

    public static void SetReferenceByProductProvider(IReferenceByProductProvider referenceByProductProvider)
    {
        _referenceByProductProvider = referenceByProductProvider;
    }
}
