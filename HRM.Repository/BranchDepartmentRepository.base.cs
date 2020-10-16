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
		
	public abstract partial class BranchDepartmentRepositoryBase : Repository, IBranchDepartmentRepositoryBase 
	{
        
        public BranchDepartmentRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("BranchID",new SearchColumn(){Name="BranchID",Title="BranchID",SelectClause="BranchID",WhereClause="AllRecords.BranchID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="BranchId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DepartmentID",new SearchColumn(){Name="DepartmentID",Title="DepartmentID",SelectClause="DepartmentID",WhereClause="AllRecords.DepartmentID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="DepartmentId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreatedDate",new SearchColumn(){Name="CreatedDate",Title="CreatedDate",SelectClause="CreatedDate",WhereClause="AllRecords.CreatedDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreatedDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdatedDate",new SearchColumn(){Name="UpdatedDate",Title="UpdatedDate",SelectClause="UpdatedDate",WhereClause="AllRecords.UpdatedDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdatedDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdatedBy",new SearchColumn(){Name="UpdatedBy",Title="UpdatedBy",SelectClause="UpdatedBy",WhereClause="AllRecords.UpdatedBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdatedBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetBranchDepartmentSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetBranchDepartmentBasicSearchColumns()
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

        public virtual List<SearchColumn> GetBranchDepartmentAdvanceSearchColumns()
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
        
        
        public virtual string GetBranchDepartmentSelectClause()
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
                        	selectQuery =  "[BranchDepartment]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[BranchDepartment]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<BranchDepartment> GetBranchDepartmentByBranchId(System.Int32? BranchId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchDepartmentSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [BranchDepartment] with (nolock)  where BranchID=@BranchID  ";
			SqlParameter parameter=new SqlParameter("@BranchID",BranchId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchDepartment>(ds,BranchDepartmentFromDataRow);
		}

		public virtual List<BranchDepartment> GetBranchDepartmentByDepartmentId(System.Int32? DepartmentId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchDepartmentSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [BranchDepartment] with (nolock)  where DepartmentID=@DepartmentID  ";
			SqlParameter parameter=new SqlParameter("@DepartmentID",DepartmentId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchDepartment>(ds,BranchDepartmentFromDataRow);
		}

		public virtual BranchDepartment GetBranchDepartment(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchDepartmentSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [BranchDepartment] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return BranchDepartmentFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<BranchDepartment> GetBranchDepartmentByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchDepartmentSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [BranchDepartment] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchDepartment>(ds,BranchDepartmentFromDataRow);
		}

		public virtual List<BranchDepartment> GetAllBranchDepartment(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchDepartmentSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [BranchDepartment] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchDepartment>(ds, BranchDepartmentFromDataRow);
		}

		public virtual List<BranchDepartment> GetPagedBranchDepartment(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetBranchDepartmentCount(whereClause, searchColumns);
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
            tempsql += " FROM [BranchDepartment] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetBranchDepartmentSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [BranchDepartment] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [BranchDepartment].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchDepartment>(ds, BranchDepartmentFromDataRow);
			}else{ return null;}
		}

		private int GetBranchDepartmentCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM BranchDepartment as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM BranchDepartment as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(BranchDepartment))]
		public virtual BranchDepartment InsertBranchDepartment(BranchDepartment entity)
		{

			BranchDepartment other=new BranchDepartment();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [BranchDepartment] ( [BranchID]
				,[DepartmentID]
				,[CreatedDate]
				,[UpdatedDate]
				,[UpdatedBy]
				,[UserIP]
				,[IsActive] )
				Values
				( @BranchID
				, @DepartmentID
				, @CreatedDate
				, @UpdatedDate
				, @UpdatedBy
				, @UserIP
				, @IsActive );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@BranchID",entity.BranchId ?? (object)DBNull.Value)
					, new SqlParameter("@DepartmentID",entity.DepartmentId ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedDate",entity.UpdatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetBranchDepartment(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(BranchDepartment))]
		public virtual BranchDepartment UpdateBranchDepartment(BranchDepartment entity)
		{

			if (entity.IsTransient()) return entity;
			BranchDepartment other = GetBranchDepartment(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [BranchDepartment] set  [BranchID]=@BranchID
							, [DepartmentID]=@DepartmentID
							, [UpdatedDate]=@UpdatedDate
							, [UpdatedBy]=@UpdatedBy
							, [UserIP]=@UserIP
							, [IsActive]=@IsActive 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@BranchID",entity.BranchId ?? (object)DBNull.Value)
					, new SqlParameter("@DepartmentID",entity.DepartmentId ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedDate",entity.UpdatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetBranchDepartment(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(BranchDepartment))]
		public virtual BranchDepartment UpdateBranchDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [BranchDepartment] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetBranchDepartment(Id);
		}

		public virtual bool DeleteBranchDepartment(System.Int32 Id)
		{

			string sql="delete from [BranchDepartment] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(BranchDepartment))]
		public virtual BranchDepartment DeleteBranchDepartment(BranchDepartment entity)
		{
			this.DeleteBranchDepartment(entity.Id);
			return entity;
		}


		public virtual BranchDepartment BranchDepartmentFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			BranchDepartment entity=new BranchDepartment();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("BranchID"))
			{
			entity.BranchId = dr["BranchID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["BranchID"];
			}
			if (dr.Table.Columns.Contains("DepartmentID"))
			{
			entity.DepartmentId = dr["DepartmentID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["DepartmentID"];
			}
			if (dr.Table.Columns.Contains("CreatedDate"))
			{
			entity.CreatedDate = dr["CreatedDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["CreatedDate"];
			}
			if (dr.Table.Columns.Contains("UpdatedDate"))
			{
			entity.UpdatedDate = dr["UpdatedDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["UpdatedDate"];
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
