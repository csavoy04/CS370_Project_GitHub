using NUnit.Framework;
using Unity.Properties;
using Unity.Properties.CodeGen.IntegrationTests;

[assembly: GeneratePropertyBagsForType(typeof(ClassWithOpenGeneric<>))]
[assembly: GeneratePropertyBagsForTypesQualifiedWith(typeof(IGeneratePropertyBag))]

namespace Unity.Properties.CodeGen.IntegrationTests
{
#pragma warning disable 649
    [GeneratePropertyBag]
    class ClassWithOpenGeneric<T>
    {
        public T Value;
    }

    interface IGeneratePropertyBag
    {

    }

    public class ClassWithOpenGenericInterface<T> : IGeneratePropertyBag
    {
        public T Value;
    }
#pragma warning restore 649

    [TestFixture]
    sealed partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassWithOpenGeneric_DoesNotHavePropertyBagGenerated()
        {
            AssertPropertyBagDoesNotExist(typeof(ClassWithOpenGeneric<>));
        }

        [Test]
        public void ClassWithOpenGenericInterface_DoesNotHavePropertyBagGenerated()
        {
            AssertPropertyBagDoesNotExist(typeof(ClassWithOpenGenericInterface<>));
        }
    }
}