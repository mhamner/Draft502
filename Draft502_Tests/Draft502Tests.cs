using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Draft502;
using Draft502.Data;

namespace Draft502_Tests
{
    [TestClass]
    public class Draft502Tests
    {
        private List<Candidate> _candidateList;
        private List<Employer> _employerList;
        private List<EmployerSummary> _employerSummaryList;

        [TestMethod]
        public void RunDraft_2Candidates_2Employers_MatchesCandidates()
        {
            _employerList = new List<Employer>();
            Candidate.AddCandidate(ref _candidateList, "Tony Stark", "Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "Steve Rogers", "Humana;Waystar");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Waystar", 1, "Tony Stark;Steve Rogers");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Humana", 1, "Steve Rogers;Tony Stark");

            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
               $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Draft502_TestResults\\");
        }

        [TestMethod]
        public void RunDraft_4Candidates_4Employers_MatchesCandidates()
        {
            _employerList = new List<Employer>();
            Candidate.AddCandidate(ref _candidateList, "Tony Stark", "Waystar;Humana;RSCS;Atria");
            Candidate.AddCandidate(ref _candidateList, "Steve Rogers", "Humana;Atria;RSCS;Waystar");
            Candidate.AddCandidate(ref _candidateList, "Wanda Maximoff", "Atria;RSCS;Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "The Vision", "RSCS;Waystar;Humana;Atria");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Waystar", 1, "Tony Stark;Steve Rogers;Wanda Maximoff;The Vision");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Humana", 1, "Steve Rogers;Tony Stark;The Vision; Wanda Maximoff");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "RSCS", 1, "The Vision;Steve Rogers;Wanda Maximoff;Tony Stark");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Atria", 1, "Wanda Maximoff;Tony Stark;The Vision;Steve Rogers");

            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
               $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Draft502_TestResults\\");
        }

        [TestMethod]
        public void RunDraft_MoreSlotsThanCandidates_ShowsMessage()
        {
            _employerList = new List<Employer>();
            Candidate.AddCandidate(ref _candidateList, "Tony Stark", "Waystar;Humana;RSCS;Atria");
            Candidate.AddCandidate(ref _candidateList, "Steve Rogers", "Humana;Atria;RSCS;Waystar");
            Candidate.AddCandidate(ref _candidateList, "Wanda Maximoff", "Atria;RSCS;Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "The Vision", "RSCS;Waystar;Humana;Atria");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Waystar", 4, "Tony Stark;Steve Rogers;Wanda Maximoff;The Vision");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Humana", 1, "Steve Rogers;Tony Stark;The Vision; Wanda Maximoff");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "RSCS", 2, "The Vision;Steve Rogers;Wanda Maximoff;Tony Stark");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Atria", 1, "Wanda Maximoff;Tony Stark;The Vision;Steve Rogers");

            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
               $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Draft502_TestResults\\");
        }

        [TestMethod]
        public void RunDraft_MoreCandidatesThanSlots_ShowsMessage()
        {
            _employerList = new List<Employer>();
            Candidate.AddCandidate(ref _candidateList, "Tony Stark", "Waystar;Humana;RSCS;Atria");
            Candidate.AddCandidate(ref _candidateList, "Steve Rogers", "Humana;Atria;RSCS;Waystar");
            Candidate.AddCandidate(ref _candidateList, "Wanda Maximoff", "Atria;RSCS;Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "The Vision", "RSCS;Waystar;Humana;Atria");          
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "RSCS", 1, "The Vision;Steve Rogers;Wanda Maximoff;Tony Stark");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Atria", 1, "Wanda Maximoff;Tony Stark;The Vision;Steve Rogers");

            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
               $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Draft502_TestResults\\");
        }

        [TestMethod]
        public void RunDraft_EmployerHasSlotForCandidateWhoDidntPickThem()
        {
            _employerList = new List<Employer>();
            Candidate.AddCandidate(ref _candidateList, "Tony Stark", "Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "Steve Rogers", "Humana;Atria;RSCS;Waystar");
            Candidate.AddCandidate(ref _candidateList, "Wanda Maximoff", "Atria;RSCS;Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "The Vision", "RSCS;Waystar;Humana;Atria");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "RSCS", 3, "The Vision;Steve Rogers;Wanda Maximoff;Tony Stark");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Atria", 1, "Wanda Maximoff;Tony Stark;The Vision;Steve Rogers");

            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
               $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Draft502_TestResults\\");
        }

        [TestMethod]
        public void RunDraft_CandidatePickedEmployerButEmployerDidntPickThem()
        {
            _employerList = new List<Employer>();
            Candidate.AddCandidate(ref _candidateList, "Tony Stark", "Waystar;Humana;RSCS");
            Candidate.AddCandidate(ref _candidateList, "Steve Rogers", "Humana;Atria;RSCS;Waystar");
            Candidate.AddCandidate(ref _candidateList, "Wanda Maximoff", "Atria;RSCS;Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "The Vision", "RSCS;Waystar;Humana;Atria");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "RSCS", 3, "The Vision;Steve Rogers;Wanda Maximoff");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Atria", 1, "Wanda Maximoff;Tony Stark;The Vision;Steve Rogers");

            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
               $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Draft502_TestResults\\");
        }

        [TestMethod]
        public void RunDraft_EndlessLoop()
        {
            //Misspelling one of the Candidates to force an endless loop - should be trapped and see a loop report
            _employerList = new List<Employer>();
            Candidate.AddCandidate(ref _candidateList, "Tony Stark", "Waystar;Humana;RSCS;Atria");
            Candidate.AddCandidate(ref _candidateList, "Steve Rogers", "Humana;Atria;RSCS;Waystar");
            Candidate.AddCandidate(ref _candidateList, "Wanda Maximoff", "Atria;RSCS;Waystar;Humana");
            Candidate.AddCandidate(ref _candidateList, "The Vision", "RSCS;Waystar;Humana;Atria");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Waystar", 1, "Tony Stark;Steve Rogers;Wanda Maximoff;The Vision");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Humana", 1, "Steve Rogers;Tony Stark;The Vision; Wanda Maximoff");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "RSCS", 1, "The Vision;Steve Rogers;Wanda Maximoff;Tony Stark");
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, "Atria", 1, "Wanda MaximON;Tony Stark;The Vision;Steve Rogers");

            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
               $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Draft502_TestResults\\");
        }
    }
}
