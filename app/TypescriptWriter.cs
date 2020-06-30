using System.Collections.Generic;
using System.IO;

namespace TypescriptModeller
{
    public static class TypescriptWriter
    {
        private static readonly string newline = "\r\n";
        private static readonly string tab = "\t";
        public static string WriteClass(KeyValuePair<string, List<AttributeInfo>> pair)
        {
            string outcontent;
            string classname = pair.Key.Substring(1); // cut off the I part of the interface
            //write the opening of the class 
            outcontent = OpenClass(classname);
            //write the attributes
            outcontent += WriteAttributes(pair);
            //write the constructor
            outcontent += WriteConstructor(pair);
            //write the closing part of the class
            outcontent += CloseClass();
            return outcontent;
        }
        private static string OpenClass(string classname)
        {
            classname = char.ToUpper(classname[0]) + classname.Substring(1);
            return  "export class "+classname+newline+
                        "{" + newline;
        }

        private static string WriteAttributes(KeyValuePair<string, List<AttributeInfo>> pair)
        {
            string returnstring = "";
            foreach(AttributeInfo info in pair.Value)
            {
                returnstring += tab+info.name+":"+TypeMapper.MapType(info.type)+";"+newline;
            }
            return returnstring;
        }

        private static string WriteConstructor(KeyValuePair<string, List<AttributeInfo>> pair)
        {
            string returnstring = "";
            returnstring += newline +
                        tab+"constructor(data:any)"+ newline +
                        tab+"{"+ newline;
            foreach(AttributeInfo info in pair.Value)
            {
                returnstring += tab+tab+"this."+info.name+" = data?."+info.name+";"+newline;
            }
            //close the constructor
            returnstring += tab+"}";
            return returnstring;
        }

        private static string CloseClass()
        {
            return newline 
                    + "}";
        }
    }
}