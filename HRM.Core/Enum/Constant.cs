using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Enum
{
    public class Constant
    {
        public const string ServiceStatus = "SERVICE STATUS";
        public const string AlternateAttendance = "ALTERNATE ATTENDANCE";
    }
    public enum ContactType
    {
        Landline = 1,
        MobileNumber = 2,
        AlternateMobileNumber = 3,
        EmergencyMobileNumber = 4,
        EmailAddress = 5,
        AlternateEmailAddress = 6
    }

    public enum AttendanceType
    {
        DailyAttendance = 1,
        Lunch = 2,
        Tea = 3,
        Prayers = 4
    }

    public enum AttendanceVariable
    {
        FullDay = 1,
        HalfDay = 2,
        OverTime = 4,
        CompensatoryHours = 5,
        StandardHours = 6,
        Early=7,
        Late=8,
        QuarterDay = 9
    }
    public enum DeviceConnectionType
    {
        Network = 1,
        Serial = 2,
        USB = 3
    }

    public enum ComPort
    {
        NotSelected = 0,
        ONE = 1,
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7,
        EIGHT = 8
    }
    public enum BraudRate
    {
        NotSelected = 0,
        Rate9600 = 9600,
        Rate19200 = 19200,
        Rate38400 = 38400,
        Rate57600 = 57600,
        Rate115200 = 115200
    }

    public enum PayrollVariable
    {
        //QuarterDay = 1,
        //HalfDay = 2,
        //FullDay = 3,
        //Absent = 4,
        //IncomeTax = 5,
        //CompensatoryLeave = 6,
        //Bonus = 7,
        //OverTime = 8,
        //Early = 9,
        //Late = 10
        FullDay = 1,
        HalfDay = 3,
        QuarterDay= 4,
        Early = 5,
        Late = 6,
        Absent = 7,
        Transport = 8


    }
}
