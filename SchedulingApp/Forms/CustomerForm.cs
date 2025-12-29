using SchedulingApp.Models;
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
    public partial class CustomerForm : Form
    {
        public Customer Customer { get; private set; }
        public CustomerForm()
        {
            InitializeComponent();

            Text = "Add Customer";
            chkActive.Checked = true;

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
        }

        public CustomerForm(Customer existingCustomer)
        {
            InitializeComponent();

            Text = "Modify Customer";
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            if (existingCustomer == null)
                throw new ArgumentNullException(nameof(existingCustomer));

            // clone values into a working copy to not mutate the grid object
            Customer = new Customer
            {
                CustomerId = existingCustomer.CustomerId,
                CustomerName = existingCustomer.CustomerName,
                AddressId = existingCustomer.AddressId,
                Active = existingCustomer.Active,
                AddressLine1 = existingCustomer.AddressLine1,
                AddressLine2 = existingCustomer.AddressLine2,
                CityId = existingCustomer.CityId,
                PostalCode = existingCustomer.PostalCode,
                Phone = existingCustomer.Phone,
                CityName = existingCustomer.CityName,
                CountryName = existingCustomer.CountryName
            };

            // Fill UI fields
            txtCustomerName.Text = Customer.CustomerName;
            txtAddress1.Text = Customer.AddressLine1;
            txtAddress2.Text = Customer.AddressLine2;
            txtCityId.Text = Customer.CityId.ToString();
            txtPostalCode.Text = Customer.PostalCode;
            txtPhone.Text = Customer.Phone;
            chkActive.Checked = Customer.Active;
        }

        // ------------------ Buttons ------------------

        //commits edits to a customer
        private void btnSave_Click(object sender, EventArgs e)
        {
            // If Customer is null, switches to Add customer instead
            if (Customer == null)
                Customer = new Customer();

            Customer.CustomerName = txtCustomerName.Text.Trim();
            Customer.AddressLine1 = txtAddress1.Text.Trim();
            Customer.AddressLine2 = txtAddress2.Text.Trim();
            Customer.PostalCode = txtPostalCode.Text.Trim();
            Customer.Phone = txtPhone.Text.Trim();
            Customer.Active = chkActive.Checked;

            // CityId must parse to int
            if (!int.TryParse(txtCityId.Text.Trim(), out int cityId) || cityId <= 0)
            {
                MessageBox.Show("CityId must be a valid positive number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Customer.CityId = cityId;

            // validate with The Validator(TM)
            if (!Validator.TryValidateCustomer(Customer, out string error))
            {
                MessageBox.Show(error, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        //closes out and does not save any changes or additions
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
