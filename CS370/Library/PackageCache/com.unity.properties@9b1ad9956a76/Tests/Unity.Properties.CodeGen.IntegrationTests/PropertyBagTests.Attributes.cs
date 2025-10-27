using System;
using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    class CustomAttribute : Attribute
    {
        public int Value { get; set; }
    }

    class ClassWithAttributeOnPrivateField
    {
        // ReSharper disable once InconsistentNaming
        [CustomAttribute(Value = 25), CreateProperty] int Value;
    }

    [GeneratePropertyBag]
    class SuperClassWithAttributeOnPrivateField : ClassWithAttributeOnPrivateField
    {
        [CustomAttribute(Value = 50)]
        public string OtherValue;
    }

    [TestFixture]
    partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassThatInheritsClassWithAttributes_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<SuperClassWithAttributeOnPrivateField>();
            AssertPropertyCount<SuperClassWithAttributeOnPrivateField>(2);
            AssertPropertyBagContainsProperty<SuperClassWithAttributeOnPrivateField, int>("Value", typeof(CustomAttribute));
            AssertPropertyBagContainsProperty<SuperClassWithAttributeOnPrivateField, string>("OtherValue", typeof(CustomAttribute));

            var bag = (ContainerPropertyBag<SuperClassWithAttributeOnPrivateField>) PropertyBag.GetPropertyBag<SuperClassWithAttributeOnPrivateField>();
            var value = new SuperClassWithAttributeOnPrivateField();
            Assert.That(bag.TryGetProperty(ref value, "Value", out var property), Is.True);
            Assert.That(property.HasAttribute<CustomAttribute>(), Is.True);
            var attribute = property.GetAttribute<CustomAttribute>();
            Assert.That(attribute.Value, Is.EqualTo(25));

            Assert.That(bag.TryGetProperty(ref value, "OtherValue", out property), Is.True);
            Assert.That(property.HasAttribute<CustomAttribute>(), Is.True);
            attribute = property.GetAttribute<CustomAttribute>();
            Assert.That(attribute.Value, Is.EqualTo(50));
        }
    }
}