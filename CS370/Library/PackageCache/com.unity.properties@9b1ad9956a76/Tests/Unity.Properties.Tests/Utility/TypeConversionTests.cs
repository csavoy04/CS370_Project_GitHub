using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Properties.Tests
{
    [TestFixture]
    [Category("Type Conversion")]
    class TypeConversionTests
    {
        struct Handle
        {
            public int Value { get; set; }
        }

        [Test]
        public void TypeConversion_WithNullableValueTypes_ConvertsAsExpected()
        {
            // T? => T?
            {
                int? value = null;
                Assert.That(TypeConversion.TryConvert(ref value, out int? result), Is.True);
                Assert.That(result, Is.Null);

                value = 50;
                Assert.That(TypeConversion.TryConvert(ref value, out result), Is.True);
                Assert.That(result, Is.EqualTo(50));
            }

            // T? => T
            // This should work only if T? is not null
            {
                int? nv = null;
                Assert.That(TypeConversion.TryConvert(ref nv, out int result), Is.False);

                nv = 50;
                Assert.That(TypeConversion.TryConvert(ref nv, out result), Is.True);
                Assert.That(result, Is.EqualTo(50));
            }

            // T => T?
            // This should work all the time
            {
                var value = 50;
                Assert.That(TypeConversion.TryConvert(ref value, out int? result), Is.True);
                Assert.That(result, Is.EqualTo(50));
            }

            // T? => U?
            // In pure C#, if U is assignable from T, it should be supported, but this is not a case we support.
            {
                int? value = null;
                Assert.That(TypeConversion.TryConvert(ref value, out float? result), Is.False);

                value = 50;
                Assert.That(TypeConversion.TryConvert(ref value, out result), Is.False);
            }

            // T? => U
            // In pure C#, if U is assignable from T, it should be supported when T is not null, but this is not a case we support.
            {
                int? nv = null;
                Assert.That(TypeConversion.TryConvert(ref nv, out float result), Is.False);

                nv = 50;
                Assert.That(TypeConversion.TryConvert(ref nv, out result), Is.False);
            }
        }

        [Test]
        public void TypeConversion_WithNullableEnumTypes_ConvertsAsExpected()
        {
            // T? => T?
            {
                CameraClearFlags? value = null;
                Assert.That(TypeConversion.TryConvert(ref value, out CameraClearFlags? result), Is.True);
                Assert.That(result, Is.Null);

                value = CameraClearFlags.Skybox;
                Assert.That(TypeConversion.TryConvert(ref value, out result), Is.True);
                Assert.That(result, Is.EqualTo(CameraClearFlags.Skybox));
            }

            // T? => T
            // This should work only if T? is not null
            {
                CameraClearFlags? value = null;
                Assert.That(TypeConversion.TryConvert(ref value, out CameraClearFlags result), Is.False);

                value = CameraClearFlags.Skybox;
                Assert.That(TypeConversion.TryConvert(ref value, out result), Is.True);
                Assert.That(result, Is.EqualTo(CameraClearFlags.Skybox));
            }

            // T => T?
            // This should work all the time
            {
                var value = CameraClearFlags.Skybox;
                Assert.That(TypeConversion.TryConvert(ref value, out CameraClearFlags? result), Is.True);
                Assert.That(result, Is.EqualTo(CameraClearFlags.Skybox));
            }

            // T? => U?
            {
                CameraClearFlags? value = null;
                Assert.That(TypeConversion.TryConvert(ref value, out Vector3? result), Is.False);

                value = CameraClearFlags.Skybox;
                Assert.That(TypeConversion.TryConvert(ref value, out result), Is.False);
            }

            // T? => U
            {
                CameraClearFlags? nv = null;
                Assert.That(TypeConversion.TryConvert(ref nv, out Vector3 result), Is.False);

                nv = CameraClearFlags.Skybox;
                Assert.That(TypeConversion.TryConvert(ref nv, out result), Is.False);
            }
        }

        [Test]
        public void ConverterRegistry_WithRegisteringANullDelegate_Throws()
        {
            var registry = ConversionRegistry.Create();
            Assert.Throws<ArgumentException>(() => { registry.Register(typeof(float), typeof(string), null); });
        }

        [Test]
        public void Conversion_ToUnityObject_WorksForAnyDerivedType()
        {
            var tex = new Texture2D(256, 256);
            var mappings = new Dictionary<Handle, Object>
            {
                { new Handle{ Value = 10 }, tex },
            };

            TypeConversion.Register((ref Handle handle) => mappings.TryGetValue(handle, out var value) ? value : null);

            var h = new Handle { Value = 5 };
            Assert.That(TypeConversion.TryConvert(ref h, out Texture2D texture), Is.False);
            Assert.That(texture, Is.Null);

            h.Value = 10;
            Assert.That(TypeConversion.TryConvert(ref h, out texture), Is.True);
            Assert.That(texture, Is.EqualTo(tex));
        }

        [Test]
        public void Conversion_FromToUnityObject_DoesNotThrowStackOverflow()
        {
            var texture = new Texture2D(256, 256);
            Assert.That(TypeConversion.TryConvert(ref texture, out UnityEngine.Object obj), Is.True);
            Assert.That(obj, Is.EqualTo(texture));
        }
        
        [Test]
        public void ConversionFromObject_FromToUnityObject_DoesNotThrowStackOverflow()
        {
            object texture = new Texture2D(256, 256);
            Assert.That(TypeConversion.TryConvert(ref texture, out UnityEngine.Object obj), Is.True);
            Assert.That(obj, Is.EqualTo(texture));
        }
        
        [Test]
        public void ConversionFromMatrix4x4ToTexture2D_DoesNotThrowStackOverflow()
        {
            var src = new Matrix4x4();
            Assert.That(TypeConversion.TryConvert(ref src, out Texture2D _), Is.False);
        }
    }
}
