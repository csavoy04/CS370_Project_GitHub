using JetBrains.Annotations;
using Unity.Properties;
using NUnit.Framework;
using Unity.Properties.CodeGen.IntegrationTests;

[assembly: GeneratePropertyBagsForType(typeof(ClassWithGeneric<int>))]
[assembly: GeneratePropertyBagsForType(typeof(ClassWithGeneric<NestedClass<float>>))]
[assembly: GeneratePropertyBagsForType(typeof(ClassWithGenericParameterAndGenericBase<int>))]

namespace Unity.Properties.CodeGen.IntegrationTests
{
#pragma warning disable 649
    public class ClassWithGeneric<T>
    {
        public T Value;
    }

    [UsedImplicitly]
    public class NestedClass<T>
    {
        public T Value;
    }

    public class ClassWithGenericParameterAndGenericBase<T> : Foo<T, float>
    {

    }

    [GeneratePropertyBag]
    public class Baz : Bar<string>
    {
        public float Root;
    }

    public class Bar<T> : Foo<float, int>
    {
        public T Value;
    }

    public class Foo<T0, T1>
    {
        public T0 Value0;
        public T1 Value1;

        [CreateProperty] public T0 Value0Property { get; set; }
    }

#pragma warning restore 649

    [TestFixture]
    sealed partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassWithGeneric_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassWithGeneric<int>>();
            AssertPropertyBagContainsProperty<ClassWithGeneric<int>, int>("Value");
            AssertPropertyCount<ClassWithGeneric<int>>(1);
        }

        [Test]
        public void ClassWithGenericNestedGeneric_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassWithGeneric<NestedClass<float>>>();
            AssertPropertyBagContainsProperty<ClassWithGeneric<NestedClass<float>>, NestedClass<float>>("Value");
            AssertPropertyCount<ClassWithGeneric<NestedClass<float>>>(1);

            AssertPropertyBagIsCodeGenerated<NestedClass<float>>();
            AssertPropertyBagContainsProperty<NestedClass<float>, float>("Value");
            AssertPropertyCount<NestedClass<float>>(1);
        }

        [Test]
        public void ClassWithSomeResolvedGenerics_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassWithGenericParameterAndGenericBase<int>>();
            AssertPropertyBagContainsProperty<ClassWithGenericParameterAndGenericBase<int>, int>("Value0");
            AssertPropertyBagContainsProperty<ClassWithGenericParameterAndGenericBase<int>, float>("Value1");
            AssertPropertyBagContainsProperty<ClassWithGenericParameterAndGenericBase<int>, int>("Value0Property");
            AssertPropertyCount<ClassWithGenericParameterAndGenericBase<int>>(3);
        }

        [Test]
        public void ClassWithGenericBaseClass_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<Baz>();
            AssertPropertyBagContainsProperty<Baz, float>("Root");
            AssertPropertyBagContainsProperty<Baz, string>("Value");
            AssertPropertyBagContainsProperty<Baz, float>("Value0");
            AssertPropertyBagContainsProperty<Baz, int>("Value1");
            AssertPropertyBagContainsProperty<Baz, float>("Value0Property");
            AssertPropertyCount<Baz>(5);
        }
    }
}