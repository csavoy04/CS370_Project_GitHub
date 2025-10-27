using NUnit.Framework;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    [GeneratePropertyBag]
    public class ShouldNotBeCodeGen
    {
        public float value;
    }

    class SkipCodeGenTests
    {
        [Test]
        public void RequestedCodeGenOnTypes_WhenTheAssemblyIfMissingTheOptInAttribute_DoesNotGeneratePropertyBag()
        {
            Assert.That(PropertyBag.Exists<ShouldNotBeCodeGen>(), Is.False);
        }
    }
}
