using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Unity.Properties.CodeGen.IntegrationTests
{
    partial class SourceGeneratorsTestFixture
    {
        public class BaseClassWithProperty
        {
            [CreateProperty]
            public IList ListProperty { get; set; }
            
            public float value { get; set; }
        }

        // Commented because there is an instability on CI where it seems to use an earlier version of the source generation dll. 
        // Re-enable code generation once the issue has been investigated.
        // [GeneratePropertyBag]
        public class SubClassWithNewProperty : BaseClassWithProperty
        {
            IList m_List;

            [CreateProperty]
            public new IList ListProperty
            {
                get => m_List;
                set => m_List = value;
            }
            
            [CreateProperty]
            public new Vector3 value { get; set; }
        }
        
        [Test]
        [Ignore("Ignored because there is an instability on CI where it seems to use an earlier version of the source generation dll.")]
        public void SubClassWithNewProperty_HasPropertyGenerated()
        {
            AssertPropertyCount<SubClassWithNewProperty>(2);
            AssertPropertyBagContainsProperty<SubClassWithNewProperty, IList>(nameof(SubClassWithNewProperty.ListProperty));
            AssertPropertyBagContainsProperty<SubClassWithNewProperty, Vector3>(nameof(SubClassWithNewProperty.value));
        }

        [Test]
        [Ignore("Ignored because there is an instability on CI where it seems to use an earlier version of the source generation dll.")]
        public void SubClassWithNewProperty_WhenVisited_VisitsTheCorrectInstance()
        {
            var container = new SubClassWithNewProperty();
            container.ListProperty = new List<int> {0, 1};
            container.value = Vector3.back;
            
            var asBase = (BaseClassWithProperty) container;
            asBase.ListProperty = new int[] {2, 3};
            asBase.value = 15.0f;

            Assert.That(PropertyContainer.TryGetValue(ref container, nameof(SubClassWithNewProperty.ListProperty), out List<int> list), Is.True);
            Assert.That(list, Is.EquivalentTo(new List<int>{0, 1}));
            
            Assert.That(PropertyContainer.TryGetValue(ref container, nameof(SubClassWithNewProperty.ListProperty), out int[] _), Is.False);
            
            Assert.That(PropertyContainer.TryGetValue(ref container, nameof(SubClassWithNewProperty.value), out Vector3 charValue), Is.True);
            Assert.That(charValue, Is.EqualTo(Vector3.back));
            
            Assert.That(PropertyContainer.TryGetValue(ref container, nameof(SubClassWithNewProperty.value), out float _), Is.False);
        }
    }
}