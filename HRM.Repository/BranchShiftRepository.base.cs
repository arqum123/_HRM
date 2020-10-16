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
		
	public abstract partial class BranchShiftRepositoryBase : Repository, IBranchShiftRepositoryBase 
	{
        
        public BranchShiftRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("BranchID",new SearchColumn(){Name="BranchID",Title="BranchID",SelectClause="BranchID",WhereClause="AllRecords.BranchID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="BranchId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ShiftID",new SearchColumn(){Name="ShiftID",Title="ShiftID",SelectClause="ShiftID",WhereClause="AllRecords.ShiftID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="ShiftId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreatedDate",new SearchColumn(){Name="CreatedDate",Title="CreatedDate",SelectClause="CreatedDate",WhereClause="AllRecords.CreatedDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreatedDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdatedDate",new SearchColumn(){Name="UpdatedDate",Title="UpdatedDate",SelectClause="UpdatedDate",WhereClause="AllRecords.UpdatedDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdatedDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdatedBy",new SearchColumn(){Name="UpdatedBy",Title="UpdatedBy",SelectClause="UpdatedBy",WhereClause="AllRecords.UpdatedBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdatedBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetBranchShiftSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetBranchShiftBasicSearchColumns()
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

        public virtual List<SearchColumn> GetBranchShiftAdvanceSearchColumns()
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
        
        
        public virtual string GetBranchShiftSelectClause()
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
                        	selectQuery =  "[BranchShift]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[BranchShift]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<BranchShift> GetBranchShiftByBranchId(System.Int32? BranchId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchShiftSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [BranchShift] with (nolock)  where BranchID=@BranchID  ";
			SqlParameter parameter=new SqlParameter("@BranchID",BranchId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchShift>(ds,BranchShiftFromDataRow);
		}

		public virtual List<BranchShift> GetBranchShiftByShiftId(System.Int32? ShiftId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchShiftSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [BranchShift] with (nolock)  where ShiftID=@ShiftID  ";
			SqlParameter parameter=new SqlParameter("@ShiftID",ShiftId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchShift>(ds,BranchShiftFromDataRow);
		}

		public virtual BranchShift GetBranchShift(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchShiftSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [BranchShift] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return BranchShiftFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<BranchShift> GetBranchShiftByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchShiftSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [BranchShift] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchShift>(ds,BranchShiftFromDataRow);
		}

		public virtual List<BranchShift> GetAllBranchShift(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetBranchShiftSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [BranchShift] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchShift>(ds, BranchShiftFromDataRow);
		}

		public virtual List<BranchShift> GetPagedBranchShift(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetBranchShiftCount(whereClause, searchColumns);
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
            tempsql += " FROM [BranchShift] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetBranchShiftSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [BranchShift] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [BranchShift].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<BranchShift>(ds, BranchShiftFromDataRow);
			}else{ return null;}
		}

		private int GetBranchShiftCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM BranchShift as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM BranchShift as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(BranchShift))]
		public virtual BranchShift InsertBranchShift(BranchShift entity)
		{

			BranchShift other=new BranchShift();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [BranchShift] ( [BranchID]
				,[ShiftID]
				,[CreatedDate]
				,[UpdatedDate]
				,[UpdatedBy]
				,[UserIP]
				,[IsActive] )
				Values
				( @BranchID
				, @ShiftID
				, @CreatedDate
				, @UpdatedDate
				, @UpdatedBy
				, @UserIP
				, @IsActive );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@BranchID",entity.BranchId ?? (object)DBNull.Value)
					, new SqlParameter("@ShiftID",entity.ShiftId ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedDate",entity.UpdatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetBranchShift(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(BranchShift))]
		public virtual BranchShift UpdateBranchShift(BranchShift entity)
		{

			if (entity.IsTransient()) return entity;
			BranchShift other = GetBranchShift(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [BranchShift] set  [BranchID]=@BranchID
							, [ShiftID]=@ShiftID
							, [UpdatedDate]=@UpdatedDate
							, [UpdatedBy]=@UpdatedBy
							, [UserIP]=@UserIP
							, [IsActive]=@IsActive 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@BranchID",entity.BranchId ?? (object)DBNull.Value)
					, new SqlParameter("@ShiftID",entity.ShiftId ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedDate",entity.UpdatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetBranchShift(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(BranchShift))]
		public virtual BranchShift UpdateBranchShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [BranchShift] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetBranchShift(Id);
		}

		public virtual bool DeleteBranchShift(System.Int32 Id)
		{

			string sql="delete from [BranchShift] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(BranchShift))]
		public virtual BranchShift DeleteBranchShift(BranchShift entity)
		{
			this.DeleteBranchShift(entity.Id);
			return entity;
		}


		public virtual BranchShift BranchShiftFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			BranchShift entity=new BranchShift();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("BranchID"))
			{
			entity.BranchId = dr["BranchID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["BranchID"];
			}
			if (dr.Table.Columns.Contains("ShiftID"))
			{
			entity.ShiftId = dr["ShiftID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["ShiftID"];
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
