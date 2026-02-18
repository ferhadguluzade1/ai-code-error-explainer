namespace Web.Services
{
    public class CodeFixService
    {
        public (string primaryFix, string alternativeFix) GenerateFix(string errorMessage, string codeSnippet, string mode)
        {
            errorMessage = errorMessage.ToLower();

            if (errorMessage.Contains("nullreference"))
            {
                if (mode == "beginner")
                {
                    return (
        @"// Beginner fix
if(obj != null)
{
    Console.WriteLine(obj.ToString());
}",
        @"// Alternative beginner fix
if(obj == null)
{
    Console.WriteLine(""Object is null"");
}"
                    );
                }
                else
                {
                    return (
        @"// Developer fix
Console.WriteLine(obj?.ToString());",
        @"// Alternative developer fix
var safeValue = obj ?? new object();
Console.WriteLine(safeValue.ToString());"
                    );
                }
            }

            if (errorMessage.Contains("indexoutofrange"))
            {
                if (mode == "beginner")
                {
                    return (
        @"// Beginner fix
for(int i = 0; i < arr.Length; i++)
{
    Console.WriteLine(arr[i]);
}",
        @"// Alternative beginner fix
Console.WriteLine(""Check array size before access"");"
                    );
                }
                else
                {
                    return (
        @"// Developer fix
if(index >= 0 && index < arr.Length)
{
    Console.WriteLine(arr[index]);
}",
        @"// Alternative developer fix
arr.ElementAtOrDefault(index);"
                    );
                }
            }

            return ("// No automatic fix available yet.", "");
        }


    }
}
