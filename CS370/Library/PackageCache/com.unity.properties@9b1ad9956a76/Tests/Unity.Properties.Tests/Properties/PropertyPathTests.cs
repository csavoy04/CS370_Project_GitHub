using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.Scripting;

namespace Unity.Properties.Tests
{
    [TestFixture]
    [Preserve]
    [Category("Property Path")]
    class PropertyPathTests
    {
        [Test]
        public void CanConstructPropertyPathManually()
        {
            var propertyPath = new PropertyPath();
            Assert.That(propertyPath.Length, Is.EqualTo(0));
            propertyPath = PropertyPath.AppendName(propertyPath, "Foo");
            Assert.That(propertyPath.Length, Is.EqualTo(1));
            Assert.That(propertyPath[0].Kind, Is.EqualTo(PropertyPathPartKind.Name));
            Assert.That(propertyPath[0].Name, Is.EqualTo("Foo"));

            propertyPath = PropertyPath.AppendName(propertyPath, "Bar");
            Assert.That(propertyPath.Length, Is.EqualTo(2));
            Assert.That(propertyPath[1].Kind, Is.EqualTo(PropertyPathPartKind.Name));
            Assert.That(propertyPath[1].Name, Is.EqualTo("Bar"));

            propertyPath = PropertyPath.AppendIndex(propertyPath, 5);
            Assert.That(propertyPath.Length, Is.EqualTo(3));
            Assert.That(propertyPath[2].Kind, Is.EqualTo(PropertyPathPartKind.Index));
            Assert.That(propertyPath[2].Index, Is.EqualTo(5));

            propertyPath = PropertyPath.AppendName(propertyPath, "Bee");
            Assert.That(propertyPath.Length, Is.EqualTo(4));
            Assert.That(propertyPath[3].Kind, Is.EqualTo(PropertyPathPartKind.Name));
            Assert.That(propertyPath[3].Name, Is.EqualTo("Bee"));

            Assert.That(propertyPath.ToString(), Is.EqualTo("Foo.Bar[5].Bee"));

            propertyPath = PropertyPath.Pop(propertyPath);

            Assert.That(propertyPath.Length, Is.EqualTo(3));
            Assert.That(propertyPath.ToString(), Is.EqualTo("Foo.Bar[5]"));
        }

        [Test]
        [TestCase("")]
        [TestCase("Foo")]
        [TestCase("[0]")]
        [TestCase("Foo[0]")]
        [TestCase("Foo[0].Bar")]
        [TestCase("Foo[0].Bar[1]")]
        [TestCase("Foo.Bar")]
        [TestCase("Foo.Bar[0]")]
        [TestCase("Foo.Bar[\"one\"]")]
        public void CanConstructAPropertyPathFromAString(string path)
        {
            Assert.That(() => CreateFromString(path), Throws.Nothing);
        }

        [Test]
        [TestCase("", 0)]
        [TestCase("Foo", 1)]
        [TestCase("Foo[0]", 2)]
        [TestCase("Foo[0].Bar", 3)]
        [TestCase("Foo[0].Bar[1]", 4)]
        [TestCase("Foo.Bar", 2)]
        [TestCase("Foo.Bar[0]", 3)]
        [TestCase("Foo.Foo.Foo.Foo.Foo", 5)]
        public void PropertyPathHasTheRightAmountOfParts(string path, int partCount)
        {
            var propertyPath = new PropertyPath(path);
            Assert.That(propertyPath.Length, Is.EqualTo(partCount));
        }

        [Test]
        [TestCase("Foo[0]", 0)]
        [TestCase("Foo[1]", 1)]
        [TestCase("Foo.Bar[2]", 2)]
        [TestCase("Foo.Bar[12]", 12)]
        [TestCase("Foo[0].Foo[1].Foo[2].Foo[3].Foo[4]", 0, 1, 2, 3, 4)]
        public void PropertyPathMapsListIndicesCorrectly(string path, params int[] indices)
        {
            var propertyPath = new PropertyPath(path);
            var listIndex = 0;
            for (var i = 0; i < propertyPath.Length; ++i)
            {
                var part = propertyPath[i];
                if (part.IsIndex)
                {
                    Assert.That(part.Index, Is.EqualTo(indices[listIndex]));
                    ++listIndex;
                }
            }
        }

        [Test]
        [TestCase("Foo[-1]")]
        [TestCase("Foo.Bar[-20]")]
        public void ThrowsWhenUsingNegativeIndices(string path)
        {
            Assert.That(() => CreateFromString(path), Throws.ArgumentException);
        }

        [Test]
        [TestCase("Foo[lol]")]
        public void ThrowsWhenUsingNonNumericIndices(string path)
        {
            Assert.That(() => CreateFromString(path), Throws.ArgumentException);
        }

        public struct CreateFromStringTestData
        {
            public string Input;
            public PropertyPathPart[] ExpectedParts;
            public int ExpectedAllocationCount
            {
                get
                {
                    switch (ExpectedParts.Length)
                    {
                        case 0:
                        case 1 when ExpectedParts[0].Kind == PropertyPathPartKind.Name:
                            return 0;
                        default:
                            // One per part and one optional one for the array of additional parts.
                            return ExpectedParts.Length + (ExpectedParts.Length <= PropertyPath.k_InlineCount ? 0 : 1);
                    }
                }
            }

            public override string ToString()
            {
                return Input;
            }
        }

        [Preserve]
        static IEnumerable<(string input, PropertyPathPart[] parts)> GetCreateFromStringTestData()
        {
            yield return ("", Array.Empty<PropertyPathPart>());
            yield return ("SinglePathProperty", new PropertyPathPart[]{ new ("SinglePathProperty") });
            yield return ("Path.SubPath", new PropertyPathPart[]{ new ("Path"), new("SubPath") });
            yield return ("Level1.Level2.Level3.Level4", new PropertyPathPart[]{ new ("Level1"), new ("Level2"), new ("Level3"), new ("Level4") });
            yield return ("Level1.Level2.Level3.Level4.Level5", new PropertyPathPart[]{ new ("Level1"), new ("Level2"), new ("Level3"), new ("Level4"), new ("Level5") });
            yield return ("Level1.Level2.Level3.Level4.Level5.Level6.Level7.Level8", new PropertyPathPart[]{ new ("Level1"), new ("Level2"), new ("Level3"), new ("Level4"), new ("Level5"), new ("Level6"), new ("Level7"), new ("Level8") });
            yield return ("[5]", new PropertyPathPart[]{ new (5) });
            yield return ("path[3].SubPath", new PropertyPathPart[]{ new ("path"), new (3), new ("SubPath") });
            yield return ("[\"5 is the way to go\"]", new PropertyPathPart[]{ new ((object) "5 is the way to go") });
        }

        [Preserve]
        static IEnumerable<CreateFromStringTestData> CreateFromStringTestDataSource()
        {
            foreach (var testData in GetCreateFromStringTestData())
            {
                yield return new CreateFromStringTestData
                {
                    Input = testData.input,
                    ExpectedParts = testData.parts
                };
            }
        }

        [Preserve]
        [TestCaseSource(nameof(CreateFromStringTestDataSource))]
        public void PropertyPath_WhenCreatedFromString_CreatesTheCorrectPropertyPath(CreateFromStringTestData testData)
        {
            {
                var path = new PropertyPath(testData.Input);
                Assert.That(path.Length, Is.EqualTo(testData.ExpectedParts.Length));
                for (var i = 0; i < path.Length; ++i)
                {
                    Assert.That(path[i], Is.EqualTo(testData.ExpectedParts[i]));
                }
            }
            {
                GCAllocTest.Method(() =>
                    {
                        new PropertyPath(testData.Input);
                    }).Warmup()
                    .ExpectedCount(testData.ExpectedAllocationCount)
                    .Run();
            }
        }

        public struct CombinePathsTestData
        {
            public PropertyPath Left;
            public PropertyPath Right;
            public PropertyPathPart[] ExpectedParts;
            public int ExpectedAllocationCount => ExpectedParts.Length <= PropertyPath.k_InlineCount ? 0 : 1;

            public override string ToString()
            {
                return $"\"{Left.ToString()}\" + \"{Right.ToString()}\"";
            }
        }

        [Preserve]
        static IEnumerable<(string, string, PropertyPathPart[])> GetCombinePathsTestData()
        {
            yield return ("", "", Array.Empty<PropertyPathPart>());
            yield return ("Path", "", new PropertyPathPart[]{ new ("Path") });
            yield return ("Path", "SubPath", new PropertyPathPart[]{ new ("Path"), new ("SubPath") });
            yield return ("Path", "[3]", new PropertyPathPart[]{ new ("Path"), new (3) });
            yield return ("Path", "[3].Subpath", new PropertyPathPart[]{ new ("Path"), new (3), new ("Subpath") });
            yield return ("Path", "[5][3].Subpath", new PropertyPathPart[]{ new ("Path"), new (5), new (3), new ("Subpath") });
            yield return ("Path[5]", "[3].Subpath", new PropertyPathPart[]{ new ("Path"), new (5), new (3), new ("Subpath") });
            yield return ("Path[5][3]", "Subpath", new PropertyPathPart[]{ new ("Path"), new (5), new (3), new ("Subpath") });
            yield return ("Path[5][3]", "Subpath[\"very nested\"]", new PropertyPathPart[]{ new ("Path"), new (5), new (3), new ("Subpath"), new ((object)"very nested") });
            yield return ("Path[5][3].Subpath.Subpath", "Subpath[\"very nested\"]", new PropertyPathPart[]{ new ("Path"), new (5), new (3), new ("Subpath"), new ("Subpath"), new ("Subpath"), new ((object)"very nested") });
        }

        [Preserve]
        static IEnumerable<CombinePathsTestData> CombinePathsTestDataSource()
        {
            foreach ((string left, string right, PropertyPathPart[] parts) data in GetCombinePathsTestData())
            {
                yield return  new CombinePathsTestData
                {
                    Left = new PropertyPath(data.left),
                    Right = new PropertyPath(data.right),
                    ExpectedParts = data.parts
                };
            }
        }

        [TestCaseSource(nameof(CombinePathsTestDataSource))]
        public void PropertyPath_WhenCombingPaths_CreatesTheCorrectPropertyPath(CombinePathsTestData testData)
        {
            {
                var path = PropertyPath.Combine(testData.Left, testData.Right);
                Assert.That(path.Length, Is.EqualTo(testData.ExpectedParts.Length));
                for (var i = 0; i < path.Length; ++i)
                {
                    Assert.That(path[i], Is.EqualTo(testData.ExpectedParts[i]));
                }
            }
            {
                GCAllocTest.Method(() =>
                    {
                        PropertyPath.Combine(testData.Left, testData.Right);
                    }).Warmup()
                    .ExpectedCount(testData.ExpectedAllocationCount)
                    .Run();
            }
        }

        static PropertyPath CreateFromString(string path)
        {
            return new PropertyPath(path);
        }
    }
}
