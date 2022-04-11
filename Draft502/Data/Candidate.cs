using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draft502.Data
{
    public class Candidate
    {
        public string CandidateName { get; set; }
        public bool Matched { get; set; }
        public List<string> Preferences { get; set; }
        public string MatchedToEmployerName { get; set; }
        public int MatchedEmployerRank { get; set; }
        public int MatchedCandidateRankFromEmployer { get; set; }

        public static void AddCandidate(ref List<Candidate> CandidateList, string Name, string Prefs)
        {
            Candidate c = new Candidate();         
            c.CandidateName = Name;
            string[] prefsArray = Prefs.Split(';');   
           
            c.Preferences = prefsArray.ToList<string>();
            if(CandidateList == null)
            {
                CandidateList = new List<Candidate>();
            }
            CandidateList.Add(c);
        }
    }

    
}
