using System;
using NUnit.Framework;
using UnityEngine.Scripting;

namespace Unity.Properties.Tests
{
    class TypeConstructionTests
    {
        static class Types
        {
            [Preserve]
            public class NoConstructorClass{}

            [Preserve]
            public class DefaultConstructorClass
            {
                [Preserve]
                public DefaultConstructorClass() {}
            }

            [Preserve]
            public class InternalDefaultConstructorClass
            {
                internal InternalDefaultConstructorClass() {}
            }

            [Preserve]
            public class PrivateDefaultConstructorClass
            {
                PrivateDefaultConstructorClass() {}
            }

            [Preserve]
            public class CustomConstructorClass
            {
                public CustomConstructorClass(int a) {}
            }

            [Preserve]
            public class DefaultAndCustomConstructorClass
            {
                [Preserve]
                public DefaultAndCustomConstructorClass() {}

                [Preserve]
                public DefaultAndCustomConstructorClass(int a) {}
            }

            [Preserve]
            public struct NoConstructorStruct{}

            [Preserve]
            public struct CustomConstructorStruct
            {
                [Preserve]
                public CustomConstructorStruct(int a) {}
            }

            public interface IInterface {}

            [Preserve]
            public abstract class AbstractClassWithNoConstructor : IInterface {}

            [Preserve]
            public class ChildOfAbstractClassWithNoConstructor : AbstractClassWithNoConstructor
            {
            }

            [Preserve]
            public abstract class AbstractClassWithDefaultConstructor : IInterface
            {
                [Preserve]
                public AbstractClassWithDefaultConstructor() {}
            }

            [Preserve]
            public class ChildOfAbstractClassWithDefaultConstructor : AbstractClassWithDefaultConstructor
            {
            }

            [Preserve]
            public abstract class AbstractClassWithInternalDefaultConstructor : IInterface
            {
                [Preserve]
                internal AbstractClassWithInternalDefaultConstructor() {}
            }

            [Preserve]
            public class ChildOfAbstractClassWithPrivateDefaultConstructor : AbstractClassWithInternalDefaultConstructor
            {
            }

            [Preserve]
            public class NotConstructableBaseClass
            {
                [Preserve]
                protected NotConstructableBaseClass() {}
            }

            [Preserve]
            public class NotConstructableDerivedClass : NotConstructableBaseClass
            {
                [Preserve]
                protected NotConstructableDerivedClass() {}
            }

            [Preserve]
            public class ConstructableDerivedClass : NotConstructableBaseClass {}

            [Preserve]
            public class A : ConstructableDerivedClass {}

            [Preserve]
            public class B : ConstructableDerivedClass {}

            [Preserve]
            public class C : ConstructableDerivedClass {}
        }

        [Test]
        public void CanBeConstructedFromGenericMethod_WithConstructableType_ReturnsTrue()
        {
            Assert.That(TypeUtility.CanBeInstantiated<ConstructibleBaseType>(), Is.True);
            Assert.That(TypeUtility.CanBeInstantiated<ConstructibleDerivedType>(), Is.True);
            Assert.That(TypeUtility.CanBeInstantiated<NoConstructorType>(), Is.True);
            Assert.That(TypeUtility.CanBeInstantiated<ParameterLessConstructorType>(), Is.True);
            Assert.That(TypeUtility.CanBeInstantiated<ScriptableObjectType>(), Is.True);
        }

        [Test]
        public void CanBeConstructedFromGenericMethod_WithNonConstructableType_ReturnsFalse()
        {
            Assert.That(TypeUtility.CanBeInstantiated<IConstructInterface>(), Is.False);
            Assert.That(TypeUtility.CanBeInstantiated<AbstractConstructibleBaseType>(), Is.False);
            Assert.That(TypeUtility.CanBeInstantiated<NonConstructibleDerivedType>(), Is.False);
            Assert.That(TypeUtility.CanBeInstantiated<ParameterConstructorType>(), Is.False);
        }

        [TestCase(typeof(Types.NoConstructorClass))]
        [TestCase(typeof(Types.NoConstructorStruct))]
        [TestCase(typeof(Types.DefaultConstructorClass))]
        [TestCase(typeof(Types.DefaultAndCustomConstructorClass))]
        [TestCase(typeof(Types.CustomConstructorStruct))]
        [TestCase(typeof(Types.ChildOfAbstractClassWithNoConstructor))]
        [TestCase(typeof(Types.ChildOfAbstractClassWithDefaultConstructor))]
        [TestCase(typeof(Types.ChildOfAbstractClassWithPrivateDefaultConstructor))]
        public void CanBeConstructedFromType_WithConstructableType_ReturnsTrue(Type type)
        {
            Assert.That(TypeUtility.CanBeInstantiated(type), Is.True);
        }

        [TestCase(typeof(Types.CustomConstructorClass))]
        [TestCase(typeof(Types.PrivateDefaultConstructorClass))]
        [TestCase(typeof(Types.InternalDefaultConstructorClass))]
        [TestCase(typeof(Types.IInterface))]
        [TestCase(typeof(Types.AbstractClassWithNoConstructor))]
        [TestCase(typeof(Types.AbstractClassWithDefaultConstructor))]
        [TestCase(typeof(Types.AbstractClassWithInternalDefaultConstructor))]
        public void CanBeConstructedFromType_WithNonConstructableType_ReturnsFalse(Type type)
        {
            Assert.That(TypeUtility.CanBeInstantiated(type), Is.False);
        }

        [Test]
        public void ConstructingAnInstance_WithAConstructableType_ReturnsAnActualInstance()
        {
            Assert.That(TypeUtility.Instantiate<Types.NoConstructorClass>(), Is.Not.Null);
            Assert.That(TypeUtility.Instantiate<Types.NoConstructorStruct>(), Is.Not.Null);
            Assert.That(TypeUtility.Instantiate<Types.DefaultConstructorClass>(), Is.Not.Null);
            Assert.That(TypeUtility.Instantiate<Types.DefaultAndCustomConstructorClass>(), Is.Not.Null);
            Assert.That(TypeUtility.Instantiate<Types.CustomConstructorStruct>(), Is.Not.Null);
            Assert.That(TypeUtility.Instantiate<Types.ChildOfAbstractClassWithNoConstructor>(), Is.Not.Null);
            Assert.That(TypeUtility.Instantiate<Types.ChildOfAbstractClassWithDefaultConstructor>(), Is.Not.Null);
            Assert.That(TypeUtility.Instantiate<Types.ChildOfAbstractClassWithPrivateDefaultConstructor>(), Is.Not.Null);
        }

        [Test]
        public void TryToConstructAnInstance_WithAConstructableType_ReturnsTrue()
        {
            Assert.That(TypeUtility.TryInstantiate<Types.NoConstructorClass>(out _), Is.True);
            Assert.That(TypeUtility.TryInstantiate<Types.NoConstructorStruct>(out _), Is.True);
            Assert.That(TypeUtility.TryInstantiate<Types.DefaultConstructorClass>(out _), Is.True);
            Assert.That(TypeUtility.TryInstantiate<Types.DefaultAndCustomConstructorClass>(out _), Is.True);
            Assert.That(TypeUtility.TryInstantiate<Types.CustomConstructorStruct>(out _), Is.True);
            Assert.That(TypeUtility.TryInstantiate<Types.ChildOfAbstractClassWithNoConstructor>(out _), Is.True);
            Assert.That(TypeUtility.TryInstantiate<Types.ChildOfAbstractClassWithDefaultConstructor>(out _), Is.True);
            Assert.That(TypeUtility.TryInstantiate<Types.ChildOfAbstractClassWithPrivateDefaultConstructor>(out _), Is.True);
        }

        [Test]
        public void SettingAndUnSettingAnExplicitConstructionMethod_ToCreateAnInstance_BehavesProperly()
        {
            Assert.That(TypeUtility.CanBeInstantiated<ParameterConstructorType>(), Is.False);
            Assert.That(TypeUtility.CanBeInstantiated(typeof(ParameterConstructorType)), Is.False);
            TypeUtility.SetExplicitInstantiationMethod(ExplicitConstruction);
            Assert.That(TypeUtility.CanBeInstantiated<ParameterConstructorType>(), Is.True);
            Assert.That(TypeUtility.CanBeInstantiated(typeof(ParameterConstructorType)), Is.True);
            {
                var instance = TypeUtility.Instantiate<ParameterConstructorType>();
                Assert.That(instance, Is.Not.Null);
                Assert.That(instance.Value, Is.EqualTo(10.0f));
            }

            TypeUtility.SetExplicitInstantiationMethod<ParameterConstructorType>(null);
            Assert.That(TypeUtility.CanBeInstantiated<ParameterConstructorType>(), Is.False);
            Assert.That(TypeUtility.CanBeInstantiated(typeof(ParameterConstructorType)), Is.False);
        }

        [Test]
        public void ConstructingAndInstance_FromADerivedType_ReturnsAnInstance()
        {
            {
                var instance = TypeUtility.Instantiate<ConstructibleBaseType>(typeof(ConstructibleDerivedType));
                Assert.That(instance, Is.Not.Null);
                Assert.That(instance, Is.TypeOf<ConstructibleDerivedType>());
                Assert.That(instance.Value, Is.EqualTo(25.0f));
                Assert.That((instance as ConstructibleDerivedType).SubValue, Is.EqualTo(50.0f));
            }

            {
                var instance = TypeUtility.Instantiate<ConstructibleDerivedType>();
                Assert.That(instance, Is.Not.Null);
                Assert.That(instance.Value, Is.EqualTo(25.0f));
                Assert.That(instance.SubValue, Is.EqualTo(50.0f));
            }

            {
                var instance = TypeUtility.Instantiate<ConstructibleDerivedType>();
                Assert.That(instance, Is.Not.Null);
                Assert.That(instance.Value, Is.EqualTo(25.0f));
                Assert.That(instance.SubValue, Is.EqualTo(50.0f));
            }
        }

        [Test]
        public void ConstructingAndInstance_FromANonConstructableDerivedType_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => TypeUtility.Instantiate<IConstructInterface>(typeof(NonConstructibleDerivedType)));
        }

        [Test]
        public void ConstructingAndInstance_FromANonAssignableDerivedType_Throws()
        {
            Assert.Throws<ArgumentException>(() => TypeUtility.Instantiate<IConstructInterface>(typeof(ParameterLessConstructorType)));
        }

        [Test]
        public void ConstructingAnInstance_DerivedFromObject_IsAlwaysPossible()
        {
            Assert.That(TypeUtility.Instantiate<object>(typeof(Types.A)), Is.Not.Null);
        }

        static ParameterConstructorType ExplicitConstruction()
        {
            return new ParameterConstructorType(10.0f);
        }

        static ParameterConstructorType OtherExplicitConstruction()
        {
            return new ParameterConstructorType(10.0f);
        }
    }
}
