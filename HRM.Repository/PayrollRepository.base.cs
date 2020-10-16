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
using HRM.Core.Model;

namespace HRM.Repository
{
		
	public abstract partial class PayrollRepositoryBase : Repository, IPayrollRepositoryBase 
	{
        
        public PayrollRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("PayrollCycleID",new SearchColumn(){Name="PayrollCycleID",Title="PayrollCycleID",SelectClause="PayrollCycleID",WhereClause="AllRecords.PayrollCycleID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="PayrollCycleId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserID",new SearchColumn(){Name="UserID",Title="UserID",SelectClause="UserID",WhereClause="AllRecords.UserID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UserId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Salary",new SearchColumn(){Name="Salary",Title="Salary",SelectClause="Salary",WhereClause="AllRecords.Salary",DataType="System.Decimal?",IsForeignColumn=false,PropertyName="Salary",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Addition",new SearchColumn(){Name="Addition",Title="Addition",SelectClause="Addition",WhereClause="AllRecords.Addition",DataType="System.Decimal?",IsForeignColumn=false,PropertyName="Addition",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Deduction",new SearchColumn(){Name="Deduction",Title="Deduction",SelectClause="Deduction",WhereClause="AllRecords.Deduction",DataType="System.Decimal?",IsForeignColumn=false,PropertyName="Deduction",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("NetSalary",new SearchColumn(){Name="NetSalary",Title="NetSalary",SelectClause="NetSalary",WhereClause="AllRecords.NetSalary",DataType="System.Decimal?",IsForeignColumn=false,PropertyName="NetSalary",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateBy",new SearchColumn(){Name="UpdateBy",Title="UpdateBy",SelectClause="UpdateBy",WhereClause="AllRecords.UpdateBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdateBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetPayrollSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetPayrollBasicSearchColumns()
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

        public virtual List<SearchColumn> GetPayrollAdvanceSearchColumns()
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
        
        
        public virtual string GetPayrollSelectClause()
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
                        	selectQuery =  "[Payroll]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[Payroll]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        public virtual List<Payroll> GetPayrollByPayrollCycleId(System.Int32? PayrollCycleId, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetPayrollSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [Payroll] with (nolock)  where PayrollCycleID=@PayrollCycleID  ";
            SqlParameter parameter = new SqlParameter("@PayrollCycleID", PayrollCycleId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<Payroll>(ds, PayrollFromDataRow);
        }

      
        public virtual Payroll GetPayroll(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPayrollSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Payroll] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return PayrollFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<Payroll> GetPayrollByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPayrollSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [Payroll] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Payroll>(ds,PayrollFromDataRow);
		}

		public virtual List<Payroll> GetAllPayroll(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPayrollSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Payroll] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Payroll>(ds, PayrollFromDataRow);
		}

		public virtual List<Payroll> GetPagedPayroll(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetPayrollCount(whereClause, searchColumns);
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
            tempsql += " FROM [Payroll] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetPayrollSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [Payroll] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [Payroll].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Payroll>(ds, PayrollFromDataRow);
			}else{ return null;}
		}

		private int GetPayrollCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM Payroll as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM Payroll as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(Payroll))]
		public virtual Payroll InsertPayroll(Payroll entity)
		{

			Payroll other=new Payroll();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [Payroll] ( [PayrollCycleID]
				,[UserID]
				,[Salary]
				,[Addition]
				,[Deduction]
				,[NetSalary]
				,[IsActive]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
				,[UserIP] )
				Values
				( @PayrollCycleID
				, @UserID
				, @Salary
				, @Addition
				, @Deduction
				, @NetSalary
				, @IsActive
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
				, @UserIP );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@PayrollCycleID",entity.PayrollCycleId ?? (object)DBNull.Value)
					, new SqlParameter("@UserID",entity.UserId ?? (object)DBNull.Value)
					, new SqlParameter("@Salary",entity.Salary ?? (object)DBNull.Value)
					, new SqlParameter("@Addition",entity.Addition ?? (object)DBNull.Value)
					, new SqlParameter("@Deduction",entity.Deduction ?? (object)DBNull.Value)
					, new SqlParameter("@NetSalary",entity.NetSalary ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetPayroll(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(Payroll))]
		public virtual Payroll UpdatePayroll(Payroll entity)
		{

			if (entity.IsTransient()) return entity;
			Payroll other = GetPayroll(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [Payroll] set  [PayrollCycleID]=@PayrollCycleID
							, [UserID]=@UserID
							, [Salary]=@Salary
							, [Addition]=@Addition
							, [Deduction]=@Deduction
							, [NetSalary]=@NetSalary
							, [IsActive]=@IsActive
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@PayrollCycleID",entity.PayrollCycleId ?? (object)DBNull.Value)
					, new SqlParameter("@UserID",entity.UserId ?? (object)DBNull.Value)
					, new SqlParameter("@Salary",entity.Salary ?? (object)DBNull.Value)
					, new SqlParameter("@Addition",entity.Addition ?? (object)DBNull.Value)
					, new SqlParameter("@Deduction",entity.Deduction ?? (object)DBNull.Value)
					, new SqlParameter("@NetSalary",entity.NetSalary ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetPayroll(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(Payroll))]
		public virtual Payroll UpdatePayrollByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [Payroll] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetPayroll(Id);
		}

		public virtual bool DeletePayroll(System.Int32 Id)
		{

			string sql="delete from [Payroll] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(Payroll))]
		public virtual Payroll DeletePayroll(Payroll entity)
		{
			this.DeletePayroll(entity.Id);
			return entity;
		}
       
        public virtual Payroll PayrollFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			Payroll entity=new Payroll();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("PayrollCycleID"))
			{
			entity.PayrollCycleId = dr["PayrollCycleID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["PayrollCycleID"];
			}
			if (dr.Table.Columns.Contains("UserID"))
			{
			entity.UserId = dr["UserID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["UserID"];
			}
			if (dr.Table.Columns.Contains("Salary"))
			{
			entity.Salary = dr["Salary"]==DBNull.Value?(System.Decimal?)null:(System.Decimal?)dr["Salary"];
			}
			if (dr.Table.Columns.Contains("Addition"))
			{
			entity.Addition = dr["Addition"]==DBNull.Value?(System.Decimal?)null:(System.Decimal?)dr["Addition"];
			}
			if (dr.Table.Columns.Contains("Deduction"))
			{
			entity.Deduction = dr["Deduction"]==DBNull.Value?(System.Decimal?)null:(System.Decimal?)dr["Deduction"];
			}
			if (dr.Table.Columns.Contains("NetSalary"))
			{
			entity.NetSalary = dr["NetSalary"]==DBNull.Value?(System.Decimal?)null:(System.Decimal?)dr["NetSalary"];
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
			return entity;
		}



	}
	
	
}
