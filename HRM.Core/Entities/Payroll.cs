using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.Generic;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Payroll : PayrollBase 
	{
        public PayrollCycle PayrollCycle { get; set; }
        public User User { get; set; }
        public List<PayrollDetail> PayrollDetailList { get; set; }
        public int QuarterDayCount { get; set; }
        public decimal? QuarterDayAmount { get; set; }
        public int HalfDayCount { get; set; }
        public decimal? HalfDayAmount { get; set; }
        public int FullDayCount { get; set; }
        public decimal? FullDayAmount { get; set; }
        public int AbsentDayCount { get; set; }
        public decimal? AbsentDayAmount { get; set; }
        public int OverTimeDayCount { get; set; }
        public decimal? OverTimeDayAmount { get; set; }
        //New
        public int EarlyDayCount { get; set; }
        public decimal? EarlyDayAmount { get; set; }
        public int LateDayCount { get; set; }
        public decimal? LateDayAmount { get; set; }


    }
}
