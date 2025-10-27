namespace Unity.Properties
{
    /// <summary>
    /// Implement this interface to intercept the visitation of any primitive type.
    /// </summary>
    public interface IVisitPrimitivesPropertyAdapter :
        IVisitPropertyAdapter<sbyte>,
        IVisitPropertyAdapter<short>,
        IVisitPropertyAdapter<int>,
        IVisitPropertyAdapter<long>,
        IVisitPropertyAdapter<byte>,
        IVisitPropertyAdapter<ushort>,
        IVisitPropertyAdapter<uint>,
        IVisitPropertyAdapter<ulong>,
        IVisitPropertyAdapter<float>,
        IVisitPropertyAdapter<double>,
        IVisitPropertyAdapter<bool>,
        IVisitPropertyAdapter<char>
    {
    }
}