namespace SchedulingApp.Forms
{
    partial class ReportsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnApptByType;
        private System.Windows.Forms.Button btnMonthlyTotals;
        private System.Windows.Forms.Button btnUserSchedule;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Button btnClose;


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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApptByType = new System.Windows.Forms.Button();
            this.btnMonthlyTotals = new System.Windows.Forms.Button();
            this.btnUserSchedule = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // btnApptByType
            this.btnApptByType.Location = new System.Drawing.Point(20, 20);
            this.btnApptByType.Size = new System.Drawing.Size(160, 30);
            this.btnApptByType.Text = "Appointments by Type";

            // btnMonthlyTotals
            this.btnMonthlyTotals.Location = new System.Drawing.Point(200, 20);
            this.btnMonthlyTotals.Size = new System.Drawing.Size(160, 30);
            this.btnMonthlyTotals.Text = "Monthly Totals";

            // btnUserSchedule
            this.btnUserSchedule.Location = new System.Drawing.Point(380, 20);
            this.btnUserSchedule.Size = new System.Drawing.Size(160, 30);
            this.btnUserSchedule.Text = "User Schedule";

            // dgvReport
            this.dgvReport.Location = new System.Drawing.Point(20, 70);
            this.dgvReport.Size = new System.Drawing.Size(720, 320);
            this.dgvReport.ReadOnly = true;
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(665, 400);
            this.btnClose.Size = new System.Drawing.Size(75, 28);
            this.btnClose.Text = "Close";

            // ReportsForm
            this.ClientSize = new System.Drawing.Size(760, 440);
            this.Controls.Add(this.btnApptByType);
            this.Controls.Add(this.btnMonthlyTotals);
            this.Controls.Add(this.btnUserSchedule);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.btnClose);
            this.Text = "Reports";

            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}