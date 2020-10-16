using HRM.Core;
using HRM.Core.DataInterfaces;
using HRM.Core.Entities;
using HRM.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRM.Repository
{
    public abstract partial class CustomPayrollDetailRepositoryBase : Repository, ICustomPayrollDetailRepositoryBase
    {

        public CustomPayrollDetailRepositoryBase()
        {
            this.SearchColumns = new Dictionary<string, SearchColumn>();

            this.SearchColumns.Add("ID", new SearchColumn() { Name = "ID", Title = "ID", SelectClause = "ID", WhereClause = "AllRecords.ID", DataType = "System.Int32", IsForeignColumn = false, PropertyName = "Id", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("PayrollID", new SearchColumn() { Name = "PayrollID", Title = "PayrollID", SelectClause = "PayrollID", WhereClause = "AllRecords.PayrollID", DataType = "System.Int32?", IsForeignColumn = false, PropertyName = "PayrollId", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("PayrollPolicyID", new SearchColumn() { Name = "PayrollPolicyID", Title = "PayrollPolicyID", SelectClause = "PayrollPolicyID", WhereClause = "AllRecords.PayrollPolicyID", DataType = "System.Int32?", IsForeignColumn = false, PropertyName = "PayrollPolicyId", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("Amount", new SearchColumn() { Name = "Amount", Title = "Amount", SelectClause = "Amount", WhereClause = "AllRecords.Amount", DataType = "System.Decimal?", IsForeignColumn = false, PropertyName = "Amount", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("IsActive", new SearchColumn() { Name = "IsActive", Title = "IsActive", SelectClause = "IsActive", WhereClause = "AllRecords.IsActive", DataType = "System.Boolean?", IsForeignColumn = false, PropertyName = "IsActive", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("CreationDate", new SearchColumn() { Name = "CreationDate", Title = "CreationDate", SelectClause = "CreationDate", WhereClause = "AllRecords.CreationDate", DataType = "System.DateTime?", IsForeignColumn = false, PropertyName = "CreationDate", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("UpdateDate", new SearchColumn() { Name = "UpdateDate", Title = "UpdateDate", SelectClause = "UpdateDate", WhereClause = "AllRecords.UpdateDate", DataType = "System.DateTime?", IsForeignColumn = false, PropertyName = "UpdateDate", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("UpdateBy", new SearchColumn() { Name = "UpdateBy", Title = "UpdateBy", SelectClause = "UpdateBy", WhereClause = "AllRecords.UpdateBy", DataType = "System.Int32?", IsForeignColumn = false, PropertyName = "UpdateBy", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("UserIP", new SearchColumn() { Name = "UserIP", Title = "UserIP", SelectClause = "UserIP", WhereClause = "AllRecords.UserIP", DataType = "System.String", IsForeignColumn = false, PropertyName = "UserIp", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
            this.SearchColumns.Add("PayrollPolicyName", new SearchColumn() { Name = "PayrollPolicyName", Title = "PayrollPolicyName", SelectClause = "PayrollPolicyName", WhereClause = "AllRecords.PayrollPolicyName", DataType = "System.String", IsForeignColumn = false, PropertyName = "PayrollPolicyName", IsAdvanceSearchColumn = false, IsBasicSearchColumm = false });
        }

        public virtual List<SearchColumn> GetCustomPayrollDetailSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }



        public virtual Dictionary<string, string> GetCustomPayrollDetailBasicSearchColumns()
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

        public virtual List<SearchColumn> GetCustomPayrollDetailAdvanceSearchColumns()
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


        public virtual string GetCustomPayrollDetailSelectClause()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            string selectQuery = string.Empty;
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                if (!keyValuePair.Value.IgnoreForDefaultSelect)
                {
                    if (keyValuePair.Value.IsForeignColumn)
                    {
                        if (string.IsNullOrEmpty(selectQuery))
                        {
                            selectQuery = "(" + keyValuePair.Value.SelectClause + ") as \"" + keyValuePair.Key + "\"";
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
                            selectQuery = "[CustomPayrollDetail]." + keyValuePair.Key;
                        }
                        else
                        {
                            selectQuery += ",[CustomPayrollDetail]." + keyValuePair.Key;
                        }
                    }
                }
            }
            return "Select " + selectQuery + " ";
        }

        public virtual List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollId(System.Int32? PayrollId, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [CustomPayrollDetail] with (nolock)  where PayrollID=@PayrollID  ";
            SqlParameter parameter = new SqlParameter("@PayrollID", PayrollId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<CustomPayrollDetail>(ds, CustomPayrollDetailFromDataRow);
        }

        public virtual List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollPolicyId(System.Int32? PayrollPolicyId, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select [{0}] ", SelectClause));
            sql += "from [CustomPayrollDetail] with (nolock)  where PayrollPolicyID=@PayrollPolicyID  ";
            SqlParameter parameter = new SqlParameter("@PayrollPolicyID", PayrollPolicyId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<CustomPayrollDetail>(ds, CustomPayrollDetailFromDataRow);
        }

        public virtual CustomPayrollDetail CustomPayrollDetail(System.Int32 Id, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += "from [CustomPayrollDetail] with (nolock)  where ID=@ID ";
            SqlParameter parameter = new SqlParameter("@ID", Id);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return CustomPayrollDetailFromDataRow(ds.Tables[0].Rows[0]);
        }

        public virtual List<CustomPayrollDetail> GetCustomPayrollDetailByKeyValue(string Key, string Value, Operands operand, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += string.Format("from [CustomPayrollDetail] with (nolock)  where {0} {1} {2} ", Key, operand.ToOperandString(), Value);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<CustomPayrollDetail>(ds, CustomPayrollDetailFromDataRow);
        }
        public virtual CustomPayrollDetail GetCustomPayrollDetail(System.Int32 Id, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += "from [CustomPayrollDetail] with (nolock)  where ID=@ID ";
            SqlParameter parameter = new SqlParameter("@ID", Id);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return CustomPayrollDetailFromDataRow(ds.Tables[0].Rows[0]);
        }


        public virtual List<CustomPayrollDetail> GetAllCustomPayrollDetail(string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += "from [CustomPayrollDetail] with (nolock)  ";
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, null);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<CustomPayrollDetail>(ds, CustomPayrollDetailFromDataRow);
        }

        public virtual List<CustomPayrollDetail> GetPagedCustomPayrollDetail(string orderByClause, int pageSize, int startIndex, out int count, List<SearchColumn> searchColumns, string SelectClause = null)
        {

            string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
            if (!String.IsNullOrEmpty(orderByClause))
            {
                KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
            }

            count = GetCustomPayrollDetailCount(whereClause, searchColumns);
            if (count > 0)
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
                tempsql += " FROM [CustomPayrollDetail] AllRecords with (NOLOCK)";
                if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
                if (orderByClause.Length > 0)
                {
                    tempsql += " ORDER BY " + orderByClause;
                    if (!orderByClause.Contains("ID"))
                        tempsql += " , (AllRecords.[ID])";
                }
                else
                {
                    tempsql += " ORDER BY (AllRecords.[ID])";
                }

                // Return paged results
                string pagedResultsSql =
                    (string.IsNullOrEmpty(SelectClause) ? GetCustomPayrollDetailSelectClause() : (string.Format("Select {0} ", SelectClause))) + @" FROM [CustomPayrollDetail] , #PageIndex PageIndex WHERE ";
                pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString();
                pagedResultsSql += @" AND [CustomPayrollDetail].[ID] = PageIndex.[ID] 
				                  ORDER BY PageIndex.IndexId;";
                pagedResultsSql += " drop table #PageIndex";
                sql = sql + tempsql + pagedResultsSql;
                sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
                DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
                if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
                return CollectionFromDataSet<CustomPayrollDetail>(ds, CustomPayrollDetailFromDataRow);
            }
            else { return null; }
        }

        private int GetCustomPayrollDetailCount(string whereClause, List<SearchColumn> searchColumns)
        {

            string sql = string.Empty;
            if (string.IsNullOrEmpty(whereClause))
                sql = "SELECT Count(*) FROM CustomPayrollDetail as AllRecords  ";
            else
                sql = "SELECT Count(*) FROM CustomPayrollDetail as AllRecords  where  " + whereClause;
            var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
            return rowCount == DBNull.Value ? 0 : (int)rowCount;
        }
        [MOLog(AuditOperations.Create, typeof(CustomPayrollDetail))]
        public virtual CustomPayrollDetail InsertCustomPayrollDetail(CustomPayrollDetail entity)
        {

            CustomPayrollDetail other = new CustomPayrollDetail();
            other = entity;
            if (entity.IsTransient())
            {
                string sql = @"Insert into [CustomPayrollDetail] ( [PayrollID]
				,[PayrollPolicyID]
				,[Amount]
				,[IsActive]
				,[CreationDate]
				,[UpdateDate]
				,[UpdateBy]
,[PayrollPolicyName]
				,[UserIP] )
				Values
				( @PayrollID
				, @PayrollPolicyID
				, @Amount
				, @IsActive
				, @CreationDate
				, @UpdateDate
				, @UpdateBy
, @PayrollPolicyName
				, @UserIP );
				Select scope_identity()";
                SqlParameter[] parameterArray = new SqlParameter[]{
                     new SqlParameter("@PayrollID",entity.PayrollId ?? (object)DBNull.Value)
                    , new SqlParameter("@PayrollPolicyID",entity.PayrollPolicyId ?? (object)DBNull.Value)
                    , new SqlParameter("@Amount",entity.Amount ?? (object)DBNull.Value)
                    , new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
                    , new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
                    , new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
                    , new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
                     , new SqlParameter("@PayrollPolicyName",entity.PayrollPolicyName ?? (object)DBNull.Value)
                    , new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)};
                var identity = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, parameterArray);
                if (identity == DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
                return GetCustomPayrollDetail(Convert.ToInt32(identity));
            }
            return entity;
        }


        [MOLog(AuditOperations.Update, typeof(CustomPayrollDetail))]
        public virtual CustomPayrollDetail UpdateCustomPayrollDetail(CustomPayrollDetail entity)
        {

            if (entity.IsTransient()) return entity;
            CustomPayrollDetail other = GetCustomPayrollDetail(entity.Id);
            if (entity.Equals(other)) return entity;
            string sql = @"Update [CustomPayrollDetail] set  [PayrollID]=@PayrollID
							, [PayrollPolicyID]=@PayrollPolicyID
							, [Amount]=@Amount
							, [IsActive]=@IsActive
							, [CreationDate]=@CreationDate
							, [UpdateDate]=@UpdateDate
							, [UpdateBy]=@UpdateBy
							, [UserIP]=@UserIP 
, [PayrollPolicyName]=@PayrollPolicyName 
							 where ID=@ID";
            SqlParameter[] parameterArray = new SqlParameter[]{
                     new SqlParameter("@PayrollID",entity.PayrollId ?? (object)DBNull.Value)
                                        , new SqlParameter("@PayrollPolicyID",entity.PayrollPolicyId ?? (object)DBNull.Value)
                    , new SqlParameter("@Amount",entity.Amount ?? (object)DBNull.Value)
                    , new SqlParameter("@IsActive",entity.IsActive ?? (object)DBNull.Value)
                    , new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
                    , new SqlParameter("@UpdateDate",entity.UpdateDate ?? (object)DBNull.Value)
                    , new SqlParameter("@UpdateBy",entity.UpdateBy ?? (object)DBNull.Value)
                    , new SqlParameter("@UserIP",entity.UserIp ?? (object)DBNull.Value)
                    , new SqlParameter("@PayrollPolicyName",entity.PayrollPolicyName ?? (object)DBNull.Value)
                    , new SqlParameter("@ID",entity.Id)};
            SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, parameterArray);
            return GetCustomPayrollDetail(entity.Id);
        }
        [MOLog(AuditOperations.Update, typeof(CustomPayrollDetail))]
        public virtual CustomPayrollDetail UpdateCustomPayrollDetailByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
        {

            List<SqlParameter> parameterArray = new List<SqlParameter>();
            string sql = "Update [CustomPayrollDetail] set ";
            foreach (var item in UpdateKeyValue)
            {

                sql += "[" + item.Key + "]=@" + item.Key + ",";
                parameterArray.Add(new SqlParameter("@" + item.Key, item.Value));
            }

            sql = sql.Trim(',');
            sql += " where Id=@Id";
            parameterArray.Add(new SqlParameter("@Id", Id));
            SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, parameterArray.ToArray());
            return GetCustomPayrollDetail(Id);
        }

        public virtual bool DeleteCustomPayrollDetail(System.Int32 Id)
        {

            string sql = "delete from [CustomPayrollDetail] where ID=@ID";
            SqlParameter parameter = new SqlParameter("@ID", Id);
            var identity = SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            return (Convert.ToInt32(identity)) == 1 ? true : false;
        }

        [MOLog(AuditOperations.Delete, typeof(CustomPayrollDetail))]
        public virtual CustomPayrollDetail DeleteCustomPayrollDetail(CustomPayrollDetail entity)
        {
            this.DeleteCustomPayrollDetail(entity.Id);
            return entity;
        }


        public virtual CustomPayrollDetail CustomPayrollDetailFromDataRow(DataRow dr)
        {
            if (dr == null) return null;
            CustomPayrollDetail entity = new CustomPayrollDetail();
            if (dr.Table.Columns.Contains("ID"))
            {
                entity.Id = (System.Int32)dr["ID"];
            }
            if (dr.Table.Columns.Contains("PayrollID"))
            {
                entity.PayrollId = dr["PayrollID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollID"];
            }
            if (dr.Table.Columns.Contains("PayrollPolicyID"))
            {
                entity.PayrollPolicyId = dr["PayrollPolicyId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollPolicyId"];
            }
            if (dr.Table.Columns.Contains("Amount"))
            {
                entity.Amount = dr["Amount"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Amount"];
            }
            if (dr.Table.Columns.Contains("IsActive"))
            {
                entity.IsActive = dr["IsActive"] == DBNull.Value ? (System.Boolean?)null : (System.Boolean?)dr["IsActive"];
            }
            if (dr.Table.Columns.Contains("CreationDate"))
            {
                entity.CreationDate = dr["CreationDate"] == DBNull.Value ? (System.DateTime?)null : (System.DateTime?)dr["CreationDate"];
            }
            if (dr.Table.Columns.Contains("UpdateDate"))
            {
                entity.UpdateDate = dr["UpdateDate"] == DBNull.Value ? (System.DateTime?)null : (System.DateTime?)dr["UpdateDate"];
            }
            if (dr.Table.Columns.Contains("UpdateBy"))
            {
                entity.UpdateBy = dr["UpdateBy"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["UpdateBy"];
            }
            if (dr.Table.Columns.Contains("UserIP"))
            {
                entity.UserIp = dr["UserIP"].ToString();
            }
            if (dr.Table.Columns.Contains("PayrollPolicyName"))
            {
                entity.PayrollPolicyName = dr["PayrollPolicyName"].ToString();
            }
            return entity;

        }

    }
}
