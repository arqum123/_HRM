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
		
	public abstract partial class LeaveRepositoryBase : Repository, ILeaveRepositoryBase 
	{
        
        public LeaveRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserID",new SearchColumn(){Name="UserID",Title="UserID",SelectClause="UserID",WhereClause="AllRecords.UserID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UserId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Date",new SearchColumn(){Name="Date",Title="Date",SelectClause="Date",WhereClause="AllRecords.Date",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="Date",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Reason",new SearchColumn(){Name="Reason",Title="Reason",SelectClause="Reason",WhereClause="AllRecords.Reason",DataType="System.String",IsForeignColumn=false,PropertyName="Reason",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("LeaveTypeID",new SearchColumn(){Name="LeaveTypeID",Title="LeaveTypeID",SelectClause="LeaveTypeID",WhereClause="AllRecords.LeaveTypeID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="LeaveTypeId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdatedBy",new SearchColumn(){Name="UpdatedBy",Title="UpdatedBy",SelectClause="UpdatedBy",WhereClause="AllRecords.UpdatedBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdatedBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
            this.SearchColumns.Add("IsApproved", new SearchColumn() { Name = "IsApproved", Title = "IsApproved", SelectClause = "IsApproved", WhereClause = "AllRecords.IsApproved", DataType = "System.Boolean?", IsForeignColumn = false, PropertyName = "IsApproved", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("IsReject", new SearchColumn() { Name = "IsReject", Title = "IsReject", SelectClause = "IsReject", WhereClause = "AllRecords.IsReject", DataType = "System.Boolean?", IsForeignColumn = false, PropertyName = "IsReject", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("AdminReason", new SearchColumn() { Name = "AdminReason", Title = "AdminReason", SelectClause = "AdminReason", WhereClause = "AllRecords.AdminReason", DataType = "System.String", IsForeignColumn = false, PropertyName = "AdminReason", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });

        }
        
		public virtual List<SearchColumn> GetLeaveSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetLeaveBasicSearchColumns()
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

        public virtual List<SearchColumn> GetLeaveAdvanceSearchColumns()
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
        
        
        public virtual string GetLeaveSelectClause()
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
                        	selectQuery =  "[Leave]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[Leave]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<Leave> GetLeaveByUserId(System.Int32? UserId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetLeaveSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [Leave] with (nolock)  where UserID=@UserID  ";
			SqlParameter parameter=new SqlParameter("@UserID",UserId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Leave>(ds,LeaveFromDataRow);
		}


		public virtual List<Leave> GetLeaveByLeaveTypeId(System.Int32? LeaveTypeId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetLeaveSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [Leave] with (nolock)  where LeaveTypeID=@LeaveTypeID  ";
			SqlParameter parameter=new SqlParameter("@LeaveTypeID",LeaveTypeId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Leave>(ds,LeaveFromDataRow);
		}

		public virtual Leave GetLeave(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetLeaveSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Leave] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return LeaveFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<Leave> GetLeaveByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetLeaveSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [Leave] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Leave>(ds,LeaveFromDataRow);
		}

		public virtual List<Leave> GetAllLeave(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetLeaveSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Leave] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Leave>(ds, LeaveFromDataRow);
		}

		public virtual List<Leave> GetPagedLeave(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetLeaveCount(whereClause, searchColumns);
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
            tempsql += " FROM [Leave] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetLeaveSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [Leave] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [Leave].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Leave>(ds, LeaveFromDataRow);
			}else{ return null;}
		}

		private int GetLeaveCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM Leave as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM Leave as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(Leave))]
		public virtual Leave InsertLeave(Leave entity)
		{

			Leave other=new Leave();
			other = entity;
			if(entity.IsTransient())
			{
				string sql= @"Insert into [Leave] ( [UserID]
				,[Date]
				,[Reason]
				,[LeaveTypeID]
				,[IsActive]
				,[UpdateDate]
				,[UpdatedBy]
				,[UserIP]
				,[CreationDate] 
	,[IsReject] 
	,[AdminReason] 
,[IsApproved])
				Values
				( @UserID
				, @Date
				, @Reason
				, @LeaveTypeID
				, @IsActive
				, @UpdateDate
				, @UpdatedBy
				, @UserIP
				, @CreationDate
, @IsReject
, @AdminReason

, @IsApproved);
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@UserID",entity.UserId ?? (object)DBNull.Value)
					, new SqlParameter("@Date",entity.Date ?? (object)DBNull.Value)
					, new SqlParameter("@Reason",entity.Reason ?? (object)DBNull.Value)
					, new SqlParameter("@LeaveTypeID",entity.LeaveTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
                    , new SqlParameter("@IsReject",entity.IsReject ?? (object)DBNull.Value)
                    , new SqlParameter("@AdminReason",entity.AdminReason ?? (object)DBNull.Value)
                , new SqlParameter("@IsApproved",entity.IsApproved ?? (object)DBNull.Value)
                };
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetLeave(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(Leave))]
		public virtual Leave UpdateLeave(Leave entity)
		{

			if (entity.IsTransient()) return entity;
			Leave other = GetLeave(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql= @"Update [Leave] set  [UserID]=@UserID
							, [Date]=@Date
							, [Reason]=@Reason
							, [LeaveTypeID]=@LeaveTypeID
							, [IsActive]=@IsActive
							, [UpdateDate]=@UpdateDate
							, [UpdatedBy]=@UpdatedBy
							, [UserIP]=@UserIP
							, [CreationDate]=@CreationDate 
, [IsReject]=@IsReject 
, [AdminReason]=@AdminReason 
, [IsApproved]=@IsApproved
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@UserID",entity.UserId ?? (object)DBNull.Value)
					, new SqlParameter("@Date",entity.Date ?? (object)DBNull.Value)
					, new SqlParameter("@Reason",entity.Reason ?? (object)DBNull.Value)
					, new SqlParameter("@LeaveTypeID",entity.LeaveTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
                        , new SqlParameter("@IsReject",entity.IsReject ?? (object)DBNull.Value)
                            , new SqlParameter("@AdminReason",entity.AdminReason ?? (object)DBNull.Value)
                    , new SqlParameter("@IsApproved",entity.IsApproved ?? (object)DBNull.Value)
                    , new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetLeave(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(Leave))]
		public virtual Leave UpdateLeaveByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [Leave] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetLeave(Id);
		}

		public virtual bool DeleteLeave(System.Int32 Id)
		{

			string sql="delete from [Leave] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(Leave))]
		public virtual Leave DeleteLeave(Leave entity)
		{
			this.DeleteLeave(entity.Id);
			return entity;
		}


		public virtual Leave LeaveFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			Leave entity=new Leave();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("UserID"))
			{
			entity.UserId = dr["UserID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["UserID"];
			}
			if (dr.Table.Columns.Contains("Date"))
			{
			entity.Date = dr["Date"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["Date"];
			}
			if (dr.Table.Columns.Contains("Reason"))
			{
			entity.Reason = dr["Reason"].ToString();
			}
			if (dr.Table.Columns.Contains("LeaveTypeID"))
			{
			entity.LeaveTypeId = dr["LeaveTypeID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["LeaveTypeID"];
			}
			if (dr.Table.Columns.Contains("IsActive"))
			{
			entity.IsActive = dr["IsActive"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsActive"];
			}
			if (dr.Table.Columns.Contains("UpdateDate"))
			{
			entity.UpdateDate = dr["UpdateDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["UpdateDate"];
			}
			if (dr.Table.Columns.Contains("UpdatedBy"))
			{
			entity.UpdatedBy = dr["UpdatedBy"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["UpdatedBy"];
			}
			if (dr.Table.Columns.Contains("UserIP"))
			{
			entity.UserIp = dr["UserIP"].ToString();
			}
			if (dr.Table.Columns.Contains("CreationDate"))
			{
			entity.CreationDate = dr["CreationDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["CreationDate"];
			}
            if (dr.Table.Columns.Contains("IsApproved"))
            {
                entity.IsApproved = dr["IsApproved"] == DBNull.Value ? (System.Boolean?)null : (System.Boolean?)dr["IsApproved"];
            }
            if (dr.Table.Columns.Contains("IsReject"))
            {
                entity.IsReject = dr["IsReject"] == DBNull.Value ? (System.Boolean?)null : (System.Boolean?)dr["IsReject"];
            }
            if (dr.Table.Columns.Contains("AdminReason"))
            {
                entity.AdminReason = dr["AdminReason"].ToString();
            }
            return entity;
		}

	}
	
	
}
