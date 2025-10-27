using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Unity.Properties.Reflection.Tests
{
    class TestRequires_IL2CPP_REFLECTION : Attribute, ITestAction
    {
        public ActionTargets Targets { get; }

        public void BeforeTest(ITest test)
        {
#if ENABLE_IL2CPP && !ENABLE_IL2CPP_REFLECTION
            Assert.Ignore($"Test requires Unity 2022.1.0b8 or newer]");
#endif
        }

        public void AfterTest(ITest test)
        {
        }
    }
}