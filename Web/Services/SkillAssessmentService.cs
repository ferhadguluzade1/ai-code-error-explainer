using System.Linq;
using Web.Models;

namespace Web.Services
{
    public class SkillAssessmentService
    {
        private readonly ErrorHistoryService _history;

        public SkillAssessmentService(ErrorHistoryService history)
        {
            _history = history;
        }

        public string GetSkillLevel()
        {
            var history = _history.GetAll();

            if (!history.Any())
                return "Beginner";

            int total = history.Count;

            int nullRefs = history.Count(x =>
                x.ErrorMessage.Contains("NullReferenceException"));

            if (total < 5)
                return "Beginner";

            if (nullRefs > total * 0.5)
                return "Learning Debugging";

            if (total > 15)
                return "Intermediate";

            if (total > 30)
                return "Advanced";

            return "Growing Developer";
        }
    }
}