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
		
	public abstract partial class AttendancePolicyRepositoryBase : Repository, IAttendancePolicyRepositoryBase 
	{
        
        public AttendancePolicyRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ShiftID",new SearchColumn(){Name="ShiftID",Title="ShiftID",SelectClause="ShiftID",WhereClause="AllRecords.ShiftID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="ShiftId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("AttendanceVariableID",new SearchColumn(){Name="AttendanceVariableID",Title="AttendanceVariableID",SelectClause="AttendanceVariableID",WhereClause="AllRecords.AttendanceVariableID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="AttendanceVariableId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Hours",new SearchColumn(){Name="Hours",Title="Hours",SelectClause="Hours",WhereClause="AllRecords.Hours",DataType="System.decimal?",IsForeignColumn=false,PropertyName="Hours",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Description",new SearchColumn(){Name="Description",Title="Description",SelectClause="Description",WhereClause="AllRecords.Description",DataType="System.String",IsForeignColumn=false,PropertyName="Description",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("EffectiveDate",new SearchColumn(){Name="EffectiveDate",Title="EffectiveDate",SelectClause="EffectiveDate",WhereClause="AllRecords.EffectiveDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="EffectiveDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("RetiredDate",new SearchColumn(){Name="RetiredDate",Title="RetiredDate",SelectClause="RetiredDate",WhereClause="AllRecords.RetiredDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="RetiredDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateBy",new SearchColumn(){Name="UpdateBy",Title="UpdateBy",SelectClause="UpdateBy",WhereClause="AllRecords.UpdateBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdateBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetAttendancePolicySearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetAttendancePolicyBasicSearchColumns()
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

        public virtual List<SearchColumn> GetAttendancePolicyAdvanceSearchColumns()
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
        
        
        public virtual string GetAttendancePolicySelectClause()
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
                        	selectQuery =  "[AttendancePolicy]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[AttendancePolicy]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<AttendancePolicy> GetAttendancePolicyByShiftId(System.Int32? ShiftId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendancePolicySelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [AttendancePolicy] with (nolock)  where ShiftID=@ShiftID  ";
			SqlParameter parameter=new SqlParameter("@ShiftID",ShiftId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendancePolicy>(ds,AttendancePolicyFromDataRow);
		}

		public virtual List<AttendancePolicy> GetAttendancePolicyByAttendanceVariableId(System.Int32? AttendanceVariableId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendancePolicySelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [AttendancePolicy] with (nolock)  where AttendanceVariableID=@AttendanceVariableID  ";
			SqlParameter parameter=new SqlParameter("@AttendanceVariableID",AttendanceVariableId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendancePolicy>(ds,AttendancePolicyFromDataRow);
		}

		public virtual AttendancePolicy GetAttendancePolicy(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendancePolicySelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [AttendancePolicy] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return AttendancePolicyFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<AttendancePolicy> GetAttendancePolicyByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendancePolicySelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [AttendancePolicy] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendancePolicy>(ds,AttendancePolicyFromDataRow);
		}

		public virtual List<AttendancePolicy> GetAllAttendancePolicy(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetAttendancePolicySelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [AttendancePolicy] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendancePolicy>(ds, AttendancePolicyFromDataRow);
		}

		public virtual List<AttendancePolicy> GetPagedAttendancePolicy(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetAttendancePolicyCount(whereClause, searchColumns);
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
            tempsql += " FROM [AttendancePolicy] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetAttendancePolicySelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [AttendancePolicy] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [AttendancePolicy].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<AttendancePolicy>(ds, AttendancePolicyFromDataRow);
			}else{ return null;}
		}

		private int GetAttendancePolicyCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM AttendancePolicy as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM AttendancePolicy as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(AttendancePolicy))]
		public virtual AttendancePolicy InsertAttendancePolicy(AttendancePolicy entity)
		{

			AttendancePolicy other=new AttendancePolicy();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [AttendancePolicy] ( [ShiftID]
				,[AttendanceVariableID]
				,[Hours]
				,[Description]
				,[EffectiveDate]
				,[RetiredDate]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
				,[UserIP] )
				Values
				( @ShiftID
				, @AttendanceVariableID
				, @Hours
				, @Description
				, @EffectiveDate
				, @RetiredDate
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
				, @UserIP );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@ShiftID",entity.ShiftId ?? (object)DBNull.Value)
					, new SqlParameter("@AttendanceVariableID",entity.AttendanceVariableId ?? (object)DBNull.Value)
					, new SqlParameter("@Hours",entity.Hours ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@EffectiveDate",entity.EffectiveDate ?? (object)DBNull.Value)
					, new SqlParameter("@RetiredDate",entity.RetiredDate ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetAttendancePolicy(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(AttendancePolicy))]
		public virtual AttendancePolicy UpdateAttendancePolicy(AttendancePolicy entity)
		{

			if (entity.IsTransient()) return entity;
			AttendancePolicy other = GetAttendancePolicy(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [AttendancePolicy] set  [ShiftID]=@ShiftID
							, [AttendanceVariableID]=@AttendanceVariableID
							, [Hours]=@Hours
							, [Description]=@Description
							, [EffectiveDate]=@EffectiveDate
							, [RetiredDate]=@RetiredDate
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@ShiftID",entity.ShiftId ?? (object)DBNull.Value)
					, new SqlParameter("@AttendanceVariableID",entity.AttendanceVariableId ?? (object)DBNull.Value)
					, new SqlParameter("@Hours",entity.Hours ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@EffectiveDate",entity.EffectiveDate ?? (object)DBNull.Value)
					, new SqlParameter("@RetiredDate",entity.RetiredDate ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetAttendancePolicy(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(AttendancePolicy))]
		public virtual AttendancePolicy UpdateAttendancePolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [AttendancePolicy] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetAttendancePolicy(Id);
		}

		public virtual bool DeleteAttendancePolicy(System.Int32 Id)
		{

			string sql="delete from [AttendancePolicy] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(AttendancePolicy))]
		public virtual AttendancePolicy DeleteAttendancePolicy(AttendancePolicy entity)
		{
			this.DeleteAttendancePolicy(entity.Id);
			return entity;
		}


		public virtual AttendancePolicy AttendancePolicyFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			AttendancePolicy entity=new AttendancePolicy();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("ShiftID"))
			{
			entity.ShiftId = dr["ShiftID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["ShiftID"];
			}
			if (dr.Table.Columns.Contains("AttendanceVariableID"))
			{
			entity.AttendanceVariableId = dr["AttendanceVariableID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["AttendanceVariableID"];
			}
			if (dr.Table.Columns.Contains("Hours"))
			{
			entity.Hours = dr["Hours"]==DBNull.Value?(decimal?)null:(decimal?)dr["Hours"];
			}
			if (dr.Table.Columns.Contains("Description"))
			{
			entity.Description = dr["Description"].ToString();
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
