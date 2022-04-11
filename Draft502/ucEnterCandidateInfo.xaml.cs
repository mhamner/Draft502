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
using Draft502;
using Draft502.Data;

namespace Draft502
{
    /// <summary>
    /// Interaction logic for ucEnterCandidateInfo.xaml
    /// </summary>
    public partial class ucEnterCandidateInfo : UserControl
    {
        private List<Candidate> _candidateList;
        private List<Employer> _employerList;
        private List<EmployerSummary> _employerSummaryList;

        public ucEnterCandidateInfo(ref List<Candidate> candidateList, ref List<Employer> employerList, 
            ref List<EmployerSummary> employerSummaryList)
        {
            InitializeComponent();
            _candidateList = candidateList;
            _employerList = employerList;
            _employerSummaryList = employerSummaryList;
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ucMainMenu(ref _candidateList, ref _employerList, ref _employerSummaryList);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Candidate.AddCandidate(ref _candidateList, txtCandidateName.Text, txtCandidatePreferences.Text);
            
            lblStatusMessage.Content = $"Candidate {txtCandidateName.Text} added successfully.";
            txtCandidateName.Text = "";
            txtCandidatePreferences.Text = "";
        }
    }
}
