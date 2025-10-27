using NUnit.Framework;

namespace Unity.Properties.Tests
{
    class TypeTraitsTests
    {
        [Test]
        public void TypeTraits_Primitives()
        {
            TestPrimitives<sbyte>();
            TestPrimitives<short>();
            TestPrimitives<int>();
            TestPrimitives<long>();
            TestPrimitives<byte>();
            TestPrimitives<ushort>();
            TestPrimitives<uint>();
            TestPrimitives<ulong>();
            TestPrimitives<float>();
            TestPrimitives<double>();
            TestPrimitives<bool>();
            TestPrimitives<char>();
            TestPrimitives<int>();
        }

        [Test]
        public void TypeTraits_NullableTypes()
        {
            TestNullablePrimitives<sbyte?>();
            TestNullablePrimitives<short?>();
            TestNullablePrimitives<int?>();
            TestNullablePrimitives<long?>();
            TestNullablePrimitives<byte?>();
            TestNullablePrimitives<ushort?>();
            TestNullablePrimitives<uint?>();
            TestNullablePrimitives<ulong?>();
            TestNullablePrimitives<float?>();
            TestNullablePrimitives<double?>();
            TestNullablePrimitives<bool?>();
            TestNullablePrimitives<char?>();
            TestNullablePrimitives<int?>();
        }

        static void TestPrimitives<T>()
        {
            Assert.That(TypeTraits<T>.IsPrimitive, Is.True);
            Assert.That(TypeTraits<T>.IsValueType, Is.True);
            Assert.That(TypeTraits<T>.IsAbstract, Is.False);
            Assert.That(TypeTraits<T>.IsNullable, Is.False);
            Assert.That(TypeTraits<T>.IsArray, Is.False);
            Assert.That(TypeTraits<T>.IsInterface, Is.False);
            Assert.That(TypeTraits<T>.CanBeNull, Is.False);
            Assert.That(TypeTraits<T>.IsContainer, Is.False);
            Assert.That(TypeTraits<T>.IsEnumFlags, Is.False);
            Assert.That(TypeTraits<T>.IsAbstractOrInterface, Is.False);
        }
        
        static void TestNullablePrimitives<T>()
        {
            Assert.That(TypeTraits<T>.IsPrimitive, Is.False);
            Assert.That(TypeTraits<T>.IsValueType, Is.True);
            Assert.That(TypeTraits<T>.IsAbstract, Is.False);
            Assert.That(TypeTraits<T>.IsNullable, Is.True);
            Assert.That(TypeTraits<T>.IsArray, Is.False);
            Assert.That(TypeTraits<T>.IsInterface, Is.False);
            Assert.That(TypeTraits<T>.CanBeNull, Is.True);
            Assert.That(TypeTraits<T>.IsContainer, Is.True);
            Assert.That(TypeTraits<T>.IsEnumFlags, Is.False);
            Assert.That(TypeTraits<T>.IsAbstractOrInterface, Is.False);
        }
    }
}