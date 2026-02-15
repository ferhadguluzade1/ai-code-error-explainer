using System;

namespace Web.Models
{
    public class ErrorHistory
    {
        public string ErrorMessage { get; set; }

        public string CodeSnippet { get; set; }

        public string Explanation { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
