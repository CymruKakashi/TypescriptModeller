using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TypescriptModeller
{
    class Program
    {
        static void Main(string[] args)
        {
            string source;
            string output;
            try
            {
                source = args[0];
                output = args[1];
            }catch(Exception e){
                Console.WriteLine("Please provide both the input and the output folder.");
                return;
            }
            //read all of the files and build a list of models in memory.
            foreach (var path in Directory.GetFiles(source))
            {
                string filecontent = File.ReadAllText(path);
                string outcontent = "";
                var parser = new ModelParser();
                parser.Visit(parser.GetRoot(filecontent));
                Console.WriteLine(JsonSerializer.Serialize(parser.models));
                //loop through the models and build the codefile
                foreach(KeyValuePair<string, List<AttributeInfo>> pair in parser.models)
                {
                    using(FileStream file = File.Open(output+"//"+pair.Key.Substring(1)+".ts",FileMode.OpenOrCreate))
                    {
                        outcontent = TypescriptWriter.WriteClass(pair);
                        byte[] outbytes = Encoding.UTF8.GetBytes(outcontent);
                        file.Write(outbytes,0,outbytes.Length);
                    }
                }
            }
            
            
        }
    }
}
