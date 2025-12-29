using SchedulingApp.Models;
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
        public MainForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }
    }
}
