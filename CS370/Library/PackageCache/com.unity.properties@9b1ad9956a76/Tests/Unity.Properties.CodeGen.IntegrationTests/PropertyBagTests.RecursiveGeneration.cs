using System.Collections.Generic;
using NUnit.Framework;
using TestTypes;
using Unity.Properties;

class ClassWithoutNamespace
{
}

namespace Editor
{
    class ClassInEditorNamespace
    {
    }
}

namespace UnityEngine
{
    class ClassInUnityEngineNamespace
    {
    }
}

namespace Unity.Properties
{
    class ClassInPropertiesNamespace
    {
    }
}

namespace Unity.Properties.Generated
{
    class ClassInPropertiesCodeGenNamespace
    {
    }
}

namespace TestTypes
{
    struct GenericStruct<T1, T2>
    {
        public T1 Value1;
        public T2 Value2;
    }

    struct GenericStructWithCollections<T1, T2>
    {
        public List<T1> List;
        public Dictionary<T1, T2> Dict;
    }

    struct NormalStruct
    {
        public float FloatValue;
        public string StringValue;
    }

    class TopLevelClass
    {
        [CreateProperty] OneLevelNested Nested;

        public class OneLevelNested
        {
            [CreateProperty] TwoLevelNested MoreNested;

            public class TwoLevelNested
            {
                [CreateProperty] ThreeLevelNested VeryNested;
                public class ThreeLevelNested
                {
                    public NormalStruct Data;
                }
            }
        }
    }

    [GeneratePropertyBag]
    class Everything
    {
        public ClassWithoutNamespace cwn;
        public Editor.ClassInEditorNamespace cen;
        public UnityEngine.ClassInUnityEngineNamespace cun;
        public Unity.Properties.ClassInPropertiesNamespace cpn;
        public Unity.Properties.Generated.ClassInPropertiesCodeGenNamespace cpgn;
        public GenericStruct<(int?, bool), GenericStructWithCollections<TopLevelClass, NormalStruct>> oof;
    }
}

namespace Unity.Properties.CodeGen.IntegrationTests
{
    partial class SourceGeneratorsTestFixture
    {
        [Test]
        public void ClassWithDeepHierarchyOfTypes_RecursivelyGenereatesPropertyBag()
        {
            AssertPropertyBagIsCodeGenerated<Everything>();
            AssertPropertyBagIsCodeGenerated<ClassWithoutNamespace>();
            AssertPropertyBagIsCodeGenerated<global::Editor.ClassInEditorNamespace>();
            AssertPropertyBagIsCodeGenerated<UnityEngine.ClassInUnityEngineNamespace>();
            AssertPropertyBagIsCodeGenerated<Unity.Properties.ClassInPropertiesNamespace>();
            AssertPropertyBagIsCodeGenerated<Unity.Properties.Generated.ClassInPropertiesCodeGenNamespace>();
            AssertPropertyBagIsCodeGenerated<GenericStruct<(int?, bool), GenericStructWithCollections<TopLevelClass, NormalStruct>>>();
            AssertPropertyBagIsCodeGenerated<(int?, bool)>();
            AssertPropertyBagIsCodeGenerated<GenericStructWithCollections<TopLevelClass, NormalStruct>>();
            AssertPropertyBagIsCodeGenerated<TopLevelClass>();
            AssertPropertyBagIsCodeGenerated<NormalStruct>();
            AssertPropertyBagIsCodeGenerated<TopLevelClass.OneLevelNested>();
            AssertPropertyBagIsCodeGenerated<TopLevelClass.OneLevelNested.TwoLevelNested>();
            AssertPropertyBagIsCodeGenerated<TopLevelClass.OneLevelNested.TwoLevelNested.ThreeLevelNested>();

            AssertPropertyBagIsAListPropertyBag<List<TopLevelClass>, TopLevelClass>();
            AssertPropertyBagIsADictionaryPropertyBag<Dictionary<TopLevelClass, NormalStruct>, TopLevelClass, NormalStruct>();
        }
    }
}