using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ErrorHistory
    {
        [Key]
        public int Id { get; set; }

        public string ErrorMessage { get; set; }

        public string CodeSnippet { get; set; }

        public string Explanation { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}