using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{

    public interface IAttendanceStatusRepository : IAttendanceStatusRepositoryBase
    {

   
        List<AttendanceStatus> GetAttendanceStatusByDateRangeSharp(DateTime StartDate, DateTime EndDate); //New

    }


}
