using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.User;

namespace HRM.Core.IService
{
		
	public interface IUserService
	{
        Dictionary<string, string> GetUserBasicSearchColumns();
        
        List<SearchColumn> GetUserAdvanceSearchColumns();

		List<User> GetUserByUserTypeId(System.Int32? UserTypeId);
		List<User> GetUserByGenderId(System.Int32? GenderId);
		List<User> GetUserByReligionId(System.Int32? ReligionId);
		List<User> GetUserByCountryId(System.Int32? CountryId);
		List<User> GetUserByCityId(System.Int32? CityId);
		List<User> GetUserByStateId(System.Int32? StateId);
        List<User> GetUserBySalaryTypeId(System.Int32? SalaryTypeId);
		User GetUser(System.Int32 Id);
        User GetUserWithDepartment(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		User UpdateUser(User entity);
		User UpdateUserByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUser(System.Int32 Id);
		List<User> GetAllUser();
        List<User> GetAllUserWithDepartment();
		User InsertUser(User entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);

        User GetUser(string LoginId);
        User GetUserWithDepartment(string LoginId);
        List<User> GetAllUserWithDepartment(int BranchID);
	}
	
	
}
