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
            Assert.AreEqual("Guid?",attributeList.Where(x => x.name == "clientid").First().type);
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
            Assert.AreEqual("byte",attributeList.Where(x => x.name == "byteattribute").First().type);
            Assert.AreEqual("byte?",attributeList.Where(x => x.name == "nullbyteattribute").First().type);
            Assert.AreEqual("sbyte",attributeList.Where(x => x.name == "sbyteattribute").First().type);
            Assert.AreEqual("sbyte?",attributeList.Where(x => x.name == "nullsbyteattribute").First().type);
            Assert.AreEqual("short",attributeList.Where(x => x.name == "shortattribute").First().type);
            Assert.AreEqual("short?",attributeList.Where(x => x.name == "nullshortattribute").First().type);
            Assert.AreEqual("ushort",attributeList.Where(x => x.name == "ushortattribute").First().type);
            Assert.AreEqual("ushort?",attributeList.Where(x => x.name == "nullushortattribute").First().type);
            Assert.AreEqual("int",attributeList.Where(x => x.name == "intattribute").First().type);
            Assert.AreEqual("int?",attributeList.Where(x => x.name == "nullintattribute").First().type);
            Assert.AreEqual("uint",attributeList.Where(x => x.name == "uintattribute").First().type);
            Assert.AreEqual("uint?",attributeList.Where(x => x.name == "nulluintattribute").First().type);
            Assert.AreEqual("long",attributeList.Where(x => x.name == "longattribute").First().type);
            Assert.AreEqual("long?",attributeList.Where(x => x.name == "nulllongattribute").First().type);
            Assert.AreEqual("ulong",attributeList.Where(x => x.name == "ulongattribute").First().type);
            Assert.AreEqual("ulong?",attributeList.Where(x => x.name == "nullulongattribute").First().type);
            Assert.AreEqual("float",attributeList.Where(x => x.name == "floatattribute").First().type);
            Assert.AreEqual("float?",attributeList.Where(x => x.name == "nullfloatattribute").First().type);
            Assert.AreEqual("double",attributeList.Where(x => x.name == "doubleattribute").First().type);
            Assert.AreEqual("double?",attributeList.Where(x => x.name == "nulldoubleattribute").First().type);
            Assert.AreEqual("decimal",attributeList.Where(x => x.name == "decimalattribute").First().type);
            Assert.AreEqual("decimal?",attributeList.Where(x => x.name == "nulldecimalattribute").First().type);
            Assert.AreEqual("char",attributeList.Where(x => x.name == "charattribute").First().type);
            Assert.AreEqual("char?",attributeList.Where(x => x.name == "nullcharattribute").First().type);
            Assert.AreEqual("bool",attributeList.Where(x => x.name == "boolattribute").First().type);
            Assert.AreEqual("bool?",attributeList.Where(x => x.name == "nullboolattribute").First().type);
            Assert.AreEqual("object",attributeList.Where(x => x.name == "objectattribute").First().type);
            Assert.AreEqual("string",attributeList.Where(x => x.name == "stringattribute").First().type);
            Assert.AreEqual("DateTime",attributeList.Where(x => x.name == "datetimeattribute").First().type);
            Assert.AreEqual("CustomObject",attributeList.Where(x => x.name == "customobjectattribute").First().type);
        }





    }
}