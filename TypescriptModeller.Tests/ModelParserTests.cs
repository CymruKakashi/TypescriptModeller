using TypescriptModeller;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace TypescriptModeller.Tests
{
    public class ModelParserTests
    {
        ModelParser parser;
        [SetUp]
        public void Setup()
        {
            parser = new ModelParser();
        }

        [Test]
        public void Assert_GetRoot_HandlesNullInput()
        {
            CompilationUnitSyntax expected = null;
            CompilationUnitSyntax result = parser.GetRoot(null);
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void Assert_GetRoot_HandlesBadInput()
        {
            var code = "Clearly not code";
            Assert.DoesNotThrow(() => parser.GetRoot(code),"Get root threw an exception handling a non code string");
        }

        [Test]
        public void Assert_VisitPropertyDeclaration_HandlesNullInput()
        {
           Assert.DoesNotThrow(() => parser.VisitPropertyDeclaration(null),"VisitPropertyDeclaration threw an exception on a null input");
        }

        [Test]
        public void Assert_VisitPropertyDeclaration_BlankInterfaceIgnored()
        {
            string code = @"
using System;
using System.Runtime.Serialization;

namespace Project.Models
{
    [DataContract]
    public partial class Account: IAccount
    {

    }
    
    public interface IAccount
    {
    }
}
            ";
            parser.Visit(parser.GetRoot(code));
            Assert.AreEqual(0,parser.models.Count);
        }

        [Test]
        public void Assert_VisitPropertyDeclaration_SingleInterfaceFound()
        {
            string code = @"
using System;
using System.Runtime.Serialization;

namespace Project.Models
{
    [DataContract]
    public partial class Account: IAccount
    {

    }
    
    public interface IAccount
    {
        [DataMember]
        Guid Id { get; set; }
    }
}
            ";
            parser.Visit(parser.GetRoot(code));
            Assert.AreEqual(1,parser.models.Count);
            Assert.AreEqual("IAccount",parser.models.First().Key);
        }

        [Test]
        public void Assert_VisitPropertyDeclaration_MultiInterfaceFound()
        {
            string code = @"
using System;
using System.Runtime.Serialization;

namespace Project.Models
{
    [DataContract]
    public partial class Account: IAccount
    {

    }
    
    public interface IAccount
    {
        [DataMember]
        Guid Id { get; set; }
    }

    public interface IUser
    {
        [DataMember]
        Guid Id { get; set; }
    }
}
            ";
            parser.Visit(parser.GetRoot(code));
            Assert.AreEqual(2,parser.models.Count);
            Assert.AreEqual("IAccount",parser.models.First().Key);
        }

        [Test]
        public void Assert_VisitPropertyDeclaration_InterfaceAttributesFound()
        {
            string code = @"
using System;
using System.Runtime.Serialization;

namespace Project.Models
{
    [DataContract]
    public partial class Account: IAccount
    {

    }
    
    public interface IAccount
    {
        [DataMember]
        Guid Id { get; set; }
        
        [DataMember]
        string Name { get; set; }

        [DataMember]
        Guid? ClientId { get; set; }
    }
}
            ";
            parser.Visit(parser.GetRoot(code));
            List<AttributeInfo> attributeList = parser.models["IAccount"];
            Assert.AreEqual(3,attributeList.Count);
            Assert.AreEqual("Guid",attributeList.Where(x => x.name == "id").First().type);
            Assert.AreEqual("string",attributeList.Where(x => x.name == "name").First().type);
            Assert.AreEqual("Guid?",attributeList.Where(x => x.name == "clientId").First().type);
        }

                [Test]
        public void Assert_VisitPropertyDeclaration_AllAttributesFound()
        {
            string code = @"
using System;
using System.Runtime.Serialization;

namespace Project.Models
{
    public class CustomObject
    {

    }
    [DataContract]
    public partial class Account: IAccount
    {

    }
    
    public interface IAccount
    {
        [DataMember]
        byte byteAttribute { get; set; }
        
        [DataMember]
        byte? nullByteAttribute { get; set; }

        [DataMember]
        sbyte sbyteAttribute { get; set; }

        [DataMember]
        sbyte? nullSbyteAttribute { get; set; }

        [DataMember]
        short shortAttribute { get; set; }

        [DataMember]
        short? nullShortAttribute { get; set; }

        [DataMember]
        ushort ushortAttribute { get; set; }

        [DataMember]
        ushort? nullUshortAttribute { get; set; }

        [DataMember]
        int intAttribute { get; set; }

        [DataMember]
        int? nullIntAttribute { get; set; }

        [DataMember]
        uint uintAttribute { get; set; }

        [DataMember]
        uint? nullUintAttribute { get; set; }

        [DataMember]
        long longAttribute { get; set; }

        [DataMember]
        long? nullLongAttribute { get; set; }

        [DataMember]
        ulong ulongAttribute { get; set; }

        [DataMember]
        ulong? nullUlongAttribute { get; set; }

        [DataMember]
        float floatAttribute { get; set; }

        [DataMember]
        float? nullFloatAttribute { get; set; }

        [DataMember]
        double doubleAttribute { get; set; }

        [DataMember]
        double? nullDoubleAttribute { get; set; }

        [DataMember]
        decimal decimalAttribute { get; set; }

        [DataMember]
        decimal? nullDecimalAttribute { get; set; }

        [DataMember]
        char charAttribute { get; set; }

        [DataMember]
        char? nullCharAttribute { get; set; }

        [DataMember]
        bool boolAttribute { get; set; }

        [DataMember]
        bool? nullBoolAttribute { get; set; }

        [DataMember]
        string stringAttribute { get; set; }

        [DataMember]
        object objectAttribute { get; set; }

        [DataMember]
        DateTime dateTimeAttribute { get; set; }

        [DataMember]
        CustomObject customObjectAttribute { get; set; }
    }
}
            ";
            parser.Visit(parser.GetRoot(code));
            List<AttributeInfo> attributeList = parser.models["IAccount"];
            Assert.AreEqual(30,attributeList.Count);
            Assert.AreEqual("byte",attributeList.Where(x => x.name == "byteAttribute").First().type);
            Assert.AreEqual("byte?",attributeList.Where(x => x.name == "nullByteAttribute").First().type);
            Assert.AreEqual("sbyte",attributeList.Where(x => x.name == "sbyteAttribute").First().type);
            Assert.AreEqual("sbyte?",attributeList.Where(x => x.name == "nullSbyteAttribute").First().type);
            Assert.AreEqual("short",attributeList.Where(x => x.name == "shortAttribute").First().type);
            Assert.AreEqual("short?",attributeList.Where(x => x.name == "nullShortAttribute").First().type);
            Assert.AreEqual("ushort",attributeList.Where(x => x.name == "ushortAttribute").First().type);
            Assert.AreEqual("ushort?",attributeList.Where(x => x.name == "nullUshortAttribute").First().type);
            Assert.AreEqual("int",attributeList.Where(x => x.name == "intAttribute").First().type);
            Assert.AreEqual("int?",attributeList.Where(x => x.name == "nullIntAttribute").First().type);
            Assert.AreEqual("uint",attributeList.Where(x => x.name == "uintAttribute").First().type);
            Assert.AreEqual("uint?",attributeList.Where(x => x.name == "nullUintAttribute").First().type);
            Assert.AreEqual("long",attributeList.Where(x => x.name == "longAttribute").First().type);
            Assert.AreEqual("long?",attributeList.Where(x => x.name == "nullLongAttribute").First().type);
            Assert.AreEqual("ulong",attributeList.Where(x => x.name == "ulongAttribute").First().type);
            Assert.AreEqual("ulong?",attributeList.Where(x => x.name == "nullUlongAttribute").First().type);
            Assert.AreEqual("float",attributeList.Where(x => x.name == "floatAttribute").First().type);
            Assert.AreEqual("float?",attributeList.Where(x => x.name == "nullFloatAttribute").First().type);
            Assert.AreEqual("double",attributeList.Where(x => x.name == "doubleAttribute").First().type);
            Assert.AreEqual("double?",attributeList.Where(x => x.name == "nullDoubleAttribute").First().type);
            Assert.AreEqual("decimal",attributeList.Where(x => x.name == "decimalAttribute").First().type);
            Assert.AreEqual("decimal?",attributeList.Where(x => x.name == "nullDecimalAttribute").First().type);
            Assert.AreEqual("char",attributeList.Where(x => x.name == "charAttribute").First().type);
            Assert.AreEqual("char?",attributeList.Where(x => x.name == "nullCharAttribute").First().type);
            Assert.AreEqual("bool",attributeList.Where(x => x.name == "boolAttribute").First().type);
            Assert.AreEqual("bool?",attributeList.Where(x => x.name == "nullBoolAttribute").First().type);
            Assert.AreEqual("object",attributeList.Where(x => x.name == "objectAttribute").First().type);
            Assert.AreEqual("string",attributeList.Where(x => x.name == "stringAttribute").First().type);
            Assert.AreEqual("DateTime",attributeList.Where(x => x.name == "dateTimeAttribute").First().type);
            Assert.AreEqual("CustomObject",attributeList.Where(x => x.name == "customObjectAttribute").First().type);
        }





    }
}