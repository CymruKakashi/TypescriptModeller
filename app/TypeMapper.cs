namespace TypescriptModeller
{
    public static class TypeMapper
    {
        public static string MapType(string type)
        {
            switch(type)
            {
                case "Guid":
                case "Guid?":
                case "string":
                case "char":
                    return "string";
                case "int":
                case "int?":
                case "float":
                case "float?":
                case "short":
                case "short?":
                case "uint":
                case "uint?":
                case "long":
                case "long?":
                case "ulong?":
                case "ulong":
                case "decimal":
                case "decimal?":
                    return "number";
                case "DateTime":
                case "DateTime?":
                    return "Date";
                case "bool":
                    return "boolean";
                default: 
                    return "string";
            }
        }
    }
}