using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IUserRepository: IUserRepositoryBase
	{
        User GetUser(string LoginId, string SelectClause = null);
	}
	
	
}
