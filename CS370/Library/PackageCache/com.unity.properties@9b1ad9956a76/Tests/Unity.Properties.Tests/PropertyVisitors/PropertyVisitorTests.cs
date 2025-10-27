using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#pragma warning disable 649

namespace Unity.Properties.Tests
{
    [TestFixture]
    partial class PropertyVisitorTests : PropertiesTestFixture
    {
        [GeneratePropertyBag]
        internal class Node
        {
            public string Name;
            public List<Node> Children;

            public Node(string name) => Name = name;
        }

        class PropertyStateValidationVisitor : PropertyVisitor
        {
            public int Count;
            
            protected override void VisitProperty<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
            {
                var index = GetIndex(property);

                Count++;

                PropertyContainer.TryAccept(this, ref value);
                
                Assert.That(GetIndex(property), Is.EqualTo(index));
            }

            static int GetIndex(IProperty property) => property is IListElementProperty l ? l.Index : -1;
        }

        class NodeNameAdapter : IVisitPropertyAdapter<Node>
        {
            public List<string> NodeNames = new List<string>();
            
            public void Visit<TContainer>(in VisitContext<TContainer, Node> context, ref TContainer container, ref Node value)
            {
                NodeNames.Add(value.Name);

                context.ContinueVisitation(ref container, ref value);
            }
        }

        [Test]
        public void PropertyVisitor_ContainerWithRecursiveTypes_PropertyStateIsCorrect()
        {
            var visitor = new PropertyStateValidationVisitor();
            var nodeNames = new NodeNameAdapter();

            visitor.AddAdapter(nodeNames);

            PropertyContainer.Accept(visitor, new Node("Root")
            {
                Children = new List<Node>
                {
                    new Node("A")
                    {
                        Children = new List<Node>()
                        {
                            new Node("a.1"),
                            new Node("a.2"),
                            new Node("a.3")
                        }
                    },
                    new Node("B"),
                    new Node("C")
                    {
                        Children = new List<Node>()
                        {
                            new Node("c.1"),
                            new Node("c.2"),
                            new Node("c.3"),
                            new Node("c.4"),
                            new Node("c.5"),
                            new Node("c.6")
                        }
                    },
                    new Node("D")
                },
            });

            Assert.That(nodeNames.NodeNames.SequenceEqual(new List<string>
            {
                "A", "a.1", "a.2", "a.3", 
                "B",
                "C", "c.1", "c.2", "c.3", "c.4", "c.5", "c.6",
                "D"
            }));
        }

        class VisitorWithoutVisitCollection : PropertyVisitor
        {
            public int VisitPropertyCount;
            public int VisitCollectionCount;

            protected override void VisitProperty<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
            {
                VisitPropertyCount++;
            }
        }

        class VisitorWithVisitCollection : PropertyVisitor
        {
            public int VisitPropertyCount;
            public int VisitCollectionCount;

            protected override void VisitProperty<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
            {
                VisitPropertyCount++;
            }

            protected override void VisitCollection<TContainer, TCollection, TElement>(Property<TContainer, TCollection> property, ref TContainer container, ref TCollection value)
            {
                VisitCollectionCount++;
            }
        }

        [Test]
        public void PropertyVisitor_NullCollectionType_VisitCollectionIsInvoked()
        {
            PropertyBag.RegisterList<int>();
            
            var withVisitCollection = new VisitorWithVisitCollection();
            var withoutVisitCollection = new VisitorWithoutVisitCollection();

            var container = new ClassWithLists()
            {
                Int32List = new List<int>(),
                ClassContainerList = null,
                StructContainerList = null,
                Int32ListList = null
            };

            PropertyContainer.Accept(withVisitCollection, container);
            PropertyContainer.Accept(withoutVisitCollection, container);

            Assert.That(withVisitCollection.VisitCollectionCount, Is.EqualTo(4));
            Assert.That(withVisitCollection.VisitPropertyCount, Is.EqualTo(0));

            Assert.That(withoutVisitCollection.VisitCollectionCount, Is.EqualTo(0));
            Assert.That(withoutVisitCollection.VisitPropertyCount, Is.EqualTo(4));
        }
    }
}
