using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draft502.Data
{
    public class Employer
    {
        public string EmployerName { get; set; }
        public bool Matched { get; set; }
        public string[] Preferences { get; set; }
        public string Preference1 { get; set; }
        public string Preference2 { get; set; }
        public string Preference3 { get; set; }
        public string Preference4 { get; set; }
        public string MatchedToCandidateName { get; set; }
        public int MatchedCandidateRank { get; set; }   
        
        public static void AddEmployer(ref List<Employer> EmployerList, string Name, string[] Prefs)
        {
            Employer e = new Employer();
            e.EmployerName = Name;
            e.Matched = false;
            e.MatchedCandidateRank = 0;
            e.MatchedToCandidateName = "";
            e.Preferences = Prefs;

            if(EmployerList == null)
            {
                EmployerList = new List<Employer>();
            }

            EmployerList.Add(e);           
        }
    }
}
