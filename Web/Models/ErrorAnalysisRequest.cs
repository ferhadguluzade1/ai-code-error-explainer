namespace Web.Models
{
    public class ErrorAnalysisRequest
    {
        public string ErrorMessage { get; set; }
        public string CodeSnippet { get; set; }
        public string ExplanationMode { get; set; } // YENİ
    }
}
