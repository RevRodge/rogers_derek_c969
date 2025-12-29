using SchedulingApp.Models;
using SchedulingApp.Utilities;
using SchedulingApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly User _currentUser;

        private readonly CustomerRepo _customerRepo = new CustomerRepo();
        private readonly ApptRepo _apptRepo = new ApptRepo();

        private List<Customer> _customers = new List<Customer>();
        private List<Appointment> _appointments = new List<Appointment>();

        public MainForm(User user)
        {
            InitializeComponent();

            _currentUser = user ?? throw new ArgumentNullException(nameof(user));

            //TODO: Draw the rest of the owl
        }
    }
}
