using SchedulingApp;
using SchedulingApp.Data;
using SchedulingApp.Models;
using SchedulingApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingApp.Forms
{
    public partial class AppointmentForm : Form
    {
        private readonly ApptRepo _apptRepo = new ApptRepo();

        private readonly List<Customer> _customers;
        private readonly List<User> _users;

        public Appointment Appointment { get; private set; }

        public AppointmentForm(List<Customer> customers, List<User> users)
        {
            InitializeComponent();

            Text = "Add Appointment";

            _customers = customers ?? new List<Customer>();
            _users = users ?? new List<User>();

            BindDropdowns();

            btnSave.Text = Strings.BtnSave;
            btnCancel.Text = Strings.BtnCancel;
        }

        private void BindDropdowns()
        {
            cmbCustomer.DataSource = _customers;
            cmbCustomer.DisplayMember = "CustomerName";
            cmbCustomer.ValueMember = "CustomerId";

            cmbUser.DataSource = _users;
            cmbUser.DisplayMember = "UserName";
            cmbUser.ValueMember = "UserId";
        }

        // modify appt
        public AppointmentForm(
           Appointment existing,
           List<Customer> customers,
           List<User> users)
           : this(customers, users)
        {
            //TODO: Needs es lang support
            Text = "Modify Appointment";

            Appointment = new Appointment
            {
                AppointmentId = existing.AppointmentId,
                CustomerId = existing.CustomerId,
                UserId = existing.UserId,
                Type = existing.Type,
                StartUtc = existing.StartUtc,
                EndUtc = existing.EndUtc
            };

            cmbCustomer.SelectedValue = Appointment.CustomerId;
            cmbUser.SelectedValue = Appointment.UserId;
            txtType.Text = Appointment.Type;

            dtpStart.Value = TimeZoneHelper.UtcToLocal(Appointment.StartUtc);
            dtpEnd.Value = TimeZoneHelper.UtcToLocal(Appointment.EndUtc);
        }

        //------------------ Buttons -------------------

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Appointment == null)
                Appointment = new Appointment();

            Appointment.CustomerId = (int)cmbCustomer.SelectedValue;
            Appointment.UserId = (int)cmbUser.SelectedValue;
            Appointment.Type = txtType.Text.Trim();

            DateTime startLocal = dtpStart.Value;
            DateTime endLocal = dtpEnd.Value;

            // Basic validation
            if (string.IsNullOrWhiteSpace(Appointment.Type))
            {
                MessageBox.Show(Strings.ApptTypeRequired, Strings.InfoTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // check business hours (Eastern)
            if (!TimeZoneHelper.IsWithinBusinessHoursEastern(startLocal, endLocal))
            {
                MessageBox.Show(Strings.ApptBusinessHoursError, Strings.InfoTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // convert to UTC before storing
            Appointment.StartUtc = TimeZoneHelper.LocalToUtc(startLocal);
            Appointment.EndUtc = TimeZoneHelper.LocalToUtc(endLocal);

            //assigns an explicit local var to make the overlap compare
            int? apptId = Appointment.AppointmentId == 0
                            ? (int?)null
                            : Appointment.AppointmentId;

            // check for overlapping appt by comparing times            
            bool overlap = _apptRepo.HasOverlappingAppointment(
                Appointment.CustomerId,
                apptId,
                Appointment.StartUtc,
                Appointment.EndUtc);

            if (overlap)
            {
                MessageBox.Show(Strings.ApptOverlapError, Strings.InfoTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }



    }
}
