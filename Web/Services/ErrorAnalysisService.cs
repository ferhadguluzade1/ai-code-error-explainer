namespace Web.Services
{
    public class ErrorAnalysisService
    {
        public (string explanation, string suggestion, int confidence) Analyze(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return ("No error provided.", "Please paste an error message.", 0);

            errorMessage = errorMessage.ToLower();

            if (errorMessage.Contains("nullreference"))
            {
                return (
                    "This error occurs when you try to use an object that is null.",
                    "Check where the object is initialized before using it.",
                    90
                );
            }

            if (errorMessage.Contains("indexoutofrange"))
            {
                return (
                    "This error happens when you try to access an array index that doesn't exist.",
                    "Ensure your loop or index is within bounds.",
                    85
                );
            }

            if (errorMessage.Contains("dividebyzero"))
            {
                return (
                    "A number is being divided by zero, which is not allowed.",
                    "Ensure the divisor is not zero before division.",
                    80
                );
            }

            if (errorMessage.Contains("syntax"))
            {
                return (
                    "There is a syntax mistake in your code.",
                    "Check for missing brackets, semicolons or typos.",
                    80
                );
            }

            if (errorMessage.Contains("typeerror"))
            {
                return (
                    "Type mismatch detected in your code.",
                    "Check variable types and function arguments.",
                    75
                );
            }

            return (
                "Error not recognized locally.",
                "AI analysis required.",
                20
            );
        }
    }
}
