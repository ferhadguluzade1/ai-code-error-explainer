using System.Net.Http;
using System.Text;
using System.Text.Json;
using Web.Models;

namespace Web.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiKey = config["OpenAI:ApiKey"];
        }

        public async Task<AiStructuredResponse> AnalyzeStructuredAsync(string input, string mode)
        {
            try
            {
                var prompt = $@"
Return ONLY valid JSON.
Do NOT include explanations outside JSON.

Format:

{{
  ""Explanation"": ""..."",
  ""RootCause"": ""..."",
  ""BestPractice"": ""..."",
  ""FixedCode"": ""..."",
  ""AlternativeFix"": ""...""
}}

Mode: {mode}

Input:
{input}
";

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a professional coding assistant." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.2
                };

                var json = JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post,
                    "https://api.openai.com/v1/chat/completions");

                request.Headers.Add("Authorization", $"Bearer {_apiKey}");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseString);

                var content = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                var structured = JsonSerializer.Deserialize<AiStructuredResponse>(content);

                return structured;
            }
            catch
            {
                return null;
            }
        }
    }
}