﻿using System;
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
    }
}
