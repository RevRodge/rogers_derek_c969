using SchedulingApp.Data;
using SchedulingApp.Utilities;
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
    public partial class ReportsForm : Form
    {
        private readonly ApptRepo _apptRepo = new ApptRepo();
        public ReportsForm()
        {
            InitializeComponent();

            dgvReport.AutoGenerateColumns = true;

            btnApptByType.Click += btnApptByType_Click;
            btnMonthlyTotals.Click += btnMonthlyTotals_Click;
            btnUserSchedule.Click += btnUserSchedule_Click;
            btnClose.Click += (s, e) => Close();
        }

        //Appointmnt by Type report
        private void btnApptByType_Click(object sender, EventArgs e)
        {
            var appts = _apptRepo.GetAll();

            var report = appts
                .GroupBy(a => a.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .OrderBy(r => r.Type)
                .ToList();

            dgvReport.DataSource = report;
        }

        // Monthly appointment total report
        private void btnMonthlyTotals_Click(object sender, EventArgs e)
        {
            var appts = _apptRepo.GetAll();

            var report = appts
                .Select(a => TimeZoneHelper.UtcToLocal(a.StartUtc))
                .GroupBy(d => new { d.Year, d.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ToList();

            dgvReport.DataSource = report;
        }

        private void btnUserSchedule_Click(object sender, EventArgs e)
        {
            var appts = _apptRepo.GetAll();

            var report = appts
                .OrderBy(a => a.UserName)
                .ThenBy(a => a.StartUtc)
                .Select(a => new
                {
                    a.UserName,
                    a.CustomerName,
                    a.Type,
                    StartLocal = TimeZoneHelper.UtcToLocal(a.StartUtc),
                    EndLocal = TimeZoneHelper.UtcToLocal(a.EndUtc)
                })
                .ToList();

            dgvReport.DataSource = report;
        }

    }
}
