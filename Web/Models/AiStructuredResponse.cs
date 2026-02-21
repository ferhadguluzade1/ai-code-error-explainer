namespace Web.Models
{
    public class AiStructuredResponse
    {
        public string Explanation { get; set; }
        public string RootCause { get; set; }
        public string BestPractice { get; set; }
        public string FixedCode { get; set; }
        public string AlternativeFix { get; set; }
    }
}