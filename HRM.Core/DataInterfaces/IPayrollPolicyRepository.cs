using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.Model;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollPolicyRepository: IPayrollPolicyRepositoryBase
	{
        System.Data.DataSet GetPayrollPolicyInformationSP(System.Int32? UserId, System.Boolean? IsEarly, System.Boolean? IsLate,System.String StartDate, System.String EndDate);
        
    }
	
	
}
