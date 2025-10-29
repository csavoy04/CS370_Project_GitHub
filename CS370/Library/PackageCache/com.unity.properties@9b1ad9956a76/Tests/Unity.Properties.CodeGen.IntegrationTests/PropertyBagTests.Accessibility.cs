using JetBrains.Annotations;
using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    partial class SourceGeneratorsTestFixture
    {
        [GeneratePropertyBag]
        class ClassWithSpecialAccessibility
        {
            [CreateProperty] public int PublicWithPrivateSetter { get; private set; }
            [CreateProperty] public int PublicWithPrivateGetter { private get; set; }
            [CreateProperty] public int PublicWithInternalSetter { get; internal set; }
            [CreateProperty] public int PublicWithInternalGetter { internal get; set; }
            [CreateProperty] int Private { get; set; }
        }

        [GeneratePropertyBag]
        partial class ClassWithSpecialAccessibilityWhenTypeIsPartial
        {
            [CreateProperty] public int PublicWithPrivateSetter { get; private set; }
            [CreateProperty] public int PublicWithPrivateGetter { private get; set; }
            [CreateProperty] int Private { get; set; }

            [GeneratePropertyBag]
            class Nested
            {
                [CreateProperty] public int PublicWithPrivateSetter { get; private set; }
                [CreateProperty] public int PublicWithPrivateGetter { private get; set; }
                [CreateProperty] int Private { get; set; }
            }
        }

        [Test]
        public void ClassWithSpecialAccessibility_HasReflectedPropertyGenerated()
        {
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithPrivateSetter));
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithPrivateGetter));
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithInternalSetter));
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithInternalGetter));
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibility, int>("Private");

            AssertPropertyIsReflected<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithPrivateSetter));
            AssertPropertyIsReflected<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithPrivateGetter));
            AssertPropertyIsReflected<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithInternalSetter));
            AssertPropertyIsReflected<ClassWithSpecialAccessibility, int>(nameof(ClassWithSpecialAccessibility.PublicWithInternalGetter));
            AssertPropertyIsReflected<ClassWithSpecialAccessibility, int>("Private");
        }

        [Test]
        public void ClassWithSpecialAccessibility_WhenTypeIsPartial_DoesNotHaveReflectedPropertyGenerated()
        {
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibilityWhenTypeIsPartial, int>(nameof(ClassWithSpecialAccessibilityWhenTypeIsPartial.PublicWithPrivateSetter));
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibilityWhenTypeIsPartial, int>(nameof(ClassWithSpecialAccessibilityWhenTypeIsPartial.PublicWithPrivateGetter));
            AssertPropertyBagContainsProperty<ClassWithSpecialAccessibilityWhenTypeIsPartial, int>("Private");

            AssertPropertyIsNotReflected<ClassWithSpecialAccessibilityWhenTypeIsPartial, int>(nameof(ClassWithSpecialAccessibilityWhenTypeIsPartial.PublicWithPrivateSetter));
            AssertPropertyIsNotReflected<ClassWithSpecialAccessibilityWhenTypeIsPartial, int>(nameof(ClassWithSpecialAccessibilityWhenTypeIsPartial.PublicWithPrivateGetter));
            AssertPropertyIsNotReflected<ClassWithSpecialAccessibilityWhenTypeIsPartial, int>("Private");
        }
    }
}
