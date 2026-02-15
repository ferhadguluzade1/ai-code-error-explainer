using System.Collections.Generic;
using Web.Models;

namespace Web.Services
{
    public class ErrorHistoryService
    {
        private static List<ErrorHistory> _history = new List<ErrorHistory>();

        public void Add(ErrorHistory item)
        {
            _history.Add(item);
        }

        public List<ErrorHistory> GetAll()
        {
            return _history;
        }
    }
}
