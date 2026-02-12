using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey = "AIzaSyB7FEfT16B8FCOfIv-n0HGyD9XDGDjw0ww";

        public async Task<string> AnalyzeErrorAsync(string input, string mode)
        {
            var prompt = mode == "beginner"
                ? $"Explain this programming error simply and give a fixed code example:\n{input}"
                : $"Analyze this programming error technically and give improved fixed code:\n{input}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_apiKey}"
            );

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);

            var aiText = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return aiText;
        }
    }
}
