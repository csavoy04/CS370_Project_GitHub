using System;
using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    [GeneratePropertyBag]
    class ClassWithPointerTypes
    {
#pragma warning disable 649
        public unsafe int* IntPointer;
        public IntPtr IntPtr;
#pragma warning restore 649
    }

    [TestFixture]
    sealed partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassWithPointerTypes_HasHasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassWithPointerTypes>();
            AssertPropertyCount<ClassWithPointerTypes>(1);
            AssertPropertyBagContainsProperty<ClassWithPointerTypes, IntPtr>("IntPtr");
        }
    }
}