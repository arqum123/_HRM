using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMMonthlyUserAttDetail
    {
        public string Date { get; set; }
        public string Shift { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public string Off_Day { get; set; }
        public string Full_Day { get; set; }
        public string Time_In { get; set; }
        public string Time_Out { get; set; }
        public string Late_In { get; set; }
        public string Early_Out { get; set; }
        public string Over_Time { get; set; }
        public string Working_Hours { get; set; }
        public string BreakType { get; set; }
    }
    //public class VMMonthlyUserAttDetailHeader
    //{
    //    public string DateFrom { get; set; }
    //    public string DateTo { get; set; }
    //    public string EmpName { get; set; }
    //    public int EmpId { get; set; }
    //    public string EmpDesignation { get; set; }
    //    public string EmpCategoryName { get; set; }
    //    public string BranchName { get; set; }
    //    public string EmpDepartmentName { get; set; }
    //    public byte[] EmpPic { get; set; }
        
    //}
}
