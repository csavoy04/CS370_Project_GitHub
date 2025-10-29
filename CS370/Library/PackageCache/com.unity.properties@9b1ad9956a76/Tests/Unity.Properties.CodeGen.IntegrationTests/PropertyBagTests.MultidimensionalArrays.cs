using System.Collections.Generic;
using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
#pragma warning disable 649
    [GeneratePropertyBag]
    class ClassWithMultidimensionalArray
    {
        public int[,] IntArrayField;
    }
    [GeneratePropertyBag]
    class ClassWithMultidimensionalGeneric
    {
        public Dictionary<int, int[,]> DictionaryWithMultidimensionalArray;
        public Dictionary<int, List<int[,]>> DictionaryWithListOfMultidimensionalArray;
    }
#pragma warning restore 649

    [TestFixture]
    sealed partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassWithMultidimensionalArray_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassWithMultidimensionalArray>();
            AssertPropertyCount<ClassWithMultidimensionalArray>(1);
            AssertPropertyBagContainsProperty<ClassWithMultidimensionalArray, int[,]>("IntArrayField");
        }

        [Test]
        public void ClassWithMultidimensionalGeneric_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassWithMultidimensionalGeneric>();
            AssertPropertyCount<ClassWithMultidimensionalGeneric>(2);
            AssertPropertyBagContainsProperty<ClassWithMultidimensionalGeneric, Dictionary<int, int[,]>>("DictionaryWithMultidimensionalArray");
            AssertPropertyBagContainsProperty<ClassWithMultidimensionalGeneric, Dictionary<int, List<int[,]>>>("DictionaryWithListOfMultidimensionalArray");
            AssertPropertyBagIsADictionaryPropertyBag<Dictionary<int, int[,]>, int, int[,]>();
            AssertPropertyBagIsADictionaryPropertyBag<Dictionary<int, List<int[,]>>, int, List<int[,]>>();
            AssertPropertyBagIsAListPropertyBag<List<int[,]>, int[,]>();
        }
    }
}