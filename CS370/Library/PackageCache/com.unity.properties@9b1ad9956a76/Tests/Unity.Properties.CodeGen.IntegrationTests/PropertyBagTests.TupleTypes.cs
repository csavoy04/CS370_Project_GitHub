using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    [GeneratePropertyBag]
    public class ClassWithAnonymousType
    {
        public (int, float) AnonymousValue;
    }

    [GeneratePropertyBag]
    public class ClassWithNamedAnonymousType
    {
        public (int A, float, string B) AnonymousValue1;
        public (int C, string D) AnonymousValue2;
        public Tuple<int, string> TupleValue;
        [CreateProperty] (int P, MyStruct D) AnonymousValue3;
        [CreateProperty] (int P, MyNamespaceThatIsWayTooLong.WhyWouldAnyoneDoThis.SeriouslyComeOn.MyStruct D) AnonymousValue4;
    }

    namespace MyNamespaceThatIsWayTooLong.WhyWouldAnyoneDoThis.SeriouslyComeOn
    {
        struct MyStruct
        {
        }
    }

    struct MyStruct
    {
    }

    [TestFixture]
    sealed partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassWithAnonymousType_HasPropertyBagGenerated()
        {
            // Check properties are generated for anonymous field types.
            {
                AssertPropertyBagIsCodeGenerated<ClassWithNamedAnonymousType>();
                AssertPropertyCount<ClassWithNamedAnonymousType>(5);
                AssertPropertyBagContainsProperty<ClassWithNamedAnonymousType, (int A, float, string B)>("AnonymousValue1");
                AssertPropertyBagContainsProperty<ClassWithNamedAnonymousType, (int C, string D)>("AnonymousValue2");
                AssertPropertyBagContainsProperty<ClassWithNamedAnonymousType, Tuple<int, string>>("TupleValue");
                AssertPropertyBagContainsProperty<ClassWithNamedAnonymousType, (int P, MyStruct D)>("AnonymousValue3", typeof(TupleElementNamesAttribute));
                AssertPropertyIsReflected<ClassWithNamedAnonymousType, (int P, MyStruct D)>("AnonymousValue3");
                AssertPropertyBagContainsProperty<ClassWithNamedAnonymousType, (int P, MyNamespaceThatIsWayTooLong.WhyWouldAnyoneDoThis.SeriouslyComeOn.MyStruct D)>("AnonymousValue4");
                AssertPropertyIsReflected<ClassWithNamedAnonymousType, (int P, MyNamespaceThatIsWayTooLong.WhyWouldAnyoneDoThis.SeriouslyComeOn.MyStruct D)>("AnonymousValue4");

                AssertPropertyBagIsCodeGenerated<(int, MyStruct)>();
                AssertPropertyBagIsCodeGenerated<MyStruct>();
                AssertPropertyBagIsCodeGenerated<(int, MyNamespaceThatIsWayTooLong.WhyWouldAnyoneDoThis.SeriouslyComeOn.MyStruct)>();
                AssertPropertyBagIsCodeGenerated<MyNamespaceThatIsWayTooLong.WhyWouldAnyoneDoThis.SeriouslyComeOn.MyStruct>();
            }


            // Check that the anonymous type has a property bag generated
            {
                AssertPropertyBagIsCodeGenerated<(int, float, string)>();
                AssertPropertyCount<(int, float, string)>(3);
                AssertPropertyBagContainsProperty<(int, float, string), int>("Item1");
                AssertPropertyBagContainsProperty<(int, float, string), float>("Item2");
                AssertPropertyBagContainsProperty<(int, float, string), string>("Item3");

                AssertPropertyBagIsCodeGenerated<(int, string)>();
                AssertPropertyCount<(int, string)>(2);
                AssertPropertyBagContainsProperty<(int, string), int>("Item1");
                AssertPropertyBagContainsProperty<(int, string), string>("Item2");
            }
        }
    }
}