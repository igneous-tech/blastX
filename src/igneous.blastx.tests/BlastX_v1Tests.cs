using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using igneous.blastx.v1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace igneous.blastx.tests
{
    [TestClass]
    public class BlastX_v1Tests
    {
        static bool _hasJsonProperties(PropertyInfo propInfo) =>
            propInfo.GetCustomAttributes(typeof(JsonPropertyAttribute), false).Any();

        static bool _hasJsonProperties(Type type) =>
            type.GetProperties().Any(_hasJsonProperties);

        [TestMethod]
        public void AutoRoundTripTesting()
        {
            var atLeastOneTestWasRun = false;

            // Iterate over the various json serializeable types and make sure doing a
            // round trip works out of the box
            var types = typeof(Blast).Assembly.GetExportedTypes().Where(_hasJsonProperties);
            foreach (var type in types)
            {
                var obj = Activator.CreateInstance(type);
                var roundTripObj = JsonConvert.DeserializeObject(Blast.ToJson(obj), type);
                Assert.AreEqual(obj, obj, $"The {type} type failed the equality test with itself");
                Assert.AreEqual(obj, roundTripObj, $"The {type} type failed the round trip comparison");

                atLeastOneTestWasRun = true;
            }

            Assert.IsTrue(atLeastOneTestWasRun, "No default round trip tests were run");
        }

        // Super retarded copy for blastX serializable objects
        static T _copy<T>(T obj) => JsonConvert.DeserializeObject<T>(Blast.ToJson(obj));

        bool _isListType(Type type) =>
            type.IsGenericType &&
            type == typeof(List<>).MakeGenericType(type.GenericTypeArguments);

        [TestMethod]
        public void EqualityTests()
        {
            // Ensure equality tests work so that the roundtrip comparisons are reliable
            var sample = new DepthSample { Depth = 9, EastingOffset = 1, NorthingOffset = 2, };
            Assert.AreEqual(sample, sample);
            Assert.AreEqual(sample, _copy(sample));
            Assert.AreNotEqual(sample, new DepthSample { Depth = 8, EastingOffset = 1, NorthingOffset = 2, });
            Assert.AreNotEqual(sample, new DepthSample { Depth = 9, EastingOffset = 2, NorthingOffset = 2, });
            Assert.AreNotEqual(sample, new DepthSample { Depth = 9, EastingOffset = 1, NorthingOffset = 3, });

            var hole = new BlastHole
            {
                Id = "aBlastHole",
                DisplayName = "a blast hole",
                HoleNumber = 5,
                Depth = 12,
                Subdrill = 1,
                Diameter = 125,
                DrillingAngle = 10,
                DrillingBearing = 235,
                CenterLeftOffset = 7,
                CenterTopOffset = 2,
                HoleLoadId = "load1",
                PatternId = "patId1",
                Location = new GeographicalLocation(),
            };
            Assert.AreEqual(hole, hole);
            Assert.AreEqual(hole, _copy(hole));
            Assert.IsTrue(changePropertiesOneByOne(hole));
            foreach (var propInfo in hole.GetType().GetProperties().Where(_hasJsonProperties))
            {
                var holeCopy = _copy(hole);
                var value = propInfo.PropertyType.IsValueType ?
                    Activator.CreateInstance(propInfo.PropertyType) :
                    null;
                propInfo.SetValue(holeCopy, value);
                Assert.AreNotEqual(sample, holeCopy);
            }

            // TODO: extend this test to cover the majority (if not all) of types

            // Set the property values one, by one to a default value and ensure the
            // equality test fails
            bool changePropertiesOneByOne<T>(T obj)
            {
                var testsRun = false;
                foreach (var propInfo in obj.GetType().GetProperties().Where(_hasJsonProperties))
                    if (_isListType(propInfo.PropertyType) == false)
                    {
                        var objCopy = _copy(obj);
                        var value = propInfo.PropertyType.IsValueType ?
                            Activator.CreateInstance(propInfo.PropertyType) :
                            null;
                        if (propInfo.PropertyType == typeof(string))
                            value = string.Empty;
                        propInfo.SetValue(objCopy, value);
                        Assert.AreNotEqual(obj, objCopy);
                        testsRun = true;
                    }

                return testsRun;
            }
        }

        [TestMethod]
        public void GetExtensionData()
        {
            var hole = new BlastHole();
            Assert.IsNull(hole.ExtensionData);

            dynamic data1 = new ExpandoObject();
            hole.GetExtensionData().Add(data1);
            Assert.IsNotNull(hole.ExtensionData);
            Assert.AreEqual(hole.ExtensionData.Count, 1);
            Assert.AreEqual(hole.ExtensionData[0], data1);

            dynamic data2 = new ExpandoObject();
            hole.GetExtensionData().Add(data2);
            Assert.AreEqual(hole.ExtensionData.Count, 2);
            Assert.AreEqual(hole.ExtensionData[0], data1);
            Assert.AreEqual(hole.ExtensionData[1], data2);
        }

        [TestMethod]
        public void AddingExtensionData()
        {
            var blast = new Blast();
            Assert.IsNull(blast.ExtensionData);

            dynamic data = new ExpandoObject();
            data.intProperty = 123;
            data.doubleProperty = 12.34;
            data.strProperty = "He is risen";
            data.dateProperty = new DateTime(2020, 4, 19);
            blast.GetExtensionData().Add(data);

            var unserializedBlast = Blast.FromJson(blast.ToJson());
            Assert.IsNotNull(unserializedBlast.ExtensionData);

            dynamic unserializedData = unserializedBlast.GetExtensionData()[0];
            Assert.AreEqual((int)unserializedData.intProperty, 123);
            Assert.AreEqual((double)unserializedData.doubleProperty, 12.34);
            Assert.AreEqual((string)unserializedData.strProperty, "He is risen");
            Assert.AreEqual((DateTime)unserializedData.dateProperty, data.dateProperty);
        }

        [TestMethod]
        public void ExtensionDataEquality()
        {
            dynamic data1 = new ExpandoObject();
            data1.str1 = "foo";
            data1.str2 = "bar";

            dynamic data2 = new ExpandoObject();
            data2.str1 = "foo";
            data2.str2 = "bar";

            var expando1 = (ExpandoObject)data1;
            var expando2 = (ExpandoObject)data2;

            Assert.IsTrue(Compare.IsEquivalentTo(expando1, expando2));
            Assert.AreEqual(Compare.GetHashCode(data1), Compare.GetHashCode(data2));

            data2.str3 = "";
            Assert.IsFalse(Compare.IsEquivalentTo(expando1, expando2));
            Assert.AreNotEqual(Compare.GetHashCode(data1), Compare.GetHashCode(data2));

            ((IDictionary<string, object>)data2).Remove("str3");
            Assert.IsTrue(Compare.IsEquivalentTo(expando1, expando2));
            Assert.AreEqual(Compare.GetHashCode(data1), Compare.GetHashCode(data2));

            data2.str1 = 123;
            Assert.IsFalse(Compare.IsEquivalentTo(expando1, expando2));
            Assert.AreNotEqual(Compare.GetHashCode(data1), Compare.GetHashCode(data2));
        }

        [TestMethod]
        public void Blast_Get()
        {
            var blast = new Blast();

            var hole1 = new BlastHole { Id = "one" };
            var hole2 = new BlastHole { Id = "two" };
            var hole3 = new BlastHole { Id = "three" };
            blast.Holes.Add(hole1);
            blast.Holes.Add(hole2);
            blast.Holes.Add(hole3);
            Assert.AreEqual(blast.Get<BlastHole>("one"), hole1);
            Assert.AreEqual(blast.Get<BlastHole>("two"), hole2);
            Assert.AreEqual(blast.Get<BlastHole>("three"), hole3);
            Assert.AreEqual(blast.Get<BlastHole>("empty"), null);

            var product1 = new Product { Id = "one" };
            var product2 = new Product { Id = "2" };
            var product3 = new Product { Id = "3" };
            blast.Products.Add(product1);
            blast.Products.Add(product2);
            blast.Products.Add(product3);
            Assert.AreEqual(blast.Get<Product>("one"), product1);
            Assert.AreEqual(blast.Get<Product>("2"), product2);
            Assert.AreEqual(blast.Get<Product>("3"), product3);
        }

        [TestMethod]
        public void GetLength()
        {
            var hole = new BlastHole { Id = "one", Depth = 99 };
            hole.DepthSamples.AddRange(new[]
            {
                new DepthSample { NorthingOffset = 0.1, Depth = 1 },
                new DepthSample { NorthingOffset = -0.1, Depth = 2 },
                new DepthSample { EastingOffset = 0.1, Depth = 3 },
                new DepthSample { EastingOffset = -0.1, Depth = 4 },
            });
            Assert.AreEqual(hole.GetLength(), 4.055, 0.01);

            hole.DepthSamples.Clear();
            Assert.AreEqual(hole.GetLength(), 99);

            hole.Depth = null;
            Assert.AreEqual(hole.GetLength(), 0d);
        }

        [TestMethod]
        public void Validation()
        {
            var blast = new Blast();
            Assert.AreEqual(blast.Validate().Count(), 0);

            var hole = new BlastHole { Id = "one" };
            blast.Holes.Add(hole);
            Assert.AreEqual(blast.Validate().Count(), 0);

            // Should complain about a missing load
            hole.HoleLoadId = "1";
            Assert.AreEqual(blast.Validate().Count(), 1);
            
            hole.HoleLoadId = null;
            Assert.AreEqual(blast.Validate().Count(), 0);

            // Should complain about a duplicate id
            var tmpHole = new BlastHole { Id = hole.Id };
            blast.Holes.Add(tmpHole);
            Assert.AreEqual(blast.Validate().Count(), 1);

            blast.Holes.Remove(tmpHole);
            Assert.AreEqual(blast.Validate().Count(), 0);

            var product = new Product { Id = "one" };
            blast.Products.Add(product);
            Assert.AreEqual(blast.Validate().Count(), 0);

            // Should complain about a duplicate id
            var tmpProduct = new Product { Id = product.Id };
            blast.Products.Add(tmpProduct);
            Assert.AreEqual(blast.Validate().Count(), 1);
            blast.Products.Remove(tmpProduct);

            hole.HoleLoadId = product.Id;
            Assert.AreEqual(blast.Validate().Count(), 0);
        }
    }
}
