using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Web.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OpenAiService(IConfiguration config)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _http = new HttpClient(handler);

            _apiKey = config["OpenAI:ApiKey"];
        }

        public async Task<string> AnalyzeAsync(string input, string mode)
        {
            var prompt =
$@"You are an AI programming mentor.

Explain the following error in {mode} mode.

Error:
{input}

Give:
- Explanation
- Root cause
- Best practice fix
";

            var body = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.openai.com/v1/chat/completions"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );
            HttpResponseMessage response;

            try
            {
                response = await _http.SendAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception("OPENAI CONNECTION ERROR: " + ex.Message);
            }
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new Exception(err);
            }
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}