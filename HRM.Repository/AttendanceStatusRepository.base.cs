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
		
	public abstract partial class AttendanceStatusRepositoryBase : Repository, IAttendanceStatusRepositoryBase 
	{
        
        public AttendanceStatusRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("AttendanceID",new SearchColumn(){Name="AttendanceID",Title="AttendanceID",SelectClause="AttendanceID",WhereClause="AllRecords.AttendanceID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="AttendanceId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsShiftOffDay",new SearchColumn(){Name="IsShiftOffDay",Title="IsShiftOffDay",SelectClause="IsShiftOffDay",WhereClause="AllRecords.IsShiftOffDay",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsShiftOffDay",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsHoliday",new SearchColumn(){Name="IsHoliday",Title="IsHoliday",SelectClause="IsHoliday",WhereClause="AllRecords.IsHoliday",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsHoliday",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsLeaveDay",new SearchColumn(){Name="IsLeaveDay",Title="IsLeaveDay",SelectClause="IsLeaveDay",WhereClause="AllRecords.IsLeaveDay",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsLeaveDay",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsQuarterDay",new SearchColumn(){Name="IsQuarterDay",Title="IsQuarterDay",SelectClause="IsQuarterDay",WhereClause="AllRecords.IsQuarterDay",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsQuarterDay",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsHalfDay",new SearchColumn(){Name="IsHalfDay",Title="IsHalfDay",SelectClause="IsHalfDay",WhereClause="AllRecords.IsHalfDay",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsHalfDay",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsFullDay",new SearchColumn(){Name="IsFullDay",Title="IsFullDay",SelectClause="IsFullDay",WhereClause="AllRecords.IsFullDay",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsFullDay",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Reason",new SearchColumn(){Name="Reason",Title="Reason",SelectClause="Reason",WhereClause="AllRecords.Reason",DataType="System.String",IsForeignColumn=false,PropertyName="Reason",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsApproved",new SearchColumn(){Name="IsApproved",Title="IsApproved",SelectClause="IsApproved",WhereClause="AllRecords.IsApproved",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsApproved",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Remarks",new SearchColumn(){Name="Remarks",Title="Remarks",SelectClause="Remarks",WhereClause="AllRecords.Remarks",DataType="System.String",IsForeignColumn=false,PropertyName="Remarks",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ActionBy",new SearchColumn(){Name="ActionBy",Title="ActionBy",SelectClause="ActionBy",WhereClause="AllRecords.ActionBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="ActionBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ActionDate",new SearchColumn(){Name="ActionDate",Title="ActionDate",SelectClause="ActionDate",WhereClause="AllRecords.ActionDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="ActionDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateBy",new SearchColumn(){Name="UpdateBy",Title="UpdateBy",SelectClause="UpdateBy",WhereClause="AllRecords.UpdateBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdateBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsLate",new SearchColumn(){Name="IsLate",Title="IsLate",SelectClause="IsLate",WhereClause="AllRecords.IsLate",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsLate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsEarly",new SearchColumn(){Name="IsEarly",Title="IsEarly",SelectClause="IsEarly",WhereClause="AllRecords.IsEarly",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsEarly",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("LateMinutes",new SearchColumn(){Name="LateMinutes",Title="LateMinutes",SelectClause="LateMinutes",WhereClause="AllRecords.LateMinutes",DataType="System.Int32?",IsForeignColumn=false,PropertyName="LateMinutes",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("EarlyMinutes",new SearchColumn(){Name="EarlyMinutes",Title="EarlyMinutes",SelectClause="EarlyMinutes",WhereClause="AllRecords.EarlyMinutes",DataType="System.Int32?",IsForeignColumn=false,PropertyName="EarlyMinutes",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("WorkingMinutes",new SearchColumn(){Name="WorkingMinutes",Title="WorkingMinutes",SelectClause="WorkingMinutes",WhereClause="AllRecords.WorkingMinutes",DataType="System.Int32?",IsForeignColumn=false,PropertyName="WorkingMinutes",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("TotalMinutes",new SearchColumn(){Name="TotalMinutes",Title="TotalMinutes",SelectClause="TotalMinutes",WhereClause="AllRecords.TotalMinutes",DataType="System.Int32?",IsForeignColumn=false,PropertyName="TotalMinutes",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("OverTimeMinutes",new SearchColumn(){Name="OverTimeMinutes",Title="OverTimeMinutes",SelectClause="OverTimeMinutes",WhereClause="AllRecords.OverTimeMinutes",DataType="System.Int32?",IsForeignColumn=false,PropertyName="OverTimeMinutes",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
            this.SearchColumns.Add("BreakType", new SearchColumn() { Name = "BreakType", Title = "BreakType", SelectClause = "BreakType", WhereClause = "AllRecords.BreakType", DataType = "System.String", IsForeignColumn = false, PropertyName = "BreakType", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });                
        }
        
		public virtual List<SearchColumn> GetAttendanceStatusSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetAttendanceStatusBasicSearchColumns()
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

        public virtual List<SearchColumn> GetAttendanceStatusAdvanceSearchColumns()
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
        
        
        public virtual string GetAttendanceStatusSelectClause()
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
                        	selectQuery =  "[AttendanceStatus]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[AttendanceStatus]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
    
        public virtual List<AttendanceStatus> GetAttendanceStatusByDateRangeSharp(DateTime dtStart, DateTime dtEnd)    //New
        {
            string sql = ""; // string.IsNullOrEmpty(SelectClause) ? GetAttendanceSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "SELECT AtS.ID ,AtS.IsShiftOffDay,AtS.IsHoliday,AtS.IsLeaveDay,AtS.IsQuarterDay,AtS.IsHalfDay,AtS.IsFullDay,AtS.IsApproved,AtS.IsLate,AtS.IsEarly,Ats.IsActive ";
            sql += "from [Attendance] A Inner JOIN AttendanceStatus AtS ON A.ID = AtS.AttendanceID Where A.[Date] BETWEEN @StartDate AND @EndDate ORDER BY A.ID  ";

            SqlParameter[] parameterArray = new SqlParameter[]
            {
                new SqlParameter("@StartDate", dtStart)
                , new SqlParameter("@EndDate", dtEnd)
            };
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, parameterArray);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<AttendanceStatus>(ds, AttendanceStatusFromDataRow);
        }
        public virtual List<AttendanceStatus> GetAttendanceStatusByAttendanceId(System.Int32? AttendanceId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendanceStatusSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [AttendanceStatus] with (nolock)  where AttendanceID=@AttendanceID  ";
			SqlParameter parameter=new SqlParameter("@AttendanceID",AttendanceId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendanceStatus>(ds,AttendanceStatusFromDataRow);
		}
      

        public virtual AttendanceStatus GetAttendanceStatus(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendanceStatusSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [AttendanceStatus] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return AttendanceStatusFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<AttendanceStatus> GetAttendanceStatusByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendanceStatusSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [AttendanceStatus] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendanceStatus>(ds,AttendanceStatusFromDataRow);
		}

		public virtual List<AttendanceStatus> GetAllAttendanceStatus(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendanceStatusSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [AttendanceStatus] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendanceStatus>(ds, AttendanceStatusFromDataRow);
		}

        //
        public virtual List<AttendanceStatus> GetAllAttendanceStatusStartAndEndDate(string StartDate, string EndDate,string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetAttendanceStatusSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += "from [Attendance] with (nolock)  where Date between @StartDate AND @EndDate";
            SqlParameter parameter = new SqlParameter("@StartDate", StartDate);
            SqlParameter parameter1 = new SqlParameter("@EndDate", EndDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter ,parameter1});
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return CollectionFromDataSet<AttendanceStatus>(ds, AttendanceStatusFromDataRow);
        }

        public virtual List<AttendanceStatus> GetAttendanceStatusByDateRange(DateTime StartDate, DateTime EndDate, string SelectClause = null)
        {
            string sql = string.IsNullOrEmpty(SelectClause) ? GetAttendanceStatusSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [Attendance] A Inner JOIN AttendanceStatus AtS ON A.ID = AtS.AttendanceID Where A.[Date] BETWEEN @StartDate AND @EndDate ORDER BY A.ID";
            SqlParameter parameter1 = new SqlParameter("@StartDate", StartDate);
            SqlParameter parameter2 = new SqlParameter("@EndDate", EndDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter1, parameter2 });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<AttendanceStatus>(ds, AttendanceStatusFromDataRow);
        }
        //public List<AttendanceStatus> GetAttendanceStatusByDateRange(DateTime StartDate, DateTime EndDate)
        //{
        //    string sql = "SELECT_AttendanceDetailForUpdate";
        //    List<SqlParameter> sqlparams = new List<SqlParameter>();
        //    sqlparams.Add(new SqlParameter() { ParameterName = "@StartDate", Value = StartDate });
        //    sqlparams.Add(new SqlParameter() { ParameterName = "@EndDate", Value = EndDate });
        //    return SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.StoredProcedure, sql, sqlparams.ToArray());
        //}
        public virtual List<AttendanceStatus> GetPagedAttendanceStatus(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetAttendanceStatusCount(whereClause, searchColumns);
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
            tempsql += " FROM [AttendanceStatus] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetAttendanceStatusSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [AttendanceStatus] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [AttendanceStatus].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendanceStatus>(ds, AttendanceStatusFromDataRow);
			}else{ return null;}
		}

		private int GetAttendanceStatusCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM AttendanceStatus as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM AttendanceStatus as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(AttendanceStatus))]
		public virtual AttendanceStatus InsertAttendanceStatus(AttendanceStatus entity)
		{

			AttendanceStatus other=new AttendanceStatus();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [AttendanceStatus] ( [AttendanceID]
				,[IsShiftOffDay]
				,[IsHoliday]
				,[IsLeaveDay]
				,[IsQuarterDay]
				,[IsHalfDay]
				,[IsFullDay]
				,[Reason]
				,[IsApproved]
				,[Remarks]
				,[ActionBy]
				,[ActionDate]
				,[IsActive]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
				,[UserIP]
				,[IsLate]
				,[IsEarly]
				,[LateMinutes]
				,[EarlyMinutes]
				,[WorkingMinutes]
				,[TotalMinutes]
				,[OverTimeMinutes]
                ,[BreakType])
				Values
				( @AttendanceID
				, @IsShiftOffDay
				, @IsHoliday
				, @IsLeaveDay
				, @IsQuarterDay
				, @IsHalfDay
				, @IsFullDay
				, @Reason
				, @IsApproved
				, @Remarks
				, @ActionBy
				, @ActionDate
				, @IsActive
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
				, @UserIP
				, @IsLate
				, @IsEarly
				, @LateMinutes
				, @EarlyMinutes
				, @WorkingMinutes
				, @TotalMinutes
				, @OverTimeMinutes
                ,@BreakType);
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@AttendanceID",entity.AttendanceId ?? (object)DBNull.Value)
					, new SqlParameter("@IsShiftOffDay",entity.IsShiftOffDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsHoliday",entity.IsHoliday ?? (object)DBNull.Value)
					, new SqlParameter("@IsLeaveDay",entity.IsLeaveDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsQuarterDay",entity.IsQuarterDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsHalfDay",entity.IsHalfDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsFullDay",entity.IsFullDay ?? (object)DBNull.Value)
					, new SqlParameter("@Reason",entity.Reason ?? (object)DBNull.Value)
					, new SqlParameter("@IsApproved",entity.IsApproved ?? (object)DBNull.Value)
					, new SqlParameter("@Remarks",entity.Remarks ?? (object)DBNull.Value)
					, new SqlParameter("@ActionBy",entity.ActionBy ?? (object)DBNull.Value)
					, new SqlParameter("@ActionDate",entity.ActionDate ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsLate",entity.IsLate ?? (object)DBNull.Value)
					, new SqlParameter("@IsEarly",entity.IsEarly ?? (object)DBNull.Value)
					, new SqlParameter("@LateMinutes",entity.LateMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@EarlyMinutes",entity.EarlyMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@WorkingMinutes",entity.WorkingMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@TotalMinutes",entity.TotalMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@OverTimeMinutes",entity.OverTimeMinutes ?? (object)DBNull.Value)
                    , new SqlParameter("@BreakType",entity.BreakType ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetAttendanceStatus(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(AttendanceStatus))]
		public virtual AttendanceStatus UpdateAttendanceStatus(AttendanceStatus entity)
		{

			if (entity.IsTransient()) return entity;
			AttendanceStatus other = GetAttendanceStatus(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [AttendanceStatus] set  [AttendanceID]=@AttendanceID
							, [IsShiftOffDay]=@IsShiftOffDay
							, [IsHoliday]=@IsHoliday
							, [IsLeaveDay]=@IsLeaveDay
							, [IsQuarterDay]=@IsQuarterDay
							, [IsHalfDay]=@IsHalfDay
							, [IsFullDay]=@IsFullDay
							, [Reason]=@Reason
							, [IsApproved]=@IsApproved
							, [Remarks]=@Remarks
							, [ActionBy]=@ActionBy
							, [ActionDate]=@ActionDate
							, [IsActive]=@IsActive
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP
							, [IsLate]=@IsLate
							, [IsEarly]=@IsEarly
							, [LateMinutes]=@LateMinutes
							, [EarlyMinutes]=@EarlyMinutes
							, [WorkingMinutes]=@WorkingMinutes
							, [TotalMinutes]=@TotalMinutes
							, [OverTimeMinutes]=@OverTimeMinutes 
                            , [BreakType]=@BreakType
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@AttendanceID",entity.AttendanceId ?? (object)DBNull.Value)
					, new SqlParameter("@IsShiftOffDay",entity.IsShiftOffDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsHoliday",entity.IsHoliday ?? (object)DBNull.Value)
					, new SqlParameter("@IsLeaveDay",entity.IsLeaveDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsQuarterDay",entity.IsQuarterDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsHalfDay",entity.IsHalfDay ?? (object)DBNull.Value)
					, new SqlParameter("@IsFullDay",entity.IsFullDay ?? (object)DBNull.Value)
					, new SqlParameter("@Reason",entity.Reason ?? (object)DBNull.Value)
					, new SqlParameter("@IsApproved",entity.IsApproved ?? (object)DBNull.Value)
					, new SqlParameter("@Remarks",entity.Remarks ?? (object)DBNull.Value)
					, new SqlParameter("@ActionBy",entity.ActionBy ?? (object)DBNull.Value)
					, new SqlParameter("@ActionDate",entity.ActionDate ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsLate",entity.IsLate ?? (object)DBNull.Value)
					, new SqlParameter("@IsEarly",entity.IsEarly ?? (object)DBNull.Value)
					, new SqlParameter("@LateMinutes",entity.LateMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@EarlyMinutes",entity.EarlyMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@WorkingMinutes",entity.WorkingMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@TotalMinutes",entity.TotalMinutes ?? (object)DBNull.Value)
					, new SqlParameter("@OverTimeMinutes",entity.OverTimeMinutes ?? (object)DBNull.Value)                    
					, new SqlParameter("@BreakType",entity.BreakType ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetAttendanceStatus(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(AttendanceStatus))]
		public virtual AttendanceStatus UpdateAttendanceStatusByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [AttendanceStatus] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetAttendanceStatus(Id);
		}

		public virtual bool DeleteAttendanceStatus(System.Int32 Id)
		{

			string sql="delete from [AttendanceStatus] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(AttendanceStatus))]
		public virtual AttendanceStatus DeleteAttendanceStatus(AttendanceStatus entity)
		{
			this.DeleteAttendanceStatus(entity.Id);
			return entity;
		}


		public virtual AttendanceStatus AttendanceStatusFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			AttendanceStatus entity=new AttendanceStatus();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("AttendanceID"))
			{
			entity.AttendanceId = dr["AttendanceID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["AttendanceID"];
			}
			if (dr.Table.Columns.Contains("IsShiftOffDay"))
			{
			entity.IsShiftOffDay = dr["IsShiftOffDay"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsShiftOffDay"];
			}
			if (dr.Table.Columns.Contains("IsHoliday"))
			{
			entity.IsHoliday = dr["IsHoliday"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsHoliday"];
			}
			if (dr.Table.Columns.Contains("IsLeaveDay"))
			{
			entity.IsLeaveDay = dr["IsLeaveDay"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsLeaveDay"];
			}
			if (dr.Table.Columns.Contains("IsQuarterDay"))
			{
			entity.IsQuarterDay = dr["IsQuarterDay"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsQuarterDay"];
			}
			if (dr.Table.Columns.Contains("IsHalfDay"))
			{
			entity.IsHalfDay = dr["IsHalfDay"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsHalfDay"];
			}
			if (dr.Table.Columns.Contains("IsFullDay"))
			{
			entity.IsFullDay = dr["IsFullDay"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsFullDay"];
			}
			if (dr.Table.Columns.Contains("Reason"))
			{
			entity.Reason = dr["Reason"].ToString();
			}
			if (dr.Table.Columns.Contains("IsApproved"))
			{
			entity.IsApproved = dr["IsApproved"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsApproved"];
			}
			if (dr.Table.Columns.Contains("Remarks"))
			{
			entity.Remarks = dr["Remarks"].ToString();
			}
			if (dr.Table.Columns.Contains("ActionBy"))
			{
			entity.ActionBy = dr["ActionBy"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["ActionBy"];
			}
			if (dr.Table.Columns.Contains("ActionDate"))
			{
			entity.ActionDate = dr["ActionDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["ActionDate"];
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
			if (dr.Table.Columns.Contains("IsLate"))
			{
			entity.IsLate = dr["IsLate"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsLate"];
			}
			if (dr.Table.Columns.Contains("IsEarly"))
			{
			entity.IsEarly = dr["IsEarly"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsEarly"];
			}
			if (dr.Table.Columns.Contains("LateMinutes"))
			{
			entity.LateMinutes = dr["LateMinutes"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["LateMinutes"];
			}
			if (dr.Table.Columns.Contains("EarlyMinutes"))
			{
			entity.EarlyMinutes = dr["EarlyMinutes"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["EarlyMinutes"];
			}
			if (dr.Table.Columns.Contains("WorkingMinutes"))
			{
			entity.WorkingMinutes = dr["WorkingMinutes"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["WorkingMinutes"];
			}
			if (dr.Table.Columns.Contains("TotalMinutes"))
			{
			entity.TotalMinutes = dr["TotalMinutes"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["TotalMinutes"];
			}
			if (dr.Table.Columns.Contains("OverTimeMinutes"))
			{
			entity.OverTimeMinutes = dr["OverTimeMinutes"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["OverTimeMinutes"];
			}
			return entity;
		}

	}
	
	
}
