using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using log4net;
using Draft502.Data;

namespace Draft502
{
    public class CandidateMatching
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Runs one round of the candidate draft, using the Stable Marriage Algorithm
        /// </summary>
        /// <param name="CandidateList">List of Candidates for this round</param>
        /// <param name="EmployerList">List of Employers</param>
        /// <param name="EmployerSummaryList">Employer Summary records</param>
        /// <returns>bool:  True = All good;  False = Endless loop detected</returns>
        public static bool RunTheDraft(ref List<Candidate> CandidateList, ref List<Employer> EmployerList,
            ref List<EmployerSummary> EmployerSummaryList)
        {
            log.Info($"** Starting the Draft **");
            int loopCount = 0;

            //First, populate the Employer List from the Employer Summary for the first time
            ConstructNewEmployerList(ref EmployerSummaryList, ref EmployerList);

            //Now keep running through and matching until each employer fills all their slots, at which point there will be no more records
            //  in the employer list - OR until we run out of candidates, at which point we're also done
            while (EmployerList.Count > 0 && CandidateList.Count > 0)
            {            
                int employersMatched = 0;

                //Call the Match method until all the employers are matched (can't just go through once, as some employers may have
                //  their match removed if a candidate gets a higher preference match)
                while (employersMatched < EmployerList.Count)
                {
                    MatchCandidates(ref CandidateList, ref EmployerList, ref employersMatched);

                    //The code below guards against an endless loop
                    //int lc = Convert.ToInt32(ConfigurationManager.AppSettings["LoopCount"]);
                    if (loopCount++ > Convert.ToInt32(ConfigurationManager.AppSettings["loopCount"]))
                    {
                        //If we get ourselves into a loop because we have a candidate on one employer's list that doesn't have a candidate record entered, let's update the summaries so 
                        //  we can at least match who we can
                        UpdateEmployerSummary(ref EmployerSummaryList, ref EmployerList);
                        
                        RemoveMatchedCandidates(ref CandidateList);
                       
                        ConstructNewEmployerList(ref EmployerSummaryList, ref EmployerList);
                        
                        return false;
                    }
                }
                //Once all the employers are matched in this go-round, update the summary with these matches
                UpdateEmployerSummary(ref EmployerSummaryList, ref EmployerList);

                //Once a candidate has been matched in a full runthrough of our Match method, they are "off the table", so remove them from the list
                RemoveMatchedCandidates(ref CandidateList);

                //Now clear the "working" employer list and construct a new one containing any employers that haven't filled all their slots
                ConstructNewEmployerList(ref EmployerSummaryList, ref EmployerList);
                //Note:  Once this newly constructed list has 0 items in it, all employers are all matched, and we'll get out of the loop              
                
                
            }
            return true;
        }

        /// <summary>
        /// Matches Candidates to Employers using the "Stable Marriage Problem" Algorithm
        /// </summary>
        /// <param name="Candidates">List of Candidates</param>
        /// <param name="Employers">List of Employers</param>
        /// <param name="EmployersMatched">List of Employer Summary Records</param>
        private static void MatchCandidates(ref List<Candidate> Candidates, ref List<Employer> Employers, ref int EmployersMatched)
        {
            //log4net.Util.LogLog.InternalDebugging = true;

            log.Info("** BEGINNING NEW DRAFT ROUND **");
            //Iterate through all employers
            foreach(Employer employer in Employers)
            {
                //Is this employer free?
                if(employer.Matched == false)
                {
                    //If not yet matched, loop through the employer's preferences in order
                    int rank = 0;
                    foreach (string p in employer.Preferences)
                    {
                        if (employer.Matched == false)
                        {
                            rank++;
                            //Get the index of the corresponding candidate from the Candidate list
                            //int i = Candidates.FindIndex(x => x.CandidateName.Contains(p));
                            int i = Candidates.FindIndex(x => x.CandidateName.Replace(" ","").ToLower().
                                Contains(p.Replace(" ","").ToLower()));

                            //See if that candidate is free (-1 check is in case their preferred candidate is no longer in the available list)
                            if (i != -1)
                            {
                                if (Candidates[i].Matched == false)
                                {                                  
                                    //If the candidate is free, change them to matched and update both lists
                                    UpdateCandidateRecord(ref Candidates, i, employer, rank);
                                    
                                    employer.Matched = true;
                                    employer.MatchedToCandidateName = Candidates[i].CandidateName;
                                    EmployersMatched++;
                                    log.Info($"Candidate {employer.MatchedToCandidateName} matched the Employer {employer.EmployerName} " +
                                        $"as both were unmatched.");
                                }
                                else
                                {
                                    //If the candidate is already matched, let's see if they want to stay that way
                                    //Compare the rank of their existing match -vs- the rank of this match
                                    //If this match is a lower number (i.e. higher in the ranked list), it gets to replace the existing match
                                    //In other words, someone's 2nd choice will replace their 4th choice
                                    int newRank = DetermineEmployerRank(Candidates[i], employer.EmployerName);
                                    if (newRank < Candidates[i].MatchedEmployerRank)
                                    {
                                        // ****Remove the old employer match****
                                        //Save the existing matched employer name
                                        string formerEmployerMatchName = Candidates[i].MatchedToEmployerName;
                                        
                                        log.Info($"Candidate {Candidates[i].CandidateName} broke up with Employer {formerEmployerMatchName}, " +
                                            $"who was number {Candidates[i].MatchedEmployerRank.ToString()} on the Candidates preference " +
                                            $"list and chose New Employer {employer.EmployerName}, who is number {newRank.ToString()} on " +
                                            $"the Candidates preference list.");

                                        //Find that employer name in the Employer list and grab the index, then update that record                                       
                                        int j = Employers.FindIndex(x => x.EmployerName.Replace(" ","").ToLower().
                                            Contains(formerEmployerMatchName.Replace(" ","").ToLower()));

                                        Employers[j].Matched = false;
                                        Employers[j].MatchedToCandidateName = "";
                                        Employers[j].MatchedCandidateRank = 0;
                                        EmployersMatched--;

                                        // **** Update the new match ****
                                        UpdateCandidateRecord(ref Candidates, i, employer, rank, newRank);
                                     
                                        employer.Matched = true;
                                        employer.MatchedToCandidateName = Candidates[i].CandidateName;
                                        EmployersMatched++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update Candidate Record with matched information
        /// </summary>
        /// <param name="CandidateList">List of Candidates</param>
        /// <param name="LocationInCandidateList">Location of Current Candidate in the CandidateList</param>
        /// <param name="Employer">Matched Employer Info.</param>
        /// <param name="MatchedCandidateRankFromEmployer">Where the matched Candidate ranks on the Employers Pref list</param>
        private static void UpdateCandidateRecord(ref List<Candidate> CandidateList,
            int LocationInCandidateList, Employer Employer, int MatchedCandidateRankFromEmployer)
        {         
            CandidateList[LocationInCandidateList].Matched = true;
            CandidateList[LocationInCandidateList].MatchedToEmployerName = Employer.EmployerName;
            //Determine where this employer falls in the candidate's preference list and save that rank
            CandidateList[LocationInCandidateList].MatchedEmployerRank = DetermineEmployerRank(CandidateList[LocationInCandidateList], Employer.EmployerName);
            CandidateList[LocationInCandidateList].MatchedCandidateRankFromEmployer = MatchedCandidateRankFromEmployer;          
        }

        /// <summary>
        /// Update Candidate Record with matched information - Overload for passing in a new rank
        /// </summary>
        /// <param name="CandidateList">List of Candidates</param>
        /// <param name="LocationInCandidateList">Location of Current Candidate in the CandidateList</param>
        /// <param name="Employer">Matched Employer Info.</param>
        /// <param name="MatchedCandidateRankFromEmployer">Where the matched Candidate ranks on the Employers Pref list</param>
        /// <param name="NewRank">The Rank of the current employer in the Candidates Pref List (when a candiate switches employers)</param>
        private static void UpdateCandidateRecord(ref List<Candidate> CandidateList,
            int LocationInCandidateList, Employer Employer, int MatchedCandidateRankFromEmployer, int NewRank)
        {         
            //Determine where this employer falls in the candidate's preference list and save that rank
            CandidateList[LocationInCandidateList].MatchedCandidateRankFromEmployer = MatchedCandidateRankFromEmployer;
            CandidateList[LocationInCandidateList].Matched = true;
            CandidateList[LocationInCandidateList].MatchedToEmployerName = Employer.EmployerName;
            CandidateList[LocationInCandidateList].MatchedEmployerRank = NewRank;
            CandidateList[LocationInCandidateList].MatchedCandidateRankFromEmployer = MatchedCandidateRankFromEmployer;
        }

        /// <summary>
        /// Determines where a particular Employer ranks in the Candidate's Preference List
        /// </summary>
        /// <param name="CurrentCandidate">Current Candidate's Candidate Record</param>
        /// <param name="EmployerName">Employer Name to find the rank of</param>
        /// <returns></returns>
        private static int DetermineEmployerRank(Candidate CurrentCandidate, string EmployerName)
        {
            int employerRank = 0;
            foreach(string prefEmployerName in CurrentCandidate.Preferences)
            {
                employerRank++;
                if(prefEmployerName.Replace(" ","").ToLower() == EmployerName.Replace(" ", "").ToLower())
                {
                    break;
                }
            }
            return employerRank;
        }

        /// <summary>
        /// Updates the Employer Summary for each employer with any matches made in the Employer list
        /// </summary>
        /// <param name="EmployerSummaries">Employer Summary List</param>
        /// <param name="Employers">Employer List</param>
        private static void UpdateEmployerSummary(ref List<EmployerSummary> EmployerSummaries, ref List<Employer> Employers)
        {
            foreach(Employer employer in Employers)
            {
                //Did the employer get a match?
                if(employer.Matched == true)
                {
                    //Find the Employer Summary record for this employer
                    //int e = EmployerSummaries.FindIndex(x => x.EmployerName.Contains(employer.EmployerName));
                    int e = EmployerSummaries.FindIndex(x => x.EmployerName.Replace(" ","").ToLower()
                        .Contains(employer.EmployerName.Replace(" ","").ToLower()));
                    if (EmployerSummaries[e].MatchedCandidates == null)
                    {
                        EmployerSummaries[e].MatchedCandidates = new List<string>();
                    }
                    //Add the matched candidate to the list of matched candidates in this employer's Summary
                    EmployerSummaries[e].MatchedCandidates.Add(employer.MatchedToCandidateName);
                    //Update the number of slots filled in this employer's Summary
                    EmployerSummaries[e].NumberOfSlotsFilled++;
                }
            }
        }
        /// <summary>
        /// Creates a new Employer list based on any Employer Summaries that still have available slots
        /// </summary>
        /// <param name="EmployerSummaries">Employer Summary List</param>
        /// <param name="Employers">Employer List</param>
        private static void ConstructNewEmployerList(ref List<EmployerSummary> EmployerSummaries, ref List<Employer> Employers)
        {
            Employers.Clear();
            foreach(EmployerSummary employerSummary in EmployerSummaries)
            {
                //Has this employer filled all their available slots?
                if(employerSummary.NumberOfSlotsFilled < employerSummary.NumberOfSlotsAvailable)
                {
                    //If all available slots haven't yet been filled, create a record for that employer in the employer list
                    Employer.AddEmployer(ref Employers, employerSummary.EmployerName, employerSummary.EmployerPreferences);
                }
            }
        }
        /// <summary>
        /// Removes candidates who have already been matched from the Candidate List
        /// </summary>
        /// <param name="Candidates">Candidates List</param>
        private static void RemoveMatchedCandidates(ref List<Candidate> Candidates)
        {
            //If the Candidate has been matched after a full runthrough of the matching method, remove them from the candidates list
            //Note:  We have to go through the candidates list in REVERSE order here, because we are removing items from the list,
            //  which will mess up your location in the list if you go forward in a ++ loop, AND it will throw an "enumeration changed"
            //  error if you try to just use a foreach to iteration over the list - so we go in reverse and remove things from the END
            //  first, so the earlier positions in the list aren't impacted
            for (int i = (Candidates.Count-1); i>=0; i--)
            {
                if(Candidates[i].Matched == true)
                {
                    Candidates.RemoveAt(i);
                }

            }
        }      
    }
}
