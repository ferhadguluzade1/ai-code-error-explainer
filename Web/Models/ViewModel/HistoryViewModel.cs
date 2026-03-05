using System.Collections.Generic;

namespace Web.Models
{
    public class HistoryViewModel
    {
        public List<ErrorHistory> Errors { get; set; }

        public List<CommonErrorViewModel> CommonErrors { get; set; }

        public List<string> Warnings { get; set; }

        public List<string> LearningInsights { get; set; }

        public List<string> TrendInsights { get; set; }

        public object Stats { get; set; }

        public List<string> ProgressInsights { get; set; }

        public BehaviorInsights Behavior { get; set; }

        public string SkillLevel { get; set; }
    }

    public class CommonErrorViewModel
    {
        public string Error { get; set; }
        public int Count { get; set; }
        public DateTime LastActivity { get; set; }
    }
}