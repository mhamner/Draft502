using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draft502.Data
{
    public class EmployerSummary
    {
        public string EmployerName { get; set; }
        public int NumberOfSlotsAvailable { get; set; }
        public int NumberOfSlotsFilled { get; set; }
        public string[] EmployerPreferences { get; set; }
        public List<string> MatchedCandidates { get; set; }

        public static void AddEmployerSummary(ref List<EmployerSummary> EmployerSummaryList, string Name, int Slots, string Prefs)
        {
            EmployerSummary es = new EmployerSummary();
            es.EmployerName = Name;
            es.NumberOfSlotsAvailable = Slots;               
            string[] prefsArray = Prefs.Split(';');
            
            es.EmployerPreferences = prefsArray;

            if(EmployerSummaryList == null)
            {
                EmployerSummaryList = new List<EmployerSummary>();
            }

            EmployerSummaryList.Add(es);
        }
    }
}
