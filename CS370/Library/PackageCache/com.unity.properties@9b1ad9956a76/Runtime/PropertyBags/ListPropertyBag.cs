using System.Collections.Generic;

namespace Unity.Properties
{
    /// <summary>
    /// A <see cref="IPropertyBag{T}"/> implementation for a <see cref="List{TElement}"/> type.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    public class ListPropertyBag<TElement> : IndexedCollectionPropertyBag<List<TElement>, TElement>
    {
        /// <inheritdoc/>
        protected override InstantiationKind InstantiationKind => InstantiationKind.PropertyBagOverride;

        /// <inheritdoc/>
        protected override List<TElement> InstantiateWithCount(int count) => new List<TElement>(count);

        /// <inheritdoc/>
        protected override List<TElement> Instantiate() => new List<TElement>();
    }
}
