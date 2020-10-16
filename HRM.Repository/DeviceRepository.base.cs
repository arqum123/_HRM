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
		
	public abstract partial class DeviceRepositoryBase : Repository, IDeviceRepositoryBase 
	{
        
        public DeviceRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("ID",new SearchColumn(){Name="ID",Title="ID",SelectClause="ID",WhereClause="AllRecords.ID",DataType="System.Int32",IsForeignColumn=false,PropertyName="Id",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("MachineID",new SearchColumn(){Name="MachineID",Title="MachineID",SelectClause="MachineID",WhereClause="AllRecords.MachineID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="MachineId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DeviceID",new SearchColumn(){Name="DeviceID",Title="DeviceID",SelectClause="DeviceID",WhereClause="AllRecords.DeviceID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="DeviceId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ConnectionTypeID",new SearchColumn(){Name="ConnectionTypeID",Title="ConnectionTypeID",SelectClause="ConnectionTypeID",WhereClause="AllRecords.ConnectionTypeID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="ConnectionTypeId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DeviceModalID",new SearchColumn(){Name="DeviceModalID",Title="DeviceModalID",SelectClause="DeviceModalID",WhereClause="AllRecords.DeviceModalID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="DeviceModalId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IPAddress",new SearchColumn(){Name="IPAddress",Title="IPAddress",SelectClause="IPAddress",WhereClause="AllRecords.IPAddress",DataType="System.String",IsForeignColumn=false,PropertyName="IpAddress",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("PortNumber",new SearchColumn(){Name="PortNumber",Title="PortNumber",SelectClause="PortNumber",WhereClause="AllRecords.PortNumber",DataType="System.Int32?",IsForeignColumn=false,PropertyName="PortNumber",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Password",new SearchColumn(){Name="Password",Title="Password",SelectClause="Password",WhereClause="AllRecords.Password",DataType="System.String",IsForeignColumn=false,PropertyName="Password",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ComPort",new SearchColumn(){Name="ComPort",Title="ComPort",SelectClause="ComPort",WhereClause="AllRecords.ComPort",DataType="System.Int32?",IsForeignColumn=false,PropertyName="ComPort",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Baudrate",new SearchColumn(){Name="Baudrate",Title="Baudrate",SelectClause="Baudrate",WhereClause="AllRecords.Baudrate",DataType="System.Int64?",IsForeignColumn=false,PropertyName="Baudrate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Status",new SearchColumn(){Name="Status",Title="Status",SelectClause="Status",WhereClause="AllRecords.Status",DataType="System.String",IsForeignColumn=false,PropertyName="Status",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("StatusDescription",new SearchColumn(){Name="StatusDescription",Title="StatusDescription",SelectClause="StatusDescription",WhereClause="AllRecords.StatusDescription",DataType="System.String",IsForeignColumn=false,PropertyName="StatusDescription",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("BranchID",new SearchColumn(){Name="BranchID",Title="BranchID",SelectClause="BranchID",WhereClause="AllRecords.BranchID",DataType="System.Int32?",IsForeignColumn=false,PropertyName="BranchId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsActive",new SearchColumn(){Name="IsActive",Title="IsActive",SelectClause="IsActive",WhereClause="AllRecords.IsActive",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="IsActive",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateDate",new SearchColumn(){Name="UpdateDate",Title="UpdateDate",SelectClause="UpdateDate",WhereClause="AllRecords.UpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="UpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UpdateBy",new SearchColumn(){Name="UpdateBy",Title="UpdateBy",SelectClause="UpdateBy",WhereClause="AllRecords.UpdateBy",DataType="System.Int32?",IsForeignColumn=false,PropertyName="UpdateBy",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("UserIP",new SearchColumn(){Name="UserIP",Title="UserIP",SelectClause="UserIP",WhereClause="AllRecords.UserIP",DataType="System.String",IsForeignColumn=false,PropertyName="UserIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetDeviceSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetDeviceBasicSearchColumns()
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

        public virtual List<SearchColumn> GetDeviceAdvanceSearchColumns()
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
        
        
        public virtual string GetDeviceSelectClause()
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
                        	selectQuery =  "[Device]."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",[Device]."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<Device> GetDeviceByDeviceModalId(System.Int32? DeviceModalId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [Device] with (nolock)  where DeviceModalID=@DeviceModalID  ";
			SqlParameter parameter=new SqlParameter("@DeviceModalID",DeviceModalId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Device>(ds,DeviceFromDataRow);
		}

		public virtual List<Device> GetDeviceByBranchId(System.Int32? BranchId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceSelectClause():(string.Format("Select [{0}] ",SelectClause));
			sql+="from [Device] with (nolock)  where BranchID=@BranchID  ";
			SqlParameter parameter=new SqlParameter("@BranchID",BranchId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Device>(ds,DeviceFromDataRow);
		}

		public virtual Device GetDevice(System.Int32 Id,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Device] with (nolock)  where ID=@ID ";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return DeviceFromDataRow(ds.Tables[0].Rows[0]);
		}

		public virtual List<Device> GetDeviceByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [Device] with (nolock)  where {0} {1} {2} ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Device>(ds,DeviceFromDataRow);
		}

		public virtual List<Device> GetAllDevice(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDeviceSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [Device] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Device>(ds, DeviceFromDataRow);
		}

		public virtual List<Device> GetPagedDevice(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetDeviceCount(whereClause, searchColumns);
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
            tempsql += " FROM [Device] AllRecords with (NOLOCK)";
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
                (string.IsNullOrEmpty(SelectClause)? GetDeviceSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [Device] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [Device].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<Device>(ds, DeviceFromDataRow);
			}else{ return null;}
		}

		private int GetDeviceCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM Device as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM Device as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(Device))]
		public virtual Device InsertDevice(Device entity)
		{

			Device other=new Device();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into [Device] ( [MachineID]
				,[DeviceID]
				,[ConnectionTypeID]
				,[DeviceModalID]
				,[IPAddress]
				,[PortNumber]
				,[Password]
				,[ComPort]
				,[Baudrate]
				,[Status]
				,[StatusDescription]
				,[BranchID]
				,[IsActive]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
				,[UserIP] )
				Values
				( @MachineID
				, @DeviceID
				, @ConnectionTypeID
				, @DeviceModalID
				, @IPAddress
				, @PortNumber
				, @Password
				, @ComPort
				, @Baudrate
				, @Status
				, @StatusDescription
				, @BranchID
				, @IsActive
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
				, @UserIP );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@MachineID",entity.MachineId ?? (object)DBNull.Value)
					, new SqlParameter("@DeviceID",entity.DeviceId ?? (object)DBNull.Value)
					, new SqlParameter("@ConnectionTypeID",entity.ConnectionTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@DeviceModalID",entity.DeviceModalId ?? (object)DBNull.Value)
					, new SqlParameter("@IPAddress",entity.IpAddress ?? (object)DBNull.Value)
					, new SqlParameter("@PortNumber",entity.PortNumber ?? (object)DBNull.Value)
					, new SqlParameter("@Password",entity.Password ?? (object)DBNull.Value)
					, new SqlParameter("@ComPort",entity.ComPort ?? (object)DBNull.Value)
					, new SqlParameter("@Baudrate",entity.Baudrate ?? (object)DBNull.Value)
					, new SqlParameter("@Status",entity.Status ?? (object)DBNull.Value)
					, new SqlParameter("@StatusDescription",entity.StatusDescription ?? (object)DBNull.Value)
					, new SqlParameter("@BranchID",entity.BranchId ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetDevice(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(Device))]
		public virtual Device UpdateDevice(Device entity)
		{

			if (entity.IsTransient()) return entity;
			Device other = GetDevice(entity.Id);
			if (entity.Equals(other)) return entity;
			string sql=@"Update [Device] set  [MachineID]=@MachineID
							, [DeviceID]=@DeviceID
							, [ConnectionTypeID]=@ConnectionTypeID
							, [DeviceModalID]=@DeviceModalID
							, [IPAddress]=@IPAddress
							, [PortNumber]=@PortNumber
							, [Password]=@Password
							, [ComPort]=@ComPort
							, [Baudrate]=@Baudrate
							, [Status]=@Status
							, [StatusDescription]=@StatusDescription
							, [BranchID]=@BranchID
							, [IsActive]=@IsActive
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP 
							 where ID=@ID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@MachineID",entity.MachineId ?? (object)DBNull.Value)
					, new SqlParameter("@DeviceID",entity.DeviceId ?? (object)DBNull.Value)
					, new SqlParameter("@ConnectionTypeID",entity.ConnectionTypeId ?? (object)DBNull.Value)
					, new SqlParameter("@DeviceModalID",entity.DeviceModalId ?? (object)DBNull.Value)
					, new SqlParameter("@IPAddress",entity.IpAddress ?? (object)DBNull.Value)
					, new SqlParameter("@PortNumber",entity.PortNumber ?? (object)DBNull.Value)
					, new SqlParameter("@Password",entity.Password ?? (object)DBNull.Value)
					, new SqlParameter("@ComPort",entity.ComPort ?? (object)DBNull.Value)
					, new SqlParameter("@Baudrate",entity.Baudrate ?? (object)DBNull.Value)
					, new SqlParameter("@Status",entity.Status ?? (object)DBNull.Value)
					, new SqlParameter("@StatusDescription",entity.StatusDescription ?? (object)DBNull.Value)
					, new SqlParameter("@BranchID",entity.BranchId ?? (object)DBNull.Value)
					, new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
					, new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
					, new SqlParameter("@ID",entity.Id)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetDevice(entity.Id);
		}

		[MOLog(AuditOperations.Update,typeof(Device))]
		public virtual Device UpdateDeviceByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{

			List<SqlParameter> parameterArray = new List<SqlParameter>();
			string sql="Update [Device] set ";
			foreach (var item in UpdateKeyValue)
			{

			sql += "["+ item.Key + "]=@" + item.Key + ",";
			parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
			}

			sql = sql.Trim(',');
			sql +=" where Id=@Id";
			parameterArray.Add(new SqlParameter("@Id", Id));
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray.ToArray());
			return GetDevice(Id);
		}

		public virtual bool DeleteDevice(System.Int32 Id)
		{

			string sql="delete from [Device] where ID=@ID";
			SqlParameter parameter=new SqlParameter("@ID",Id);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(Device))]
		public virtual Device DeleteDevice(Device entity)
		{
			this.DeleteDevice(entity.Id);
			return entity;
		}


		public virtual Device DeviceFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			Device entity=new Device();
			if (dr.Table.Columns.Contains("ID"))
			{
			entity.Id = (System.Int32)dr["ID"];
			}
			if (dr.Table.Columns.Contains("MachineID"))
			{
			entity.MachineId = dr["MachineID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["MachineID"];
			}
			if (dr.Table.Columns.Contains("DeviceID"))
			{
			entity.DeviceId = dr["DeviceID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["DeviceID"];
			}
			if (dr.Table.Columns.Contains("ConnectionTypeID"))
			{
			entity.ConnectionTypeId = dr["ConnectionTypeID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["ConnectionTypeID"];
			}
			if (dr.Table.Columns.Contains("DeviceModalID"))
			{
			entity.DeviceModalId = dr["DeviceModalID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["DeviceModalID"];
			}
			if (dr.Table.Columns.Contains("IPAddress"))
			{
			entity.IpAddress = dr["IPAddress"].ToString();
			}
			if (dr.Table.Columns.Contains("PortNumber"))
			{
			entity.PortNumber = dr["PortNumber"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["PortNumber"];
			}
			if (dr.Table.Columns.Contains("Password"))
			{
			entity.Password = dr["Password"].ToString();
			}
			if (dr.Table.Columns.Contains("ComPort"))
			{
			entity.ComPort = dr["ComPort"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["ComPort"];
			}
			if (dr.Table.Columns.Contains("Baudrate"))
			{
			entity.Baudrate = dr["Baudrate"]==DBNull.Value?(System.Int64?)null:(System.Int64?)dr["Baudrate"];
			}
			if (dr.Table.Columns.Contains("Status"))
			{
			entity.Status = dr["Status"].ToString();
			}
			if (dr.Table.Columns.Contains("StatusDescription"))
			{
			entity.StatusDescription = dr["StatusDescription"].ToString();
			}
			if (dr.Table.Columns.Contains("BranchID"))
			{
			entity.BranchId = dr["BranchID"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["BranchID"];
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
