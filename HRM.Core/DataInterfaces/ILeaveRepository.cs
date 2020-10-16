using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.Model;

namespace HRM.Core.DataInterfaces
{
		
	public interface ILeaveRepository: ILeaveRepositoryBase
	{
        System.Data.DataSet GetApprovedLeavesByDateRangeSP(int? UserID, int? DepartmentID, DateTime StartDate, DateTime EndDate);
        System.Data.DataSet GetPendingLeavesByDateRangeSP(int? UserID, int? DepartmentID, DateTime StartDate, DateTime EndDate);

        System.Data.DataSet GetLeaveByUserIdAndDateRangeSP(int? UserID, DateTime StartDate, DateTime EndDate);
    }
	
	
}
