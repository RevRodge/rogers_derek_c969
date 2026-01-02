namespace SchedulingApp.Forms
{
    partial class CalendarForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.DataGridView dgvDayAppointments;
        private System.Windows.Forms.Label lblSelectedDate;
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
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.dgvDayAppointments = new System.Windows.Forms.DataGridView();
            this.lblSelectedDate = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayAppointments)).BeginInit();
            this.SuspendLayout();

            // monthCalendar1
            this.monthCalendar1.Location = new System.Drawing.Point(18, 18);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;

            // lblSelectedDate
            this.lblSelectedDate.AutoSize = true;
            this.lblSelectedDate.Location = new System.Drawing.Point(18, 195);
            this.lblSelectedDate.Name = "lblSelectedDate";
            this.lblSelectedDate.Size = new System.Drawing.Size(0, 13);
            this.lblSelectedDate.TabIndex = 1;

            // dgvDayAppointments
            this.dgvDayAppointments.AllowUserToAddRows = false;
            this.dgvDayAppointments.AllowUserToDeleteRows = false;
            this.dgvDayAppointments.Location = new System.Drawing.Point(18, 220);
            this.dgvDayAppointments.MultiSelect = false;
            this.dgvDayAppointments.Name = "dgvDayAppointments";
            this.dgvDayAppointments.ReadOnly = true;
            this.dgvDayAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDayAppointments.Size = new System.Drawing.Size(740, 230);
            this.dgvDayAppointments.TabIndex = 2;

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(683, 460);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;

            // CalendarForm
            this.ClientSize = new System.Drawing.Size(780, 505);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvDayAppointments);
            this.Controls.Add(this.lblSelectedDate);
            this.Controls.Add(this.monthCalendar1);
            this.Name = "CalendarForm";
            this.Text = "Calendar";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}