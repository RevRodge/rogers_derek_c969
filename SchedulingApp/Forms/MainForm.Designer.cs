namespace SchedulingApp.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.Button btnAddCustomer;
        private System.Windows.Forms.Button btnEditCustomer;
        private System.Windows.Forms.Button btnDeleteCustomer;
        private System.Windows.Forms.Button btnAddAppt;
        private System.Windows.Forms.Button btnEditAppt;
        private System.Windows.Forms.Button btnDeleteAppt;
        private System.Windows.Forms.Button btnCalendar;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.btnAddCustomer = new System.Windows.Forms.Button();
            this.btnEditCustomer = new System.Windows.Forms.Button();
            this.btnDeleteCustomer = new System.Windows.Forms.Button();
            this.btnAddAppt = new System.Windows.Forms.Button();
            this.btnEditAppt = new System.Windows.Forms.Button();
            this.btnDeleteAppt = new System.Windows.Forms.Button();
            this.btnCalendar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.Location = new System.Drawing.Point(20, 20);
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomers.Size = new System.Drawing.Size(500, 200);
            this.dgvCustomers.TabIndex = 7;
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.Location = new System.Drawing.Point(20, 270);
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.ReadOnly = true;
            this.dgvAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAppointments.Size = new System.Drawing.Size(760, 200);
            this.dgvAppointments.TabIndex = 6;
            // 
            // btnAddCustomer
            // 
            this.btnAddCustomer.Location = new System.Drawing.Point(540, 20);
            this.btnAddCustomer.Name = "btnAddCustomer";
            this.btnAddCustomer.Size = new System.Drawing.Size(100, 30);
            this.btnAddCustomer.TabIndex = 5;
            this.btnAddCustomer.Text = "Add";
            // 
            // btnEditCustomer
            // 
            this.btnEditCustomer.Location = new System.Drawing.Point(540, 60);
            this.btnEditCustomer.Name = "btnEditCustomer";
            this.btnEditCustomer.Size = new System.Drawing.Size(100, 30);
            this.btnEditCustomer.TabIndex = 4;
            this.btnEditCustomer.Text = "Modify";
            // 
            // btnDeleteCustomer
            // 
            this.btnDeleteCustomer.Location = new System.Drawing.Point(540, 100);
            this.btnDeleteCustomer.Name = "btnDeleteCustomer";
            this.btnDeleteCustomer.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteCustomer.TabIndex = 3;
            this.btnDeleteCustomer.Text = "Delete";
            // 
            // btnAddAppt
            // 
            this.btnAddAppt.Location = new System.Drawing.Point(20, 230);
            this.btnAddAppt.Name = "btnAddAppt";
            this.btnAddAppt.Size = new System.Drawing.Size(100, 30);
            this.btnAddAppt.TabIndex = 2;
            this.btnAddAppt.Text = "Add Appointment";
            // 
            // btnEditAppt
            // 
            this.btnEditAppt.Location = new System.Drawing.Point(140, 230);
            this.btnEditAppt.Name = "btnEditAppt";
            this.btnEditAppt.Size = new System.Drawing.Size(120, 30);
            this.btnEditAppt.TabIndex = 1;
            this.btnEditAppt.Text = "Modify Appointment";
            // 
            // btnDeleteAppt
            // 
            this.btnDeleteAppt.Location = new System.Drawing.Point(280, 230);
            this.btnDeleteAppt.Name = "btnDeleteAppt";
            this.btnDeleteAppt.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteAppt.TabIndex = 0;
            this.btnDeleteAppt.Text = "Delete Appointment";
            // 
            // btnCalendar
            // 
            this.btnCalendar.Location = new System.Drawing.Point(431, 230);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(89, 30);
            this.btnCalendar.TabIndex = 8;
            this.btnCalendar.Text = "Calendar";
            this.btnCalendar.UseVisualStyleBackColor = true;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btnCalendar);
            this.Controls.Add(this.btnDeleteAppt);
            this.Controls.Add(this.btnEditAppt);
            this.Controls.Add(this.btnAddAppt);
            this.Controls.Add(this.btnDeleteCustomer);
            this.Controls.Add(this.btnEditCustomer);
            this.Controls.Add(this.btnAddCustomer);
            this.Controls.Add(this.dgvAppointments);
            this.Controls.Add(this.dgvCustomers);
            this.Name = "MainForm";
            this.Text = "Scheduling Application";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.ResumeLayout(false);

        }

        
    }
}