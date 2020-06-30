using NUnit.Framework;
using TypescriptModeller;

namespace TypescriptModeller.Tests
{
    public class TypeMapperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Assert_MapType_StringMapping()
        {
            string expectedType = "string";
            Assert.AreEqual(expectedType,TypeMapper.MapType("Guid"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("Guid?"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("string"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("char"));
        }

        [Test]
        public void Assert_MapType_NumberMapping()
        {
            string expectedType = "number";
            Assert.AreEqual(expectedType,TypeMapper.MapType("int"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("int?"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("float"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("float?"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("short"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("short?"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("uint"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("uint?"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("long"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("long?"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("ulong?"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("ulong"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("decimal"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("decimal?"));
        }

        [Test]
        public void Assert_MapType_DateMapping()
        {
            string expectedType = "Date";
            Assert.AreEqual(expectedType,TypeMapper.MapType("DateTime"));
            Assert.AreEqual(expectedType,TypeMapper.MapType("DateTime?"));
        }

        [Test]
        public void Assert_MapType_BoolMapping()
        {
            string expectedType = "boolean";
            Assert.AreEqual(expectedType,TypeMapper.MapType("bool"));
        }

        [Test]
        public void Assert_MapType_DefaultMapping()
        {
            string expectedType = "string";
            Assert.AreEqual(expectedType,TypeMapper.MapType("NotSupported"));
        }
    }
}