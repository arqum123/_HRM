using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.ContactType;

namespace HRM.Core.IService
{
		
	public interface IContactTypeService
	{
        Dictionary<string, string> GetContactTypeBasicSearchColumns();
        
        List<SearchColumn> GetContactTypeAdvanceSearchColumns();

		ContactType GetContactType(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		ContactType UpdateContactType(ContactType entity);
		ContactType UpdateContactTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteContactType(System.Int32 Id);
		List<ContactType> GetAllContactType();
		ContactType InsertContactType(ContactType entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
