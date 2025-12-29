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

            //update some buttons when selections are made
            dgvCustomers.SelectionChanged += (s, e) => UpdateCustomerButtons();
            dgvAppointments.SelectionChanged += (s, e) => UpdateAppointmentButtons();

            //load init data
            LoadCustomers();
            LoadAppointments();

            UpdateCustomerButtons();
            UpdateAppointmentButtons();
        }
        // data grid view setup functions
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
        // ------------- Populate Form ---------------
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
                MessageBox.Show("Unable to load customers.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Unable to load appointments.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------- Pulling Data -----------------
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
        
        //TODO: Wire buttons

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Customer Add form not wired yet.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
                return;

            MessageBox.Show("Customer Modify form not wired yet.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
                return;

            var confirm = MessageBox.Show(
                $"Delete customer '{selected.CustomerName}'?",
                "Confirm",
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

                // Common case: FK constraint because customer has appointments
                MessageBox.Show("Unable to delete customer. They may have appointments.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddAppt_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" FIX MEEEE Appointment Add form not wired yet.", "AHHH",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditAppt_Click(object sender, EventArgs e)
        {
            int? apptId = GetSelectedAppointmentId();
            if (!apptId.HasValue)
                return;

            MessageBox.Show("FIX MEEE Appointment Modify form not wired yet.", "OH NO",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteAppt_Click(object sender, EventArgs e)
        {
            int? apptId = GetSelectedAppointmentId();
            if (!apptId.HasValue)
                return;

            var confirm = MessageBox.Show(
                "Delete selected appointment?",
                "Confirm",
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
                MessageBox.Show("Unable to delete appointment.", "Error",
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

    }
}
