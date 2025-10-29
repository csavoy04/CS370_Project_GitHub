using NUnit.Framework;
using Unity.Properties.Internal;
using UnityEngine.Scripting;

namespace Unity.Properties.Reflection.Tests
{
    partial class ReflectedPropertyBagTests
    {
        [Preserve]
        class ClassWithInternalProperties
        {
            public static string IntPropertyName => nameof(IntProperty);
            public static string FloatPropertyName => nameof(FloatProperty);
            public static string MaskedPropertyName => nameof(MaskedProperty);
            public static string VirtualPropertyName => nameof(VirtualProperty);

            [Preserve]
            internal int IntProperty { get; set; } = 42;
            [CreateProperty, Preserve] internal float FloatProperty { get; set; } = 123.456f;

            [CreateProperty, Preserve] internal int MaskedProperty { get; set; } = 1;
            [CreateProperty, Preserve] internal virtual short VirtualProperty { get; set; } = -12345;
        }

        [Preserve]
        class DerivedClassWithInternalProperties : ClassWithInternalProperties
        {
            public static string BoolPropertyName => nameof(BoolProperty);
            public static string StringPropertyName => nameof(StringProperty);

            [Preserve]
            internal bool BoolProperty { get; set; } = true;
            [CreateProperty, Preserve] internal string StringProperty { get; set; } = "Hello the World!";
            [CreateProperty, Preserve] internal new int MaskedProperty { get; set; } = 2;
            [CreateProperty, Preserve] internal override short VirtualProperty { get; set; } = 12345;
        }

        [Preserve]
        abstract class AbstractClassWithInternalProperties
        {
            public static string IntPropertyName => nameof(IntProperty);
            public static string FloatPropertyName => nameof(FloatProperty);

            [Preserve]
            internal abstract int IntProperty { get; set; }
            [CreateProperty, Preserve] internal abstract float FloatProperty { get; set; }
        }

        [Preserve]
        class ImplementedAbstractClassWithInternalProperties : AbstractClassWithInternalProperties
        {
            [Preserve]
            internal override int IntProperty { get; set; } = 13;
            [CreateProperty, Preserve] internal override float FloatProperty { get; set; } = 3.1416f;
        }

        [Test]
        public void CreatePropertyBag_ClassWithInternalProperties_PropertiesAreGenerated()
        {
            var propertyBag = new ReflectedPropertyBagProvider().CreatePropertyBag<ClassWithInternalProperties>();

            Assert.That(propertyBag, Is.Not.Null);

            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.IntPropertyName), Is.False);
            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.FloatPropertyName), Is.True);
            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.MaskedPropertyName), Is.True);
            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.VirtualPropertyName), Is.True);
            Assert.That(propertyBag.HasProperty(DerivedClassWithInternalProperties.BoolPropertyName), Is.False);
            Assert.That(propertyBag.HasProperty(DerivedClassWithInternalProperties.StringPropertyName), Is.False);

            var container = new ClassWithInternalProperties();

            Assert.That(propertyBag.GetPropertyValue(ref container, ClassWithInternalProperties.FloatPropertyName), Is.EqualTo(123.456f));
            Assert.That(propertyBag.GetPropertyValue(ref container, ClassWithInternalProperties.MaskedPropertyName), Is.EqualTo(1));
            Assert.That(propertyBag.GetPropertyValue(ref container, ClassWithInternalProperties.VirtualPropertyName), Is.EqualTo((short)-12345));
        }

        [Test]
        public void CreatePropertyBag_DerivedClassWithInternalProperties_PropertiesAreGenerated()
        {
            var propertyBag = new ReflectedPropertyBagProvider().CreatePropertyBag<DerivedClassWithInternalProperties>();

            Assert.That(propertyBag, Is.Not.Null);

            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.IntPropertyName), Is.False);
            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.FloatPropertyName), Is.True);
            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.MaskedPropertyName), Is.True);
            Assert.That(propertyBag.HasProperty(ClassWithInternalProperties.VirtualPropertyName), Is.True);
            Assert.That(propertyBag.HasProperty(DerivedClassWithInternalProperties.BoolPropertyName), Is.False);
            Assert.That(propertyBag.HasProperty(DerivedClassWithInternalProperties.StringPropertyName), Is.True);

            var container = new DerivedClassWithInternalProperties();

            Assert.That(propertyBag.GetPropertyValue(ref container, ClassWithInternalProperties.FloatPropertyName), Is.EqualTo(123.456f));
            Assert.That(propertyBag.GetPropertyValue(ref container, ClassWithInternalProperties.MaskedPropertyName), Is.EqualTo(2));
            Assert.That(propertyBag.GetPropertyValue(ref container, ClassWithInternalProperties.VirtualPropertyName), Is.EqualTo((short)12345));
            Assert.That(propertyBag.GetPropertyValue(ref container, DerivedClassWithInternalProperties.StringPropertyName), Is.EqualTo( "Hello the World!"));
        }

        [Test]
        public void CreatePropertyBag_ImplementedAbstractClassWithInternalProperties_PropertiesAreGenerated()
        {
            var propertyBag = new ReflectedPropertyBagProvider().CreatePropertyBag<ImplementedAbstractClassWithInternalProperties>();

            Assert.That(propertyBag, Is.Not.Null);

            Assert.That(propertyBag.HasProperty(AbstractClassWithInternalProperties.IntPropertyName), Is.False);
            Assert.That(propertyBag.HasProperty(AbstractClassWithInternalProperties.FloatPropertyName), Is.True);

            var container = new ImplementedAbstractClassWithInternalProperties();

            Assert.That(propertyBag.GetPropertyValue(ref container, AbstractClassWithInternalProperties.FloatPropertyName), Is.EqualTo(3.1416f));
        }
    }
}
