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
    public partial class CalendarForm : Form
    {
        private readonly ApptRepo _apptRepo = new ApptRepo();
        public CalendarForm()
        {
            InitializeComponent();

            dgvDayAppointments.AutoGenerateColumns = true;

            monthCalendar1.DateSelected += monthCalendar1_DateSelected;
            btnClose.Click += (s, e) => Close();

            //init load
            RefreshCalendarBoldedDates();
            ShowAppointmentsForDate(DateTime.Today);
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            ShowAppointmentsForDate(e.Start.Date);
        }

        private void RefreshCalendarBoldedDates()
        {
            try
            {
                var appts = _apptRepo.GetAll();

                // Bold dates that have appointments (based on local date)
                var boldDates = appts
                    .Select(a => TimeZoneHelper.UtcToLocal(a.StartUtc).Date)
                    .Distinct()
                    .ToArray();

                monthCalendar1.RemoveAllBoldedDates();
                monthCalendar1.BoldedDates = boldDates;
                monthCalendar1.UpdateBoldedDates();
            }
            catch (Exception ex)
            {
                Logger.LogError("RefreshCalendarBoldedDates failed", ex);
                // calendar still works without bolded dates
            }
        }

        private void ShowAppointmentsForDate(DateTime selectedDateLocal)
        {
            lblSelectedDate.Text = "Appointments for: " + selectedDateLocal.ToShortDateString();

            try
            {
                var appts = _apptRepo.GetAll();

                // filter by the selected local date (start time local)
                var dayAppts = appts
                    .Select(a => new
                    {
                        a.AppointmentId,
                        a.CustomerName,
                        a.UserName,
                        a.Type,
                        StartLocal = TimeZoneHelper.UtcToLocal(a.StartUtc),
                        EndLocal = TimeZoneHelper.UtcToLocal(a.EndUtc)
                    })
                    .Where(x => x.StartLocal.Date == selectedDateLocal.Date)
                    .OrderBy(x => x.StartLocal)
                    .ToList();

                dgvDayAppointments.DataSource = null;
                dgvDayAppointments.DataSource = dayAppts;
            }
            catch (Exception ex)
            {
                Logger.LogError("ShowAppointmentsForDate failed", ex);
                MessageBox.Show("Unable to load calendar appointments.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
