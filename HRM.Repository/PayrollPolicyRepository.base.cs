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
		
	public abstract partial class PayrollPolicyRepositoryBase : Repository, IPayrollPolicyRepositoryBase 
	{
        
        public PayrollPolicyRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("PayrollVariableID",new SearchColumn(){Name="PayrollVariableID",Title="PayrollVariableID",SelectClause="PayrollVariableID",WhereClause="AllRecords.PayrollVariableID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="PayrollVariableId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsUnit",new SearchColumn(){Name="IsUnit",Title="IsUnit",SelectClause="IsUnit",WhereClause="AllRecords.IsUnit",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsUnit",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsPercentage",new SearchColumn(){Name="IsPercentage",Title="IsPercentage",SelectClause="IsPercentage",WhereClause="AllRecords.IsPercentage",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsPercentage",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Value",new SearchColumn(){Name="Value",Title="Value",SelectClause="Value",WhereClause="AllRecords.Value",DataType="System.Decimal?",IsForeignColumn=false,PropertyName="Value",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Description",new SearchColumn(){Name="Description",Title="Description",SelectClause="Description",WhereClause="AllRecords.Description",DataType="System.String",IsForeignColumn=false,PropertyName="Description",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("EffectiveDate",new SearchColumn(){Name="EffectiveDate",Title="EffectiveDate",SelectClause="EffectiveDate",WhereClause="AllRecords.EffectiveDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="EffectiveDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("RetiredDate",new SearchColumn(){Name="RetiredDate",Title="RetiredDate",SelectClause="RetiredDate",WhereClause="AllRecords.RetiredDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="RetiredDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateBy",new SearchColumn(){Name="UpdateBy",Title="UpdateBy",SelectClause="UpdateBy",WhereClause="AllRecords.UpdateBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdateBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
            //New Fields
            this.SearchColumns.Add("IsAttendance", new SearchColumn() { Name = "IsAttendance", Title = "IsAttendance", SelectClause = "IsAttendance", WhereClause = "AllRecords.IsAttendance", DataType = "System.Boolean?", IsForeignColumn = false, PropertyName = "IsAttendance", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("IsDay", new SearchColumn() { Name = "IsDay", Title = "IsDay", SelectClause = "IsDay", WhereClause = "AllRecords.IsDay", DataType = "System.Boolean?", IsForeignColumn = false, PropertyName = "IsDay", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("SalaryType", new SearchColumn() { Name = "SalaryType", Title = "SalaryType", SelectClause = "SalaryType", WhereClause = "AllRecords.SalaryType", DataType = "System.Int32?", IsForeignColumn = false, PropertyName = "SalaryType", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("Occurance", new SearchColumn() { Name = "Occurance", Title = "Occurance", SelectClause = "Occurance", WhereClause = "AllRecords.Occurance", DataType = "System.Int32?", IsForeignColumn = false, PropertyName = "Occurance", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
        }
        
		public virtual List<SearchColumn> GetPayrollPolicySearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetPayrollPolicyBasicSearchColumns()
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

        public virtual List<SearchColumn> GetPayrollPolicyAdvanceSearchColumns()
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
        
        
        public virtual string GetPayrollPolicySelectClause()
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
                        	selectQuery =  "[PayrollPolicy]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[PayrollPolicy]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<PayrollPolicy> GetPayrollPolicyByPayrollVariableId(System.Int32? PayrollVariableId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPayrollPolicySelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [PayrollPolicy] with (nolock)  where PayrollVariableID=@PayrollVariableID  ";
			SqlParameter parameter=new SqlParameter("@PayrollVariableID",PayrollVariableId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<PayrollPolicy>(ds,PayrollPolicyFromDataRow);
		}

		public virtual PayrollPolicy GetPayrollPolicy(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPayrollPolicySelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [PayrollPolicy] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return PayrollPolicyFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<PayrollPolicy> GetPayrollPolicyByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPayrollPolicySelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [PayrollPolicy] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<PayrollPolicy>(ds,PayrollPolicyFromDataRow);
		}

		public virtual List<PayrollPolicy> GetAllPayrollPolicy(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPayrollPolicySelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [PayrollPolicy] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<PayrollPolicy>(ds, PayrollPolicyFromDataRow);
		}

		public virtual List<PayrollPolicy> GetPagedPayrollPolicy(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetPayrollPolicyCount(whereClause, searchColumns);
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
            tempsql += " FROM [PayrollPolicy] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetPayrollPolicySelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [PayrollPolicy] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [PayrollPolicy].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<PayrollPolicy>(ds, PayrollPolicyFromDataRow);
			}else{ return null;}
		}

		private int GetPayrollPolicyCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM PayrollPolicy as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM PayrollPolicy as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(PayrollPolicy))]
		public virtual PayrollPolicy InsertPayrollPolicy(PayrollPolicy entity)
		{

			PayrollPolicy other=new PayrollPolicy();
			other = entity;
			if(entity.IsTransient())
			{
				string sql= @"Insert into [PayrollPolicy] ( [PayrollVariableID]
				,[IsUnit]
				,[IsPercentage]
                ,[IsDay]
				,[Value]
				,[Description]
				,[EffectiveDate]
				,[RetiredDate]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
				,[UserIP]
                ,[IsAttendance]
                ,[SalaryType]
                ,[Occurance])
				Values
				( @PayrollVariableID
				, @IsUnit
				, @IsPercentage
                , @IsDay
				, @Value
				, @Description
				, @EffectiveDate
				, @RetiredDate
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
				, @UserIP 
                , @IsAttendance
                , @SalaryType
                , @Occurance );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@PayrollVariableID",entity.PayrollVariableId ?? (object)DBNull.Value)
					, new SqlParameter("@IsUnit",entity.IsUnit ?? (object)DBNull.Value)
					, new SqlParameter("@IsPercentage",entity.IsPercentage ?? (object)DBNull.Value)
                        , new SqlParameter("@IsDay",entity.IsDay ?? (object)DBNull.Value)
                    , new SqlParameter("@Value",entity.Value ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@EffectiveDate",entity.EffectiveDate ?? (object)DBNull.Value)
					, new SqlParameter("@RetiredDate",entity.RetiredDate ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
                    , new SqlParameter("@IsAttendance",entity.IsAttendance ?? (object)DBNull.Value)
                    , new SqlParameter("@SalaryType",entity.SalaryType ?? (object)DBNull.Value)
                    , new SqlParameter("@Occurance",entity.Occurance ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetPayrollPolicy(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(PayrollPolicy))]
		public virtual PayrollPolicy UpdatePayrollPolicy(PayrollPolicy entity)
		{

			if (entity.IsTransient()) return entity;
			PayrollPolicy other = GetPayrollPolicy(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql= @"Update [PayrollPolicy] set  [PayrollVariableID]=@PayrollVariableID
							, [IsUnit]=@IsUnit
							, [IsPercentage]=@IsPercentage
							, [Value]=@Value
							, [Description]=@Description
							, [EffectiveDate]=@EffectiveDate
							, [RetiredDate]=@RetiredDate
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP 
, [IsAttendance]=@IsAttendance 
, [SalaryType]=@SalaryType 
, [Occurance]=@Occurance 
, [IsDay]=@IsDay 

							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@PayrollVariableID",entity.PayrollVariableId ?? (object)DBNull.Value)
					, new SqlParameter("@IsUnit",entity.IsUnit ?? (object)DBNull.Value)
					, new SqlParameter("@IsPercentage",entity.IsPercentage ?? (object)DBNull.Value)
					, new SqlParameter("@Value",entity.Value ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@EffectiveDate",entity.EffectiveDate ?? (object)DBNull.Value)
					, new SqlParameter("@RetiredDate",entity.RetiredDate ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
                            , new SqlParameter("@IsAttendance",entity.IsAttendance ?? (object)DBNull.Value)
                    , new SqlParameter("@IsDay",entity.IsDay ?? (object)DBNull.Value)
                    , new SqlParameter("@SalaryType",entity.SalaryType ?? (object)DBNull.Value)
                    , new SqlParameter("@Occurance",entity.Occurance ?? (object)DBNull.Value)
                    , new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetPayrollPolicy(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(PayrollPolicy))]
		public virtual PayrollPolicy UpdatePayrollPolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [PayrollPolicy] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetPayrollPolicy(Id);
		}

		public virtual bool DeletePayrollPolicy(System.Int32 Id)
		{

			string sql="delete from [PayrollPolicy] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(PayrollPolicy))]
		public virtual PayrollPolicy DeletePayrollPolicy(PayrollPolicy entity)
		{
			this.DeletePayrollPolicy(entity.Id);
			return entity;
		}


		public virtual PayrollPolicy PayrollPolicyFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			PayrollPolicy entity=new PayrollPolicy();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("PayrollVariableID"))
			{
			entity.PayrollVariableId = dr["PayrollVariableID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["PayrollVariableID"];
			}
			if (dr.Table.Columns.Contains("IsUnit"))
			{
			entity.IsUnit = dr["IsUnit"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsUnit"];
			}
			if (dr.Table.Columns.Contains("IsPercentage"))
			{
			entity.IsPercentage = dr["IsPercentage"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["IsPercentage"];
			}
			if (dr.Table.Columns.Contains("Value"))
			{
			entity.Value = dr["Value"]==DBNull.Value?(System.Decimal?)null:(System.Decimal?)dr["Value"];
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
            //New
            if (dr.Table.Columns.Contains("IsAttendance"))
            {
                entity.IsAttendance = dr["IsAttendance"] == DBNull.Value ? (System.Boolean?)null : (System.Boolean?)dr["IsAttendance"];
            }
            if (dr.Table.Columns.Contains("IsDay"))
            {
                entity.IsDay = dr["IsDay"] == DBNull.Value ? (System.Boolean?)null : (System.Boolean?)dr["IsDay"];
            }
            if (dr.Table.Columns.Contains("SalaryType"))
            {
                entity.SalaryType = dr["SalaryType"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["SalaryType"];
            }
            if (dr.Table.Columns.Contains("Occurance"))
            {
                entity.Occurance = dr["Occurance"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["Occurance"];
            }
            return entity;
		}

	}
	
	
}
