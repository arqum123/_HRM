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
		
	public abstract partial class BranchRepositoryBase : Repository, IBranchRepositoryBase 
	{
        
        public BranchRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Name",new SearchColumn(){Name="Name",Title="Name",SelectClause="Name",WhereClause="AllRecords.Name",DataType="System.String",IsForeignColumn=false,PropertyName="Name",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("AddressLine",new SearchColumn(){Name="AddressLine",Title="AddressLine",SelectClause="AddressLine",WhereClause="AllRecords.AddressLine",DataType="System.String",IsForeignColumn=false,PropertyName="AddressLine",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CityID",new SearchColumn(){Name="CityID",Title="CityID",SelectClause="CityID",WhereClause="AllRecords.CityID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="CityId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("StateID",new SearchColumn(){Name="StateID",Title="StateID",SelectClause="StateID",WhereClause="AllRecords.StateID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="StateId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CountryID",new SearchColumn(){Name="CountryID",Title="CountryID",SelectClause="CountryID",WhereClause="AllRecords.CountryID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="CountryId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ZipCode",new SearchColumn(){Name="ZipCode",Title="ZipCode",SelectClause="ZipCode",WhereClause="AllRecords.ZipCode",DataType="System.String",IsForeignColumn=false,PropertyName="ZipCode",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("PhoneNumber",new SearchColumn(){Name="PhoneNumber",Title="PhoneNumber",SelectClause="PhoneNumber",WhereClause="AllRecords.PhoneNumber",DataType="System.String",IsForeignColumn=false,PropertyName="PhoneNumber",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("GUID",new SearchColumn(){Name="GUID",Title="GUID",SelectClause="GUID",WhereClause="AllRecords.GUID",DataType="System.String",IsForeignColumn=false,PropertyName="Guid",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreatedDate",new SearchColumn(){Name="CreatedDate",Title="CreatedDate",SelectClause="CreatedDate",WhereClause="AllRecords.CreatedDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreatedDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdationDate",new SearchColumn(){Name="UpdationDate",Title="UpdationDate",SelectClause="UpdationDate",WhereClause="AllRecords.UpdationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdatedBy",new SearchColumn(){Name="UpdatedBy",Title="UpdatedBy",SelectClause="UpdatedBy",WhereClause="AllRecords.UpdatedBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdatedBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetBranchSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetBranchBasicSearchColumns()
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

        public virtual List<SearchColumn> GetBranchAdvanceSearchColumns()
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
        
        
        public virtual string GetBranchSelectClause()
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
                        	selectQuery =  "[Branch]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[Branch]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual Branch GetBranch(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Branch] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return BranchFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<Branch> GetBranchByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [Branch] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Branch>(ds,BranchFromDataRow);
		}

		public virtual List<Branch> GetAllBranch(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Branch] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Branch>(ds, BranchFromDataRow);
		}

		public virtual List<Branch> GetPagedBranch(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetBranchCount(whereClause, searchColumns);
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
            tempsql += " FROM [Branch] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetBranchSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [Branch] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [Branch].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Branch>(ds, BranchFromDataRow);
			}else{ return null;}
		}

		private int GetBranchCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM Branch as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM Branch as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(Branch))]
		public virtual Branch InsertBranch(Branch entity)
		{

			Branch other=new Branch();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [Branch] ( [Name]
				,[AddressLine]
				,[CityID]
				,[StateID]
				,[CountryID]
				,[ZipCode]
				,[PhoneNumber]
				,[GUID]
				,[CreatedDate]
				,[UpdationDate]
				,[UpdatedBy]
				,[UserIP]
				,[IsActive] )
				Values
				( @Name
				, @AddressLine
				, @CityID
				, @StateID
				, @CountryID
				, @ZipCode
				, @PhoneNumber
				, @GUID
				, @CreatedDate
				, @UpdationDate
				, @UpdatedBy
				, @UserIP
				, @IsActive );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@Name",entity.Name ?? (object)DBNull.Value)
					, new SqlParameter("@AddressLine",entity.AddressLine ?? (object)DBNull.Value)
					, new SqlParameter("@CityID",entity.CityId ?? (object)DBNull.Value)
					, new SqlParameter("@StateID",entity.StateId ?? (object)DBNull.Value)
					, new SqlParameter("@CountryID",entity.CountryId ?? (object)DBNull.Value)
					, new SqlParameter("@ZipCode",entity.ZipCode ?? (object)DBNull.Value)
					, new SqlParameter("@PhoneNumber",entity.PhoneNumber ?? (object)DBNull.Value)
					, new SqlParameter("@GUID",entity.Guid ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdationDate",entity.UpdationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetBranch(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(Branch))]
		public virtual Branch UpdateBranch(Branch entity)
		{

			if (entity.IsTransient()) return entity;
			Branch other = GetBranch(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [Branch] set  [Name]=@Name
							, [AddressLine]=@AddressLine
							, [CityID]=@CityID
							, [StateID]=@StateID
							, [CountryID]=@CountryID
							, [ZipCode]=@ZipCode
							, [PhoneNumber]=@PhoneNumber
							, [GUID]=@GUID
							, [UpdationDate]=@UpdationDate
							, [UpdatedBy]=@UpdatedBy
							, [UserIP]=@UserIP
							, [IsActive]=@IsActive 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@Name",entity.Name ?? (object)DBNull.Value)
					, new SqlParameter("@AddressLine",entity.AddressLine ?? (object)DBNull.Value)
					, new SqlParameter("@CityID",entity.CityId ?? (object)DBNull.Value)
					, new SqlParameter("@StateID",entity.StateId ?? (object)DBNull.Value)
					, new SqlParameter("@CountryID",entity.CountryId ?? (object)DBNull.Value)
					, new SqlParameter("@ZipCode",entity.ZipCode ?? (object)DBNull.Value)
					, new SqlParameter("@PhoneNumber",entity.PhoneNumber ?? (object)DBNull.Value)
					, new SqlParameter("@GUID",entity.Guid ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdationDate",entity.UpdationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetBranch(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(Branch))]
		public virtual Branch UpdateBranchByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [Branch] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetBranch(Id);
		}

		public virtual bool DeleteBranch(System.Int32 Id)
		{

			string sql="delete from [Branch] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(Branch))]
		public virtual Branch DeleteBranch(Branch entity)
		{
			this.DeleteBranch(entity.Id);
			return entity;
		}


		public virtual Branch BranchFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			Branch entity=new Branch();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("Name"))
			{
			entity.Name = dr["Name"].ToString();
			}
			if (dr.Table.Columns.Contains("AddressLine"))
			{
			entity.AddressLine = dr["AddressLine"].ToString();
			}
			if (dr.Table.Columns.Contains("CityID"))
			{
			entity.CityId = dr["CityID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["CityID"];
			}
			if (dr.Table.Columns.Contains("StateID"))
			{
			entity.StateId = dr["StateID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["StateID"];
			}
			if (dr.Table.Columns.Contains("CountryID"))
			{
			entity.CountryId = dr["CountryID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["CountryID"];
			}
			if (dr.Table.Columns.Contains("ZipCode"))
			{
			entity.ZipCode = dr["ZipCode"].ToString();
			}
			if (dr.Table.Columns.Contains("PhoneNumber"))
			{
			entity.PhoneNumber = dr["PhoneNumber"].ToString();
			}
			if (dr.Table.Columns.Contains("GUID"))
			{
			entity.Guid = dr["GUID"].ToString();
			}
			if (dr.Table.Columns.Contains("CreatedDate"))
			{
			entity.CreatedDate = dr["CreatedDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["CreatedDate"];
			}
			if (dr.Table.Columns.Contains("UpdationDate"))
			{
			entity.UpdationDate = dr["UpdationDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["UpdationDate"];
			}
			if (dr.Table.Columns.Contains("UpdatedBy"))
			{
			entity.UpdatedBy = dr["UpdatedBy"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["UpdatedBy"];
			}
			if (dr.Table.Columns.Contains("UserIP"))
			{
			entity.UserIp = dr["UserIP"].ToString();
			}
			if (dr.Table.Columns.Contains("IsActive"))
			{
			entity.IsActive = dr["IsActive"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsActive"];
			}
			return entity;
		}

	}
	
	
}
