using System;
using NUnit.Framework;
using Unity.Properties.Internal;

namespace Unity.Properties.Reflection.Tests
{
    [TestFixture, TestRequires_IL2CPP_REFLECTION]
    partial class ReflectedPropertyBagTests
    {
    }

    static class PropertyBagExtensions
    {
        public static bool HasProperty<TContainer>(this IPropertyBag<TContainer> self, string name)
        {
            var container = default(TContainer);
            return self is INamedProperties<TContainer> keyable && keyable.TryGetProperty(ref container, name, out _);
        }
        
        public static object GetPropertyValue<TContainer>(this IPropertyBag<TContainer> self, ref TContainer container, string name)
        {
            if (self is INamedProperties<TContainer> keyable && keyable.TryGetProperty(ref container, name, out var property))
            {
                return property.GetValue(ref container);
            }
            
            throw new InvalidOperationException();
        }
    }
}