using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    [GeneratePropertyBag]
    partial class PartialClass
    {
        [GeneratePropertyBag]
        partial struct NestedType
        {
            public NestedType(float value1, float value2)
            {
                Value = value1;
                m_DoublyNestedType = new DoublyNestedType {Value = value2 };
            }

            [GeneratePropertyBag]
            partial class DoublyNestedType
            {
                public float Value;
            }

            public float Value;

            [CreateProperty] DoublyNestedType m_DoublyNestedType;
        }

        [CreateProperty] NestedType m_NestedType;

        public PartialClass(float value1, float value2)
        {
            m_NestedType = new NestedType(value1, value2);
        }
    }

    [TestFixture]
    partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void NestedPrivateTypes_WhenContainingTypesArePartial_CanBeGenerated()
        {
            AssertPropertyBagIsCodeGenerated<PartialClass>();
            AssertPropertyCount<PartialClass>(1);
            var container = new PartialClass(5, 10);
            Assert.That(PropertyContainer.TryGetValue(ref container, "m_NestedType", out object nested), Is.True);
            Assert.That(nested.GetType().Name, Is.EqualTo("NestedType"));

            Assert.That(PropertyContainer.TryGetValue(ref nested, "m_DoublyNestedType", out object doublyNested), Is.True);
            Assert.That(doublyNested.GetType().Name, Is.EqualTo("DoublyNestedType"));
            Assert.That(PropertyContainer.TryGetValue(ref doublyNested, "Value", out float value), Is.True);
            Assert.That(value, Is.EqualTo(10));
        }
    }
}