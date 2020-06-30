using System.Collections.Generic;
using NUnit.Framework;
using TypescriptModeller;

namespace TypescriptModeller.Tests
{
    public class TypescriptWriterTests
    {
        private static readonly string newline = "\r\n";
        private static readonly string tab = "\t";
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Assert_OpenClass_HandlesNoAttributes()
        {
            var list = new List<AttributeInfo>();
            string expected = "export class Test\r\n{\r\n\r\n\tconstructor(data:any)\r\n\t{\r\n\t}\r\n}";
            KeyValuePair<string, List<AttributeInfo>> pair = new KeyValuePair<string, List<AttributeInfo>>("Itest", list);
            string result = TypescriptWriter.WriteClass(pair);
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Assert_OpenClass_WritesCorrectClass()
        {
            string code = $"export class Test{newline}{{{newline}";
            var list = new List<AttributeInfo>();
            list.Add(new AttributeInfo{
                name = "id",
                type = "Guid"
            });
            KeyValuePair<string, List<AttributeInfo>> pair = new KeyValuePair<string, List<AttributeInfo>>("Itest", list);
            string result = TypescriptWriter.WriteClass(pair);
            Assert.IsTrue(result.Contains(code));
            Assert.AreEqual(code,result.Substring(0,code.Length));
        }

        [Test]
        public void Assert_OpenClass_WritesClassClose()
        {
            string code = $"{newline}}}";
            var list = new List<AttributeInfo>();
            list.Add(new AttributeInfo{
                name = "id",
                type = "Guid"
            });
            KeyValuePair<string, List<AttributeInfo>> pair = new KeyValuePair<string, List<AttributeInfo>>("Itest", list);
            string result = TypescriptWriter.WriteClass(pair);
            Assert.IsTrue(result.Contains(code));
            Assert.AreEqual(code,result.Substring(result.Length-code.Length,code.Length));
        }

        [Test]
        public void Assert_OpenClass_WritesCorrectConstructor()
        {
            string code = $"{tab}constructor(data:any){newline}{tab}{{{newline}{tab}{tab}this.id = data?.id;{newline}{tab}}}";
            var list = new List<AttributeInfo>();
            list.Add(new AttributeInfo{
                name = "id",
                type = "Guid"
            });
            KeyValuePair<string, List<AttributeInfo>> pair = new KeyValuePair<string, List<AttributeInfo>>("Itest", list);
            string result = TypescriptWriter.WriteClass(pair);
            Assert.IsTrue(result.Contains(code));
        }

        [Test]
        public void Assert_OpenClass_WritesCorrectAttribute()
        {
            string code = $"{newline}{tab}id:string;{newline}";
            var list = new List<AttributeInfo>();
            list.Add(new AttributeInfo{
                name = "id",
                type = "Guid"
            });
            KeyValuePair<string, List<AttributeInfo>> pair = new KeyValuePair<string, List<AttributeInfo>>("Itest", list);
            string result = TypescriptWriter.WriteClass(pair);
            Assert.IsTrue(result.Contains(code));
        }


    }
}