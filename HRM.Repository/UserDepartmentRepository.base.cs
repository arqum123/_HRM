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
		
	public abstract partial class UserDepartmentRepositoryBase : Repository, IUserDepartmentRepositoryBase 
	{
        
        public UserDepartmentRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserID",new SearchColumn(){Name="UserID",Title="UserID",SelectClause="UserID",WhereClause="AllRecords.UserID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UserId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DepartmentID",new SearchColumn(){Name="DepartmentID",Title="DepartmentID",SelectClause="DepartmentID",WhereClause="AllRecords.DepartmentID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="DepartmentId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("EffectiveDate",new SearchColumn(){Name="EffectiveDate",Title="EffectiveDate",SelectClause="EffectiveDate",WhereClause="AllRecords.EffectiveDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="EffectiveDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("RetiredDate",new SearchColumn(){Name="RetiredDate",Title="RetiredDate",SelectClause="RetiredDate",WhereClause="AllRecords.RetiredDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="RetiredDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateBy",new SearchColumn(){Name="UpdateBy",Title="UpdateBy",SelectClause="UpdateBy",WhereClause="AllRecords.UpdateBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdateBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetUserDepartmentSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetUserDepartmentBasicSearchColumns()
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

        public virtual List<SearchColumn> GetUserDepartmentAdvanceSearchColumns()
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
        
        
        public virtual string GetUserDepartmentSelectClause()
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
                        	selectQuery =  "[UserDepartment]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[UserDepartment]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<UserDepartment> GetUserDepartmentByUserId(System.Int32? UserId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserDepartmentSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [UserDepartment] with (nolock)  where UserID=@UserID  ";
			SqlParameter parameter=new SqlParameter("@UserID",UserId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<UserDepartment>(ds,UserDepartmentFromDataRow);
		}

		public virtual List<UserDepartment> GetUserDepartmentByDepartmentId(System.Int32? DepartmentId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserDepartmentSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [UserDepartment] with (nolock)  where DepartmentID=@DepartmentID  ";
			SqlParameter parameter=new SqlParameter("@DepartmentID",DepartmentId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<UserDepartment>(ds,UserDepartmentFromDataRow);
		}

		public virtual UserDepartment GetUserDepartment(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserDepartmentSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [UserDepartment] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return UserDepartmentFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<UserDepartment> GetUserDepartmentByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserDepartmentSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [UserDepartment] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<UserDepartment>(ds,UserDepartmentFromDataRow);
		}

		public virtual List<UserDepartment> GetAllUserDepartment(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetUserDepartmentSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [UserDepartment] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<UserDepartment>(ds, UserDepartmentFromDataRow);
		}

		public virtual List<UserDepartment> GetPagedUserDepartment(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetUserDepartmentCount(whereClause, searchColumns);
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
            tempsql += " FROM [UserDepartment] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetUserDepartmentSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [UserDepartment] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [UserDepartment].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<UserDepartment>(ds, UserDepartmentFromDataRow);
			}else{ return null;}
		}

		private int GetUserDepartmentCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM UserDepartment as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM UserDepartment as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(UserDepartment))]
		public virtual UserDepartment InsertUserDepartment(UserDepartment entity)
		{

			UserDepartment other=new UserDepartment();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [UserDepartment] ( [UserID]
				,[DepartmentID]
				,[EffectiveDate]
				,[RetiredDate]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
				,[UserIP] )
				Values
				( @UserID
				, @DepartmentID
				, @EffectiveDate
				, @RetiredDate
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
				, @UserIP );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@UserID",entity.UserId ?? (object)DBNull.Value)
					, new SqlParameter("@DepartmentID",entity.DepartmentId ?? (object)DBNull.Value)
					, new SqlParameter("@EffectiveDate",entity.EffectiveDate ?? (object)DBNull.Value)
					, new SqlParameter("@RetiredDate",entity.RetiredDate ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetUserDepartment(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(UserDepartment))]
		public virtual UserDepartment UpdateUserDepartment(UserDepartment entity)
		{

			if (entity.IsTransient()) return entity;
			UserDepartment other = GetUserDepartment(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [UserDepartment] set  [UserID]=@UserID
							, [DepartmentID]=@DepartmentID
							, [EffectiveDate]=@EffectiveDate
							, [RetiredDate]=@RetiredDate
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@UserID",entity.UserId ?? (object)DBNull.Value)
					, new SqlParameter("@DepartmentID",entity.DepartmentId ?? (object)DBNull.Value)
					, new SqlParameter("@EffectiveDate",entity.EffectiveDate ?? (object)DBNull.Value)
					, new SqlParameter("@RetiredDate",entity.RetiredDate ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetUserDepartment(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(UserDepartment))]
		public virtual UserDepartment UpdateUserDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [UserDepartment] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetUserDepartment(Id);
		}

		public virtual bool DeleteUserDepartment(System.Int32 Id)
		{

			string sql="delete from [UserDepartment] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(UserDepartment))]
		public virtual UserDepartment DeleteUserDepartment(UserDepartment entity)
		{
			this.DeleteUserDepartment(entity.Id);
			return entity;
		}


		public virtual UserDepartment UserDepartmentFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			UserDepartment entity=new UserDepartment();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("UserID"))
			{
			entity.UserId = dr["UserID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["UserID"];
			}
			if (dr.Table.Columns.Contains("DepartmentID"))
			{
			entity.DepartmentId = dr["DepartmentID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["DepartmentID"];
			}
			if (dr.Table.Columns.Contains("EffectiveDate"))
			{
			entity.EffectiveDate = dr["EffectiveDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["EffectiveDate"];
			}
			if (dr.Table.Columns.Contains("RetiredDate"))
			{
			entity.RetiredDate = dr["RetiredDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["RetiredDate"];
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
			return entity;
		}

	}
	
	
}
