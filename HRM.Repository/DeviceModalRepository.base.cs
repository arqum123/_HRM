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
		
	public abstract partial class DeviceModalRepositoryBase : Repository, IDeviceModalRepositoryBase 
	{
        
        public DeviceModalRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();
			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DeviceModal",new SearchColumn(){Name="DeviceModal",Title="DeviceModal",SelectClause="DeviceModal",WhereClause="AllRecords.DeviceModal",DataType="System.String",IsForeignColumn=false,PropertyName="DeviceModal",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreatedDate",new SearchColumn(){Name="CreatedDate",Title="CreatedDate",SelectClause="CreatedDate",WhereClause="AllRecords.CreatedDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreatedDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdationDate",new SearchColumn(){Name="UpdationDate",Title="UpdationDate",SelectClause="UpdationDate",WhereClause="AllRecords.UpdationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdatedBy",new SearchColumn(){Name="UpdatedBy",Title="UpdatedBy",SelectClause="UpdatedBy",WhereClause="AllRecords.UpdatedBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdatedBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }   
		public virtual List<SearchColumn> GetDeviceModalSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }	
        public virtual Dictionary<string, string> GetDeviceModalBasicSearchColumns()
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

        public virtual List<SearchColumn> GetDeviceModalAdvanceSearchColumns()
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
        
        
        public virtual string GetDeviceModalSelectClause()
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
                        	selectQuery =  "[DeviceModal]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[DeviceModal]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual DeviceModal GetDeviceModal(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceModalSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [DeviceModal] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return DeviceModalFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<DeviceModal> GetDeviceModalByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceModalSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [DeviceModal] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<DeviceModal>(ds,DeviceModalFromDataRow);
		}

		public virtual List<DeviceModal> GetAllDeviceModal(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceModalSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [DeviceModal] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<DeviceModal>(ds, DeviceModalFromDataRow);
		}

		public virtual List<DeviceModal> GetPagedDeviceModal(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetDeviceModalCount(whereClause, searchColumns);
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
            tempsql += " FROM [DeviceModal] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetDeviceModalSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [DeviceModal] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [DeviceModal].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<DeviceModal>(ds, DeviceModalFromDataRow);
			}else{ return null;}
		}

		private int GetDeviceModalCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM DeviceModal as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM DeviceModal as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(DeviceModal))]
		public virtual DeviceModal InsertDeviceModal(DeviceModal entity)
		{

			DeviceModal other=new DeviceModal();
			other = entity;
				string sql=@"Insert into [DeviceModal] ( [ID]
				,[DeviceModal]
				,[CreatedDate]
				,[UpdationDate]
				,[UpdatedBy]
				,[UserIP]
				,[IsActive] )
				Values
				( @ID
				, @DeviceModal
				, @CreatedDate
				, @UpdationDate
				, @UpdatedBy
				, @UserIP
				, @IsActive );
";
				SqlParameter[] parameterArray=new SqlParameter[]{
					new SqlParameter("@ID",entity.Id)
					, new SqlParameter("@DeviceModal",entity.DeviceModal ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdationDate",entity.UpdationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetDeviceModal(Convert.ToInt32(identity));
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(DeviceModal))]
		public virtual DeviceModal UpdateDeviceModal(DeviceModal entity)
		{

			DeviceModal other = GetDeviceModal(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [DeviceModal] set  [DeviceModal]=@DeviceModal
							, [UpdationDate]=@UpdationDate
							, [UpdatedBy]=@UpdatedBy
							, [UserIP]=@UserIP
							, [IsActive]=@IsActive 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					new SqlParameter("@ID",entity.Id)
					, new SqlParameter("@DeviceModal",entity.DeviceModal ?? (object)DBNull.Value)
					, new SqlParameter("@CreatedDate",entity.CreatedDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdationDate",entity.UpdationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdatedBy",entity.UpdatedBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetDeviceModal(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(DeviceModal))]
		public virtual DeviceModal UpdateDeviceModalByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [DeviceModal] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetDeviceModal(Id);
		}

		public virtual bool DeleteDeviceModal(System.Int32 Id)
		{

			string sql="delete from [DeviceModal] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(DeviceModal))]
		public virtual DeviceModal DeleteDeviceModal(DeviceModal entity)
		{
			this.DeleteDeviceModal(entity.Id);
			return entity;
		}


		public virtual DeviceModal DeviceModalFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			DeviceModal entity=new DeviceModal();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("DeviceModal"))
			{
			entity.DeviceModal = dr["DeviceModal"].ToString();
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
