using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Unity.Properties.Internal;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    [TestFixture]
    partial class SourceGeneratorsTestFixture
    {
        static void AssertPropertyBagIsCodeGenerated<T>()
        {
            var propertyBag = PropertyBag.GetPropertyBag<T>();
            Assert.That(propertyBag, Is.Not.Null);

            var propertyBagType = propertyBag.GetType();
            Assert.That(propertyBagType.GetCustomAttribute<CompilerGeneratedAttribute>(), Is.Not.Null);
            Assert.That(propertyBagType.GetCustomAttribute<ReflectedPropertyBagAttribute>(), Is.Null);
        }

        static void AssertPropertyBagDoesNotExist<T>()
        {
            Assert.That(PropertyBag.Exists<T>(), Is.False);
        }

        static void AssertPropertyBagDoesNotExist(Type type)
        {
            var propertyBag = PropertyBag.Exists(type);
            Assert.That(propertyBag, Is.False);
        }

        static void AssertPropertyBagIsAListPropertyBag<TList, TElement>()
            where TList : IList<TElement>
        {
            var propertyBag = PropertyBag.GetPropertyBag<TList>();
            Assert.That(propertyBag, Is.Not.Null);
            Assert.That(propertyBag, Is.InstanceOf<ListPropertyBag<TElement>>());
        }

        static void AssertPropertyBagIsADictionaryPropertyBag<TDictionary, TKey, TElement>()
            where TDictionary : IDictionary<TKey, TElement>
        {
            var propertyBag = PropertyBag.GetPropertyBag<TDictionary>();
            Assert.That(propertyBag, Is.Not.Null);
            Assert.That(propertyBag, Is.InstanceOf<DictionaryPropertyBag<TKey, TElement>>());
        }

        static void AssertPropertyBagContainsProperty<T, TPropertyType>(string propertyName, params Type[] attributes)
        {
            var propertyBag = PropertyBag.GetPropertyBag<T>();
            var properties = propertyBag.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == propertyName)
                {
                    Assert.That(property, Is.InstanceOf<Property<T, TPropertyType>>());

                    var propertyAttributes = property.GetAttributes().ToList();
                    foreach (var attribute in attributes)
                    {
                        Assert.That(propertyAttributes.Count(a => a.GetType().IsAssignableFrom(attribute)), Is.GreaterThan(0));
                    }
                    return;
                }
            }

            Assert.Fail($"The property bag for type `{typeof(T).Name}` does not contain a property named '{propertyName}'");
        }

        static void AssertPropertyCount<T>(int count)
        {
            var propertyBag = PropertyBag.GetPropertyBag<T>();
            var properties = propertyBag.GetProperties();
            Assert.That(properties.Count(), Is.EqualTo(count));
        }

        static void AssertPropertyIsReflected<T, TPropertyType>(string propertyName)
        {
            var propertyBag = PropertyBag.GetPropertyBag<T>();
            var properties = propertyBag.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == propertyName)
                {
                    Assert.That(property, Is.InstanceOf<ReflectedMemberProperty<T, TPropertyType>>());
                    return;
                }
            }

            Assert.Fail($"The property bag for type `{typeof(T).Name}` does not contain a property named '{propertyName}'");
        }
        
        static void AssertPropertyIsNotReflected<T, TPropertyType>(string propertyName)
        {
            var propertyBag = PropertyBag.GetPropertyBag<T>();
            var properties = propertyBag.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == propertyName)
                {
                    Assert.IsNotAssignableFrom<ReflectedMemberProperty<T, TPropertyType>>(property);
                    return;
                }
            }

            Assert.Fail($"The property bag for type `{typeof(T).Name}` does not contain a property named '{propertyName}'");
        }

        static void AssertPropertyBagContainsProperty<T>(string propertyName, Type propertyType)
        {
            var propertyBag = PropertyBag.GetPropertyBag<T>();
            var properties = propertyBag.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == propertyName)
                {
                    Assert.That(property.DeclaredValueType(), Is.AssignableFrom(propertyType));
                    return;
                }
            }

            Assert.Fail($"The property bag for type `{typeof(T).Name}` does not contain a property named '{propertyName}'");
        }
    }
}