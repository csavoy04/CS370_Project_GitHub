using System.ComponentModel;
using Blah.A.B;
using NUnit.Framework;
using Unity.Properties;
using UnityEngine.Internal;

namespace Blah
{
    namespace A.B
    {
        /// <undoc/>
        [ExcludeFromDocs]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class Test
        {
            /// <undoc/>
            [ExcludeFromDocs]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public class Foo
            {
                /// <undoc/>
                [ExcludeFromDocs]
                [EditorBrowsable(EditorBrowsableState.Never)]
                public class Bar
                {
                    /// <undoc/>
                    [GeneratePropertyBag]
                    [ExcludeFromDocs]
                    [EditorBrowsable(EditorBrowsableState.Never)]
                    public class ClassWithNestedNamespacesAndTypes
                    {
                    }
                }
            }
        }
    }
}

namespace Unity.Properties.CodeGen.IntegrationTests
{
    namespace NestedNamespace
    {
        // ReSharper disable once ArrangeTypeModifiers
        [GeneratePropertyBag]
        class ClassWithMultipleNamespaceScopes
        {
#pragma warning disable 649
            public int Value;
#pragma warning restore 649
        }
    }

    [TestFixture]
    sealed partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassWithMultipleNamespaceScopes_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<NestedNamespace.ClassWithMultipleNamespaceScopes>();
        }

        [Test]
        public void ClassWithNestedNamespacesAndTypes_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<Test.Foo.Bar.ClassWithNestedNamespacesAndTypes>();
        }
    }
}
