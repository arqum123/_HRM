using System;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.Extensions;

namespace HRM.Repository
{
		
	public abstract partial class UserRepositoryBase : Repository, IUserRepositoryBase 
	{
        
        public UserRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("FirstName",new SearchColumn(){Name="FirstName",Title="FirstName",SelectClause="FirstName",WhereClause="AllRecords.FirstName",DataType="System.String",IsForeignColumn=false,PropertyName="FirstName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("MiddleName",new SearchColumn(){Name="MiddleName",Title="MiddleName",SelectClause="MiddleName",WhereClause="AllRecords.MiddleName",DataType="System.String",IsForeignColumn=false,PropertyName="MiddleName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("LastName",new SearchColumn(){Name="LastName",Title="LastName",SelectClause="LastName",WhereClause="AllRecords.LastName",DataType="System.String",IsForeignColumn=false,PropertyName="LastName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserTypeID",new SearchColumn(){Name="UserTypeID",Title="UserTypeID",SelectClause="UserTypeID",WhereClause="AllRecords.UserTypeID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UserTypeId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("GenderID",new SearchColumn(){Name="GenderID",Title="GenderID",SelectClause="GenderID",WhereClause="AllRecords.GenderID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="GenderId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DateOfBirth",new SearchColumn(){Name="DateOfBirth",Title="DateOfBirth",SelectClause="DateOfBirth",WhereClause="AllRecords.DateOfBirth",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="DateOfBirth",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("NICNo",new SearchColumn(){Name="NICNo",Title="NICNo",SelectClause="NICNo",WhereClause="AllRecords.NICNo",DataType="System.String",IsForeignColumn=false,PropertyName="NicNo",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ReligionID",new SearchColumn(){Name="ReligionID",Title="ReligionID",SelectClause="ReligionID",WhereClause="AllRecords.ReligionID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="ReligionId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Address1",new SearchColumn(){Name="Address1",Title="Address1",SelectClause="Address1",WhereClause="AllRecords.Address1",DataType="System.String",IsForeignColumn=false,PropertyName="Address1",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Address2",new SearchColumn(){Name="Address2",Title="Address2",SelectClause="Address2",WhereClause="AllRecords.Address2",DataType="System.String",IsForeignColumn=false,PropertyName="Address2",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ZipCode",new SearchColumn(){Name="ZipCode",Title="ZipCode",SelectClause="ZipCode",WhereClause="AllRecords.ZipCode",DataType="System.String",IsForeignColumn=false,PropertyName="ZipCode",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CountryID",new SearchColumn(){Name="CountryID",Title="CountryID",SelectClause="CountryID",WhereClause="AllRecords.CountryID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="CountryId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CityID",new SearchColumn(){Name="CityID",Title="CityID",SelectClause="CityID",WhereClause="AllRecords.CityID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="CityId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("StateID",new SearchColumn(){Name="StateID",Title="StateID",SelectClause="StateID",WhereClause="AllRecords.StateID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="StateId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("AcadmicQualification",new SearchColumn(){Name="AcadmicQualification",Title="AcadmicQualification",SelectClause="AcadmicQualification",WhereClause="AllRecords.AcadmicQualification",DataType="System.String",IsForeignColumn=false,PropertyName="AcadmicQualification",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Designation",new SearchColumn(){Name="Designation",Title="Designation",SelectClause="Designation",WhereClause="AllRecords.Designation",DataType="System.String",IsForeignColumn=false,PropertyName="Designation",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Salary",new SearchColumn(){Name="Salary",Title="Salary",SelectClause="Salary",WhereClause="AllRecords.Salary",DataType="System.Decimal?",IsForeignColumn=false,PropertyName="Salary",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("LoginID",new SearchColumn(){Name="LoginID",Title="LoginID",SelectClause="LoginID",WhereClause="AllRecords.LoginID",DataType="System.String",IsForeignColumn=false,PropertyName="LoginId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Password",new SearchColumn(){Name="Password",Title="Password",SelectClause="Password",WhereClause="AllRecords.Password",DataType="System.String",IsForeignColumn=false,PropertyName="Password",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ImagePath",new SearchColumn(){Name="ImagePath",Title="ImagePath",SelectClause="ImagePath",WhereClause="AllRecords.ImagePath",DataType="System.String",IsForeignColumn=false,PropertyName="ImagePath",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateBy",new SearchColumn(){Name="UpdateBy",Title="UpdateBy",SelectClause="UpdateBy",WhereClause="AllRecords.UpdateBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdateBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("SalaryTypeID",new SearchColumn(){Name="SalaryTypeID",Title="SalaryTypeID",SelectClause="SalaryTypeID",WhereClause="AllRecords.SalaryTypeID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="SalaryTypeId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("FatherName",new SearchColumn(){Name="FatherName",Title="FatherName",SelectClause="FatherName",WhereClause="AllRecords.FatherName",DataType="System.String",IsForeignColumn=false,PropertyName="FatherName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("AccountNumber",new SearchColumn(){Name="AccountNumber",Title="AccountNumber",SelectClause="AccountNumber",WhereClause="AllRecords.AccountNumber",DataType="System.String",IsForeignColumn=false,PropertyName="AccountNumber",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
            this.SearchColumns.Add("JoiningDate", new SearchColumn() { Name = "JoiningDate", Title = "JoiningDate", SelectClause = "JoiningDate", WhereClause = "AllRecords.JoiningDate", DataType = "System.DateTime?", IsForeignColumn = false, PropertyName = "JoiningDate", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
        }
        
		public virtual List<SearchColumn> GetUserSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetUserBasicSearchColumns()
        {
			Dictionary<string, string> columnList = new Dictionary<string, string>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                if (keyValuePair.Value.IsBasicSearchColumm)
                {
					keyValuePair.Value.Value = string.Empty;
                    columnList.Add(keyValuePair.Key, keyValuePair.Value.Title);
                }
            }
            return columnList;
        }

        public virtual List<SearchColumn> GetUserAdvanceSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                if (keyValuePair.Value.IsAdvanceSearchColumn)
                {
					keyValuePair.Value.Value = string.Empty;
					searchColumns.Add(keyValuePair.Value);
                }
            }
            return searchColumns;
        }
        
        
        public virtual string GetUserSelectClause()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            string selectQuery=string.Empty;
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                if (!keyValuePair.Value.IgnoreForDefaultSelect)
                {
					if (keyValuePair.Value.IsForeignColumn)
                	{
						if(string.IsNullOrEmpty(selectQuery))
						{
							selectQuery = "("+keyValuePair.Value.SelectClause+") as \""+keyValuePair.Key+"\"";
						}
						else
						{
							selectQuery += ",(" + keyValuePair.Value.SelectClause + ") as \"" + keyValuePair.Key + "\"";
						}
                	}
                	else
                	{
                    	if (string.IsNullOrEmpty(selectQuery))
                    	{
                        	selectQuery =  "[User]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[User]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<User> GetUserByUserTypeId(System.Int32? UserTypeId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [User] with (nolock)  where UserTypeID=@UserTypeID  ";
			SqlParameter parameter=new SqlParameter("@UserTypeID",UserTypeId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}

		public virtual List<User> GetUserByGenderId(System.Int32? GenderId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [User] with (nolock)  where GenderID=@GenderID  ";
			SqlParameter parameter=new SqlParameter("@GenderID",GenderId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}

		public virtual List<User> GetUserByReligionId(System.Int32? ReligionId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [User] with (nolock)  where ReligionID=@ReligionID  ";
			SqlParameter parameter=new SqlParameter("@ReligionID",ReligionId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}

		public virtual List<User> GetUserByCountryId(System.Int32? CountryId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [User] with (nolock)  where CountryID=@CountryID  ";
			SqlParameter parameter=new SqlParameter("@CountryID",CountryId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}
        //GetUserByDateRange New
        public virtual List<User> GetUserByDateRange(System.DateTime? Date, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetUserSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [User] with (nolock)  where Date=@StartDate between Date=@EndDate  ";
            SqlParameter parameter = new SqlParameter("@StartDate", Date);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<User>(ds, UserFromDataRow);
        }
        public virtual List<User> GetUserByCityId(System.Int32? CityId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [User] with (nolock)  where CityID=@CityID  ";
			SqlParameter parameter=new SqlParameter("@CityID",CityId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}

		public virtual List<User> GetUserByStateId(System.Int32? StateId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [User] with (nolock)  where StateID=@StateID  ";
			SqlParameter parameter=new SqlParameter("@StateID",StateId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}

		public virtual List<User> GetUserBySalaryTypeId(System.Int32? SalaryTypeId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [User] with (nolock)  where SalaryTypeID=@SalaryTypeID  ";
			SqlParameter parameter=new SqlParameter("@SalaryTypeID",SalaryTypeId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}

		public virtual User GetUser(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [User] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return UserFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<User> GetUserByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [User] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds,UserFromDataRow);
		}

		public virtual List<User> GetAllUser(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [User] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds, UserFromDataRow);
		}

		public virtual List<User> GetPagedUser(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetUserCount(whereClause, searchColumns);
			if(count>0)
			{
			if (count < startIndex) startIndex = (count / pageSize) * pageSize;			
			
           	int PageLowerBound = startIndex;
            int PageUpperBound = PageLowerBound + pageSize;
            string sql = @"CREATE TABLE #PageIndex
				            (
				                [IndexId] int IDENTITY (1, 1) NOT NULL,
				                [ID] int				   
				            );";

            //Insert into the temp table
            string tempsql = "INSERT INTO #PageIndex ([ID])";
            tempsql += " SELECT ";
            if (pageSize > 0) tempsql += "TOP " + PageUpperBound.ToString();
            tempsql += " [ID] ";
            tempsql += " FROM [User] AllRecords with (NOLOCK)";
            if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
            if (orderByClause.Length > 0) 
			{
				tempsql += " ORDER BY " + orderByClause;
				if( !orderByClause.Contains("ID"))
					tempsql += " , (AllRecords.[ID])"; 
			}
			else 
			{
				tempsql  += " ORDER BY (AllRecords.[ID])"; 
			}           
            
            // Return paged results
            string pagedResultsSql =
                (string.IsNullOrEmpty(SelectClause)? GetUserSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [User] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [User].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<User>(ds, UserFromDataRow);
			}else{ return null;}
		}

		private int GetUserCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM User as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM User as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(User))]
		public virtual User InsertUser(User entity)
		{

			User other=new User();
			other = entity;
			if(entity.IsTransient())
			{
				string sql= @"Insert into [User] ( [FirstName]
				,[MiddleName]
				,[LastName]
				,[UserTypeID]
				,[GenderID]
				,[DateOfBirth]
				,[NICNo]
				,[ReligionID]
				,[Address1]
				,[Address2]
				,[ZipCode]
				,[CountryID]
				,[CityID]
				,[StateID]
				,[AcadmicQualification]
				,[Designation]
				,[Salary]
				,[LoginID]
				,[Password]
				,[ImagePath]
				,[IsActive]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
				,[UserIP]
				,[SalaryTypeID]
				,[FatherName]
				,[AccountNumber] 
,[JoiningDate])
				Values
				( @FirstName
				, @MiddleName
				, @LastName
				, @UserTypeID
				, @GenderID
				, @DateOfBirth
				, @NICNo
				, @ReligionID
				, @Address1
				, @Address2
				, @ZipCode
				, @CountryID
				, @CityID
				, @StateID
				, @AcadmicQualification
				, @Designation
				, @Salary
				, @LoginID
				, @Password
				, @ImagePath
				, @IsActive
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
				, @UserIP
				, @SalaryTypeID
				, @FatherName
				, @AccountNumber
, @JoiningDate);
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@FirstName",entity.FirstName ?? (object)DBNull.Value)
					, new SqlParameter("@MiddleName",entity.MiddleName ?? (object)DBNull.Value)
					, new SqlParameter("@LastName",entity.LastName ?? (object)DBNull.Value)
					, new SqlParameter("@UserTypeID",entity.UserTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@GenderID",entity.GenderId ?? (object)DBNull.Value)
					, new SqlParameter("@DateOfBirth",entity.DateOfBirth ?? (object)DBNull.Value)
					, new SqlParameter("@NICNo",entity.NicNo ?? (object)DBNull.Value)
					, new SqlParameter("@ReligionID",entity.ReligionId ?? (object)DBNull.Value)
					, new SqlParameter("@Address1",entity.Address1 ?? (object)DBNull.Value)
					, new SqlParameter("@Address2",entity.Address2 ?? (object)DBNull.Value)
					, new SqlParameter("@ZipCode",entity.ZipCode ?? (object)DBNull.Value)
					, new SqlParameter("@CountryID",entity.CountryId ?? (object)DBNull.Value)
					, new SqlParameter("@CityID",entity.CityId ?? (object)DBNull.Value)
					, new SqlParameter("@StateID",entity.StateId ?? (object)DBNull.Value)
					, new SqlParameter("@AcadmicQualification",entity.AcadmicQualification ?? (object)DBNull.Value)
					, new SqlParameter("@Designation",entity.Designation ?? (object)DBNull.Value)
					, new SqlParameter("@Salary",entity.Salary ?? (object)DBNull.Value)
					, new SqlParameter("@LoginID",entity.LoginId ?? (object)DBNull.Value)
					, new SqlParameter("@Password",entity.Password ?? (object)DBNull.Value)
					, new SqlParameter("@ImagePath",entity.ImagePath ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@SalaryTypeID",entity.SalaryTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@FatherName",entity.FatherName ?? (object)DBNull.Value)
					, new SqlParameter("@AccountNumber",entity.AccountNumber ?? (object)DBNull.Value)
                , new SqlParameter("@JoiningDate",entity.JoiningDate ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetUser(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(User))]
		public virtual User UpdateUser(User entity)
		{

			if (entity.IsTransient()) return entity;
			User other = GetUser(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql= @"Update [User] set  [FirstName]=@FirstName
							, [MiddleName]=@MiddleName
							, [LastName]=@LastName
							, [UserTypeID]=@UserTypeID
							, [GenderID]=@GenderID
							, [DateOfBirth]=@DateOfBirth
							, [NICNo]=@NICNo
							, [ReligionID]=@ReligionID
							, [Address1]=@Address1
							, [Address2]=@Address2
							, [ZipCode]=@ZipCode
							, [CountryID]=@CountryID
							, [CityID]=@CityID
							, [StateID]=@StateID
							, [AcadmicQualification]=@AcadmicQualification
							, [Designation]=@Designation
							, [Salary]=@Salary
							, [LoginID]=@LoginID
							, [Password]=@Password
							, [ImagePath]=@ImagePath
							, [IsActive]=@IsActive
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP
							, [SalaryTypeID]=@SalaryTypeID
							, [FatherName]=@FatherName
							, [AccountNumber]=@AccountNumber 
, [JoiningDate]=@JoiningDate
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@FirstName",entity.FirstName ?? (object)DBNull.Value)
					, new SqlParameter("@MiddleName",entity.MiddleName ?? (object)DBNull.Value)
					, new SqlParameter("@LastName",entity.LastName ?? (object)DBNull.Value)
					, new SqlParameter("@UserTypeID",entity.UserTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@GenderID",entity.GenderId ?? (object)DBNull.Value)
					, new SqlParameter("@DateOfBirth",entity.DateOfBirth ?? (object)DBNull.Value)
					, new SqlParameter("@NICNo",entity.NicNo ?? (object)DBNull.Value)
					, new SqlParameter("@ReligionID",entity.ReligionId ?? (object)DBNull.Value)
					, new SqlParameter("@Address1",entity.Address1 ?? (object)DBNull.Value)
					, new SqlParameter("@Address2",entity.Address2 ?? (object)DBNull.Value)
					, new SqlParameter("@ZipCode",entity.ZipCode ?? (object)DBNull.Value)
					, new SqlParameter("@CountryID",entity.CountryId ?? (object)DBNull.Value)
					, new SqlParameter("@CityID",entity.CityId ?? (object)DBNull.Value)
					, new SqlParameter("@StateID",entity.StateId ?? (object)DBNull.Value)
					, new SqlParameter("@AcadmicQualification",entity.AcadmicQualification ?? (object)DBNull.Value)
					, new SqlParameter("@Designation",entity.Designation ?? (object)DBNull.Value)
					, new SqlParameter("@Salary",entity.Salary ?? (object)DBNull.Value)
					, new SqlParameter("@LoginID",entity.LoginId ?? (object)DBNull.Value)
					, new SqlParameter("@Password",entity.Password ?? (object)DBNull.Value)
					, new SqlParameter("@ImagePath",entity.ImagePath ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@SalaryTypeID",entity.SalaryTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@FatherName",entity.FatherName ?? (object)DBNull.Value)
					, new SqlParameter("@AccountNumber",entity.AccountNumber ?? (object)DBNull.Value)
                    , new SqlParameter("@JoiningDate",entity.JoiningDate ?? (object)DBNull.Value)
                    , new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetUser(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(User))]
		public virtual User UpdateUserByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [User] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetUser(Id);
		}

		public virtual bool DeleteUser(System.Int32 Id)
		{

			string sql="delete from [User] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(User))]
		public virtual User DeleteUser(User entity)
		{
			this.DeleteUser(entity.Id);
			return entity;
		}


		public virtual User UserFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			User entity=new User();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
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
			entity.UserTypeId = dr["UserTypeID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["UserTypeID"];
			}
			if (dr.Table.Columns.Contains("GenderID"))
			{
			entity.GenderId = dr["GenderID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["GenderID"];
			}
			if (dr.Table.Columns.Contains("DateOfBirth"))
			{
			entity.DateOfBirth = dr["DateOfBirth"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["DateOfBirth"];
			}
           
            if (dr.Table.Columns.Contains("NICNo"))
			{
			entity.NicNo = dr["NICNo"].ToString();
			}
			if (dr.Table.Columns.Contains("ReligionID"))
			{
			entity.ReligionId = dr["ReligionID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["ReligionID"];
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
			entity.CountryId = dr["CountryID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["CountryID"];
			}
			if (dr.Table.Columns.Contains("CityID"))
			{
			entity.CityId = dr["CityID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["CityID"];
			}
			if (dr.Table.Columns.Contains("StateID"))
			{
			entity.StateId = dr["StateID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["StateID"];
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
			entity.Salary = dr["Salary"]==DBNull.Value?(System.Decimal?)null:(System.Decimal?)dr["Salary"];
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
			entity.IsActive = dr["IsActive"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsActive"];
			}
			if (dr.Table.Columns.Contains("CreationDate"))
			{
			entity.CreationDate = dr["CreationDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["CreationDate"];
			}
			if (dr.Table.Columns.Contains("UpdateDate"))
			{
			entity.UpdateDate = dr["UpdateDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["UpdateDate"];
			}
			if (dr.Table.Columns.Contains("UpdateBy"))
			{
			entity.UpdateBy = dr["UpdateBy"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["UpdateBy"];
			}
			if (dr.Table.Columns.Contains("UserIP"))
			{
			entity.UserIp = dr["UserIP"].ToString();
			}
			if (dr.Table.Columns.Contains("SalaryTypeID"))
			{
			entity.SalaryTypeId = dr["SalaryTypeID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["SalaryTypeID"];
			}
			if (dr.Table.Columns.Contains("FatherName"))
			{
			entity.FatherName = dr["FatherName"].ToString();
			}
			if (dr.Table.Columns.Contains("AccountNumber"))
			{
			entity.AccountNumber = dr["AccountNumber"].ToString();
			}

            if (dr.Table.Columns.Contains("JoiningDate"))
            {
                entity.JoiningDate = dr["JoiningDate"] == DBNull.Value ? (System.DateTime?)null : (System.DateTime?)dr["JoiningDate"];
            }

            return entity;
		}

	}
	
	
}
