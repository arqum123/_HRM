namespace HRM.WinService
{
    partial class AttendanceService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrAttendance = new System.Timers.Timer();
            // 
            // tmrAttendance
            // 
            this.tmrAttendance.Interval = 5000;
            this.tmrAttendance.Elapsed += new System.Timers.ElapsedEventHandler(this.tmrAttendance_Elapsed);
            // 
            // AttendanceService
            // 
            this.ServiceName = "Attendance Service";

        }

        #endregion

        private System.Timers.Timer tmrAttendance;
    }
}
