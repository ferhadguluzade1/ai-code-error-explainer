using Web.Models;

namespace Web.Services
{
    public class CodeFixService
    {
        public FixResult GenerateFix(
            string errorMessage,
            string codeSnippet,
            string mode)
        {
            if (mode == "beginner")
            {
                return new FixResult
                {
                    FixedCode =
@"if(obj != null)
{
    Console.WriteLine(obj.ToString());
}",

                    AlternativeFix =
@"if(obj == null)
{
    Console.WriteLine(""Object is null"");
}"
                };
            }

            return new FixResult
            {
                FixedCode =
@"Console.WriteLine(obj?.ToString());",

                AlternativeFix =
@"var safeValue = obj ?? new object();
Console.WriteLine(safeValue.ToString());"
            };
        }
    }
}