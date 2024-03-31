using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.Services
{
    public static class PreferenceManager
    {
        private const string lastSymptomCheckDate = "lastSymptomCheckDate";

        public static void UpdateLastSymptomCheckDate()
        {
            Preferences.Set(lastSymptomCheckDate, DateTime.Today);
        }

        public static DateTime GetLastSymptomCheckDate()
        {
            return Preferences.Get(lastSymptomCheckDate, DateTime.MinValue);
        }
    }
}
