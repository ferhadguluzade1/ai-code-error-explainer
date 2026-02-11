using System.Threading.Tasks;

namespace Web.Services
{
    public class OpenAiService
    {
        public async Task<string> AnalyzeErrorAsync(string errorMessage)
        {
            await Task.Delay(800); // AI düşünürmüş kimi effekt

            errorMessage = errorMessage.ToLower();

            if (errorMessage.Contains("null"))
            {
                return "AI: This error usually means an object was not initialized. Check where it becomes null.";
            }

            if (errorMessage.Contains("index"))
            {
                return "AI: You are likely accessing an array position that does not exist.";
            }

            if (errorMessage.Contains("type"))
            {
                return "AI: A type mismatch occurred. Verify variable and parameter types.";
            }

            return "AI: I could not fully understand the error, but it may relate to incorrect logic or missing initialization.";
        }
    }
}
