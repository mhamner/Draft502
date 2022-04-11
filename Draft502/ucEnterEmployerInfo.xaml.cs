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
using Draft502.Data;

namespace Draft502
{
    /// <summary>
    /// Interaction logic for ucEnterEmployerInfo.xaml
    /// </summary>
    public partial class ucEnterEmployerInfo : UserControl
    {      
        private List<Candidate> _candidateList;
        private List<Employer> _employerList;
        private List<EmployerSummary> _employerSummaryList;

        public ucEnterEmployerInfo(ref List<Candidate> candidateList, ref List<Employer> employerList,
            ref List<EmployerSummary> employerSummaryList)
        {
            InitializeComponent();
            _candidateList = candidateList;
            _employerList = employerList;
            _employerSummaryList = employerSummaryList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployerSummary.AddEmployerSummary(ref _employerSummaryList, txtEmployerName.Text, Convert.ToInt32(txtNumberPositions.Text),
                txtEmployerPreferences.Text);
            
            lblStatusMessage.Content = $"Employer {txtEmployerName.Text} with {txtNumberPositions.Text} positions available " +
                $"added successfully.";
            txtEmployerName.Text = "";
            txtNumberPositions.Text = "";
            txtEmployerPreferences.Text = "";
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ucMainMenu(ref _candidateList, ref _employerList, ref _employerSummaryList);
        }
    }
}
