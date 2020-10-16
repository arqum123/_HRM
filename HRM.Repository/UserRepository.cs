using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using System.Data.SqlClient;
using System.Data;

namespace HRM.Repository
{
    public partial class UserRepository: UserRepositoryBase, IUserRepository
	{
        public User GetUser(string LoginId, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetUserSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += "from [User] with (nolock)  where LoginID=@LoginID and IsActive = 1";
            SqlParameter parameter = new SqlParameter("@LoginID", LoginId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return UserFromDataRow(ds.Tables[0].Rows[0]);
        }
        //New
        public override List<User> GetAllUser(string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetUserSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += "from [User] with (nolock)  ";
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, null);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<User>(ds, UserFromDataRow1);
        }

        public virtual User UserFromDataRow1(DataRow dr)
        {
            if (dr == null) return null;
            User entity = new User();
            if (dr.Table.Columns.Contains("ID"))
            {
                entity.Id = (System.Int32)dr["ID"];
            }
            if (dr.Table.Columns.Contains("JoiningDate"))
            {
                entity.JoiningDate = dr["JoiningDate"] == DBNull.Value ? (System.DateTime?)null : (System.DateTime?)dr["JoiningDate"];
            }

            if (dr.Table.Columns.Contains("FirstName"))
            {
                entity.FirstName = dr["FirstName"].ToString();
            }
            if (dr.Table.Columns.Contains("MiddleName"))
            {
                entity.MiddleName = dr["MiddleName"].ToString();
            }
            if (dr.Table.Columns.Contains("LastName"))
            {
                entity.LastName = dr["LastName"].ToString();
            }
            if (dr.Table.Columns.Contains("UserTypeID"))
            {
                entity.UserTypeId = dr["UserTypeID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["UserTypeID"];
            }
            if (dr.Table.Columns.Contains("GenderID"))
            {
                entity.GenderId = dr["GenderID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["GenderID"];
            }
            if (dr.Table.Columns.Contains("DateOfBirth"))
            {
                entity.DateOfBirth = dr["DateOfBirth"] == DBNull.Value ? (System.DateTime?)null : (System.DateTime?)dr["DateOfBirth"];
            }

            if (dr.Table.Columns.Contains("NICNo"))
            {
                entity.NicNo = dr["NICNo"].ToString();
            }
            if (dr.Table.Columns.Contains("ReligionID"))
            {
                entity.ReligionId = dr["ReligionID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["ReligionID"];
            }
            if (dr.Table.Columns.Contains("Address1"))
            {
                entity.Address1 = dr["Address1"].ToString();
            }
            if (dr.Table.Columns.Contains("Address2"))
            {
                entity.Address2 = dr["Address2"].ToString();
            }
            if (dr.Table.Columns.Contains("ZipCode"))
            {
                entity.ZipCode = dr["ZipCode"].ToString();
            }
            if (dr.Table.Columns.Contains("CountryID"))
            {
                entity.CountryId = dr["CountryID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["CountryID"];
            }
            if (dr.Table.Columns.Contains("CityID"))
            {
                entity.CityId = dr["CityID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["CityID"];
            }
            if (dr.Table.Columns.Contains("StateID"))
            {
                entity.StateId = dr["StateID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["StateID"];
            }
            if (dr.Table.Columns.Contains("AcadmicQualification"))
            {
                entity.AcadmicQualification = dr["AcadmicQualification"].ToString();
            }
            if (dr.Table.Columns.Contains("Designation"))
            {
                entity.Designation = dr["Designation"].ToString();
            }
            if (dr.Table.Columns.Contains("Salary"))
            {
                entity.Salary = dr["Salary"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Salary"];
            }
            if (dr.Table.Columns.Contains("LoginID"))
            {
                entity.LoginId = dr["LoginID"].ToString();
            }
            if (dr.Table.Columns.Contains("Password"))
            {
                entity.Password = dr["Password"].ToString();
            }
            if (dr.Table.Columns.Contains("ImagePath"))
            {
                entity.ImagePath = dr["ImagePath"].ToString();
            }
            if (dr.Table.Columns.Contains("IsActive"))
            {
                entity.IsActive = dr["IsActive"] == DBNull.Value ? (System.Boolean?)null : (System.Boolean?)dr["IsActive"];
            }
            if (dr.Table.Columns.Contains("CreationDate"))
            {
                entity.CreationDate = dr["CreationDate"] == DBNull.Value ? (System.DateTime?)null : (System.DateTime?)dr["CreationDate"];
            }
            if (dr.Table.Columns.Contains("UpdateDate"))
            {
                entity.UpdateDate = dr["UpdateDate"] == DBNull.Value ? (System.DateTime?)null : (System.DateTime?)dr["UpdateDate"];
            }
            if (dr.Table.Columns.Contains("UpdateBy"))
            {
                entity.UpdateBy = dr["UpdateBy"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["UpdateBy"];
            }
            if (dr.Table.Columns.Contains("UserIP"))
            {
                entity.UserIp = dr["UserIP"].ToString();
            }
            if (dr.Table.Columns.Contains("SalaryTypeID"))
            {
                entity.SalaryTypeId = dr["SalaryTypeID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["SalaryTypeID"];
            }
            if (dr.Table.Columns.Contains("FatherName"))
            {
                entity.FatherName = dr["FatherName"].ToString();
            }
            if (dr.Table.Columns.Contains("AccountNumber"))
            {
                entity.AccountNumber = dr["AccountNumber"].ToString();
            }

          
            return entity;
        }
    }
	
	
}
