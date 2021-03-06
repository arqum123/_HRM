﻿using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Branch;

namespace HRM.Core.IService
{
		
	public interface IBranchService
	{
        Dictionary<string, string> GetBranchBasicSearchColumns();
        
        List<SearchColumn> GetBranchAdvanceSearchColumns();

		Branch GetBranch(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Branch UpdateBranch(Branch entity);
		Branch UpdateBranchByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteBranch(System.Int32 Id);
		List<Branch> GetAllBranch();
		Branch InsertBranch(Branch entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
