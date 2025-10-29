namespace Unity.Properties
{
    /// <summary>
    /// Base class to implement a visitor responsible for getting an object's concrete type as a generic.
    /// </summary>
    /// <remarks>
    /// It is required that the visited object is a container type with a property bag.
    /// </remarks>
    public abstract class ConcreteTypeVisitor : IPropertyBagVisitor
    {
        /// <summary>
        /// Implement this method to receive the strongly typed callback for a given container.
        /// </summary>
        /// <param name="container">The reference to the container.</param>
        /// <typeparam name="TContainer">The container type.</typeparam>
        protected abstract void VisitContainer<TContainer>(ref TContainer container);

        /// <inheritdoc cref="IPropertyBagVisitor.Visit{TContainer}"/>
        void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
            => VisitContainer(ref container);

#if !UNITY_2022_OR_NEWER
        internal static class AOT
        {
            internal static void RegisterType<TContainer>(TContainer container = default)
            {
                ((ConcreteTypeVisitor) default).VisitContainer(ref container);
            }
        }
#endif
    }
}
