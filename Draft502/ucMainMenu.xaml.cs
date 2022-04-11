using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Diagnostics;
using Draft502.Data;

namespace Draft502
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class ucMainMenu : System.Windows.Controls.UserControl
    {
        private List<Candidate> _candidateList;
        private List<EmployerSummary> _employerSummaryList;
        private List<Employer> _employerList;
        public ucMainMenu(ref List<Candidate> candidateList, ref List<Employer> employerList, ref List<EmployerSummary> employerSummaryList)
        {
            InitializeComponent();
            _candidateList = candidateList;
            _employerList = employerList;
            _employerSummaryList = employerSummaryList;
        }

        public ucMainMenu(ref List<Candidate> candidateList)
        {
            InitializeComponent();
            _candidateList = candidateList;       
        }

        public ucMainMenu(ref List<EmployerSummary> employerSummaryList)
        {
            InitializeComponent();
            _employerSummaryList = employerSummaryList;
        }

        private void btnAddCandidateInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ucEnterCandidateInfo(ref _candidateList, ref _employerList, ref _employerSummaryList);
        }

        private void btnMatchCandidates_Click(object sender, RoutedEventArgs e)
        {
            /*
            if(CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList))
            {
                ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
                    txtFileLocation.Text);
            }
            else
            {
                ReportCreator.CreateLoopReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
                    txtFileLocation.Text);
            }
            */
            CandidateMatching.RunTheDraft(ref _candidateList, ref _employerList, ref _employerSummaryList);
            ReportCreator.CreateResultsReport(ref _candidateList, ref _employerList, ref _employerSummaryList,
                    txtFileLocation.Text);
        }

        private void btnAddEmployerInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ucEnterEmployerInfo(ref _candidateList, ref _employerList, ref _employerSummaryList);
        }

        private void btnChooseResultsLocation_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            var result = folderDialog.ShowDialog();
            switch(result)
            {
                case DialogResult.OK:
                    txtFileLocation.Text = folderDialog.SelectedPath;
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;  
            }
        }
    }
}
