using NUnit.Framework;

namespace Unity.Properties.Tests
{
    [TestFixture]
    class PropertyBagStoreTests
    {
        struct Foo
        {

        }

        [Test]
        public void GetPropertyBag_WithUnregisteredType_ReturnsNull()
        {
            Assert.That(PropertyBag.Exists<Foo>(), Is.False);
        }
        
        struct MyValue
        {
            public float value;
        }

        class MyClass
        {
            public float value;
        }
        
        [Test]
        public void TryGetPropertyBagForValue_WhenValueIsAValueType_DoesNotAllocate()
        {
            GCAllocTest.Method(() =>
                {
                    var valueType = new MyValue();
                    PropertyBag.TryGetPropertyBagForValue(ref valueType, out _);
                    
                    var classType = new MyClass();
                    PropertyBag.TryGetPropertyBagForValue(ref classType, out _);
                })
                // Necessary as the property bags will be generated the first time this is called.
                .Warmup()
                // 1 for the creation of an instance of MyClass.
                .ExpectedCount(1)
                .Run();
        }
    }
}