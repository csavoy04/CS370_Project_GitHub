using JetBrains.Annotations;
using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    [UsedImplicitly]
    class ClassQualifiedWithTypeFromAnotherAssemblyWithGeneratePropertyBag : IInterfaceFromAnotherAssemblyWithGeneratePropertyBag
    {

    }
    
    [UsedImplicitly]
    class ClassWithBaseTypeFromAnotherAssemblyWithInterfaceFromAnotherAssembly : ClassWithGeneratePropertyBagFromNestedAssembly
    {

    }
    
    
    public struct StructWithGeneratedPropertyBag : IInterfaceFromAnotherAssemblyWithGeneratePropertyBag
    {
        
    }

    public class ClassWithGeneratedPropertyBag : IInterfaceFromAnotherAssemblyWithGeneratePropertyBag
    {
        
    }

    partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassQualifiedWithTypeFromAnotherAssembly_HasPropertyBagGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassQualifiedWithTypeFromAnotherAssemblyWithGeneratePropertyBag>();
        }
        
        [Test]
        public void ClassWithBaseTypeFromAnotherAssemblyWithInterfaceFromAnotherAssembly_HasPropertyBagGenerated()
        {
            AssertPropertyBagDoesNotExist<ClassWithBaseTypeFromAnotherAssemblyWithInterfaceFromAnotherAssembly>();
        }

        [Test]
        public void GeneratePropertyBagsForTypesQualifiedWith_RespectTypeGenerationOptions()
        {
            AssertPropertyBagDoesNotExist<StructWithGeneratedPropertyBag>();
            AssertPropertyBagIsCodeGenerated<ClassWithGeneratedPropertyBag>();
        }
    }
}