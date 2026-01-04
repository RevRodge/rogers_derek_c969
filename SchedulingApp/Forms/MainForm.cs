using SchedulingApp;
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
        private readonly UserRepo _userRepo = new UserRepo();
        private readonly ApptRepo _apptRepo = new ApptRepo();

        private List<Customer> _customers = new List<Customer>();
        private List<User> _users = new List<User>();
        private List<Appointment> _appointments = new List<Appointment>();
        
        public MainForm(User user)
        {
            InitializeComponent();

            _currentUser = user ?? throw new ArgumentNullException(nameof(user));

            //calls a grid setup to avoid ui weirdness
            ConfigureCustomerGrid();
            ConfigureAppointmentGrid();

            //wiring buttons
            btnAddCustomer.Click += btnAddCustomer_Click;
            btnEditCustomer.Click += btnEditCustomer_Click;
            btnDeleteCustomer.Click += btnDeleteCustomer_Click;
            btnAddAppt.Click += btnAddAppt_Click;
            btnEditAppt.Click += btnEditAppt_Click;
            btnDeleteAppt.Click += btnDeleteAppt_Click;
            btnCalendar.Click += btnCalendar_Click;

            //load init data
            LoadCustomers();
            LoadUsers();
            LoadAppointments();

            //update some buttons when selections are made
            dgvCustomers.SelectionChanged += (s, e) => UpdateCustomerButtons();
            dgvAppointments.SelectionChanged += (s, e) => UpdateAppointmentButtons();

            UpdateCustomerButtons();
            UpdateAppointmentButtons();
        }
        // ----------- data grid view setup ------------
        private void ConfigureCustomerGrid()
        {
            dgvCustomers.AutoGenerateColumns = true;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
        }
        private void ConfigureAppointmentGrid()
        {
            dgvAppointments.AutoGenerateColumns = true;
            dgvAppointments.ReadOnly = true;
            dgvAppointments.MultiSelect = false;
            dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAppointments.AllowUserToAddRows = false;
            dgvAppointments.AllowUserToDeleteRows = false;
        }
        // ------------- Populate ---------------
        private void LoadCustomers()
        {
            try
            {
                _customers = _customerRepo.GetAll();

                // binds a copy so sorting/filtering won't break selection
                dgvCustomers.DataSource = null;
                dgvCustomers.DataSource = _customers;

                //hides internal ids for views
                HideColumnIfExists(dgvCustomers, "AddressId");
                HideColumnIfExists(dgvCustomers, "CityId");
            }
            catch (Exception ex)
            {
                Logger.LogError("LoadCustomers failed", ex);
                MessageBox.Show(Strings.MainLoadCustomersError, Strings.ErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                _users = _userRepo.GetAll();
            }
            catch (Exception ex)
            {
                Logger.LogError("LoadUsers failed", ex);
                MessageBox.Show(Strings.MainLoadUsersError, Strings.ErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                _users = new List<User>();
            }
        }

        private void LoadAppointments()
        {
            try
            {
                _appointments = _apptRepo.GetAll();

                // Creates display projection to convert UTC to local for display,
                // without changing stored model values
                var display = _appointments.Select(a => new
                {
                    a.AppointmentId,
                    a.CustomerId,
                    a.UserId,
                    a.Type,
                    StartLocal = TimeZoneHelper.UtcToLocal(a.StartUtc),
                    EndLocal = TimeZoneHelper.UtcToLocal(a.EndUtc),
                    a.CustomerName,
                    a.UserName
                }).ToList();

                //binds copy as in LoadCustomers() for sorting
                dgvAppointments.DataSource = null;
                dgvAppointments.DataSource = display;

                //hides internal ids for views
                HideColumnIfExists(dgvAppointments, "CustomerId");
                HideColumnIfExists(dgvAppointments, "UserId");
            }
            catch (Exception ex)
            {
                Logger.LogError("LoadAppointments failed", ex);
                MessageBox.Show(Strings.MainLoadAppointmentsError, Strings.ErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------- Data Requests-----------------
        private Customer GetSelectedCustomer()
        {
            if (dgvCustomers.CurrentRow == null)
                return null;

            // DataSource is List<Customer>, so CurrentRow.DataBoundItem is a Customer
            return dgvCustomers.CurrentRow.DataBoundItem as Customer;
        }

        private int? GetSelectedAppointmentId()
        {
            if (dgvAppointments.CurrentRow == null)
                return null;

            // bound appointments to an anonymous display object,
            // so here I need to read the AppointmentId cell.
            object value = dgvAppointments.CurrentRow.Cells["AppointmentId"].Value;
            if (value == null)
                return null;

            return Convert.ToInt32(value);
        }

        //hides internal ID columns to tidy up UI
        private void HideColumnIfExists(DataGridView grid, string columnName)
        {
            if (grid.Columns.Contains(columnName))
                grid.Columns[columnName].Visible = false;
        }

        // -------------- Buttons --------------------


        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            using (var form = new CustomerForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    _customerRepo.Add(form.Customer);
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Add customer failed", ex);
                    MessageBox.Show(Strings.CustomerAddError, Strings.ErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
                return;

            using (var form = new CustomerForm(selected))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    _customerRepo.Update(form.Customer);
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Update customer failed", ex);
                    MessageBox.Show(Strings.CustomerUpdateError, Strings.ErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
                return;

            var confirm = MessageBox.Show(
                string.Format(Strings.CustomerDeleteConfirm, selected.CustomerName),
                Strings.ConfirmTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                _customerRepo.Delete(selected.CustomerId);
                LoadCustomers();
            }
            catch (Exception ex)
            {
                Logger.LogError("Delete customer failed", ex);

                // Common fail case: FK constraint because customer has appointments
                MessageBox.Show(Strings.CustomerDeleteErrorHasAppts, Strings.ErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddAppt_Click(object sender, EventArgs e)
        {
            using (var form = new AppointmentForm(_customers, _users))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    _apptRepo.Add(form.Appointment);
                    LoadAppointments();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Add appointment failed", ex);
                    MessageBox.Show(Strings.ApptAddError, Strings.ErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnEditAppt_Click(object sender, EventArgs e)
        {
            int? apptId = GetSelectedAppointmentId();
            if (!apptId.HasValue)
                return;

            var appt = _appointments.First(a => a.AppointmentId == apptId.Value);

            using (var form = new AppointmentForm(appt, _customers, _users))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    _apptRepo.Update(form.Appointment);
                    LoadAppointments();
                }
                catch (Exception ex)
                {
                    Logger.LogError("Update appointment failed", ex);
                    MessageBox.Show(Strings.ApptUpdateError, Strings.ErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteAppt_Click(object sender, EventArgs e)
        {
            int? apptId = GetSelectedAppointmentId();
            if (!apptId.HasValue)
                return;

            var confirm = MessageBox.Show(
                Strings.ApptDeleteConfirm,
                Strings.ConfirmTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                _apptRepo.Delete(apptId.Value);
                LoadAppointments();
            }
            catch (Exception ex)
            {
                Logger.LogError("Delete appointment failed", ex);
                MessageBox.Show(Strings.ApptDeleteError, Strings.ErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //two functions for updating the buttons when selected
        private void UpdateCustomerButtons()
        {
            bool hasSelection = dgvCustomers.CurrentRow != null && dgvCustomers.CurrentRow.Index >= 0;
            btnEditCustomer.Enabled = hasSelection;
            btnDeleteCustomer.Enabled = hasSelection;
        }
        private void UpdateAppointmentButtons()
        {
            bool hasSelection = dgvAppointments.CurrentRow != null && dgvAppointments.CurrentRow.Index >= 0;
            btnEditAppt.Enabled = hasSelection;
            btnDeleteAppt.Enabled = hasSelection;
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            using (var form = new CalendarForm())
            {
                form.ShowDialog();
            }
        }
    }
}
