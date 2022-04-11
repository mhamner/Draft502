using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Draft502.Data;

namespace Draft502
{
    public class ReportCreator
    {
        /// <summary>
        /// Writes the Employer Summary List to the file specificed
        /// </summary>
        /// <param name="EmployerSummaryList">List of Employer Summary records</param>
        /// <param name="FilePath">Full file path to write to</param>
        public static void CreateResultsReport(ref List<Candidate> CandidateList, ref List<Employer> EmployerList,
            ref List<EmployerSummary> EmployerSummaryList, string FilePath)
        {
            //Create the containing folder if it doesn't already exist
            FileInfo fi = new FileInfo(FilePath);
            fi.Directory.Create();

            string fileName = $"Draft Results - RunDate_{DateTime.Now.ToString("u").Replace(':', '-')}.txt";
            using (System.IO.StreamWriter file = new StreamWriter(Path.Combine(@FilePath, fileName)))
            {
                file.WriteLine("Employer Selections");
                file.WriteLine("----------------------------------------------");

                //Write the detail lines
                foreach (EmployerSummary es in EmployerSummaryList)
                {
                    file.WriteLine($"Employer {es.EmployerName} has matched the following Candidates:");

                    if (es.MatchedCandidates != null)
                    {
                        foreach (string s in es.MatchedCandidates)
                        {
                            file.WriteLine(s);
                        }
                    }
                    else
                    {
                        file.WriteLine("No Candidates Matched.  This can sometimes indicate a problem.");
                        file.WriteLine("Make sure there are no candidates in the employer's preference list who don't have " +
                            "a candidate record entered.");
                        file.WriteLine("Also, check for misspellings in the employer's preference list.");
                    }
                    file.WriteLine("----------------------------------------------");
                }

                //Write the summary lines
                if (EmployerList.Count == 0)
                {
                    file.WriteLine("ALL EMPLOYERS FILLED ALL THEIR OPEN SLOTS.");
                }

                if (CandidateList.Count == 0)
                {
                    file.WriteLine("ALL CANDIDATES WERE ABLE TO SECURE SLOTS.");
                }

                //If not all employers got all their matches, notify the user.
                foreach (Employer el in EmployerList)
                {
                    file.WriteLine($"Employer {el.EmployerName} was not able to fill all their slots.");
                }

                foreach (Candidate can in CandidateList)
                {
                    file.WriteLine($"Candidate {can.CandidateName} was not able to secure a spot.");
                }
            }
            //Open the file
            Process.Start(Path.Combine(FilePath, fileName));
        }

        public static void CreateLoopReport(ref List<Candidate> CandidateList, ref List<Employer> EmployerList,
            ref List<EmployerSummary> EmployerSummaryList, string FilePath)
        {
            //Create the containing folder if it doesn't already exist
            FileInfo fi = new FileInfo(FilePath);
            fi.Directory.Create();

            string fileName = $"Draft Results - RunDate_{DateTime.Now.ToString("u").Replace(':', '-')}.txt";
            using (System.IO.StreamWriter file = new StreamWriter(Path.Combine(@FilePath, fileName)))
            {
                file.WriteLine("ERROR:  Possible Loop Condition Detected.");
                file.WriteLine("----------------------------------------------");
                file.WriteLine("Recommended:  Check for name misspellings.");
                file.WriteLine("----------------------------------------------");

                file.WriteLine("The following candidates were unmatched:");
                foreach(Candidate c in CandidateList)
                {
                    file.WriteLine($"Name: {c.CandidateName}, Preferences:");
                    foreach(string s in c.Preferences)
                    {
                        file.WriteLine(s);
                    }
                }

                file.WriteLine();

                file.WriteLine("The following employers were not fully matched:");
                foreach(Employer e in EmployerList)
                {             
                    file.WriteLine($"Employer Name: {e.EmployerName}, Preferences:");
                    foreach(string p in e.Preferences)
                    {
                        file.WriteLine(p);
                    }
                    
                }           
            }
            //Open the file
            Process.Start(Path.Combine(FilePath, fileName));
        }
    }
}
