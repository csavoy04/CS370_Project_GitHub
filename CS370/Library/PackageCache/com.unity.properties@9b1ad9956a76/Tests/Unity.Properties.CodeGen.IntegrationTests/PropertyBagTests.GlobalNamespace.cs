using NUnit.Framework;
using Unity.Properties;

[GeneratePropertyBag]
class ClassInGlobalNamespace
{
}


namespace Unity.Properties.CodeGen.IntegrationTests
{
    partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassInGlobalNamespace_HasPropertyBagsGenerated()
        {
            AssertPropertyBagIsCodeGenerated<ClassInGlobalNamespace>();
        }
    }
}