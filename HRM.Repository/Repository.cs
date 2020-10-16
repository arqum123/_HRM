
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Configuration;
using HRM.Core.Entities;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


namespace HRM.Repository
{
		
	 public class Repository
    {
        public Dictionary<string, SearchColumn> SearchColumns { get; set; }
        public virtual List<SearchColumn> GetSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
				keyValuePair.Value.Value = string.Empty;
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
        protected delegate T TFromDataRow<T>(DataRow dr);
        protected static List<T> CollectionFromDataSet<T>(DataSet ds, TFromDataRow<T> action)
        {
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0)
                return null;

            List<T> list = new List<T>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(action(dr));
            }

            return list;
        }


        private string[] _numericTypes = { "System.Decimal", "System.Float", "System.Double", "System.Decimal?", "System.Float?", "System.Double?" };
        private string[] _stringTypes = { "System.String", "System.String?" };
        private string[] _intTypes = { "System.Int32", "System.Int16", "System.Int64", "System.Int32?", "System.Int16?", "System.Int64?" };
        private string[] _boolTypes = { "System.Boolean", "System.Boolean?" };
        private string[] _dateTypes = { "System.DateTime", "System.DateTime?" };

        public virtual string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["conn"].ToString();
            }
        }

        protected string GetBasicSearchWhereClauseByColumn(string columnName, string columnValue, Dictionary<string, SearchColumn> searchColumns)
        {
            string whereClause = string.Empty;
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in searchColumns)
            {
                if (keyValuePair.Value.IsBasicSearchColumm)
                {
                    if (keyValuePair.Key == columnName)
                    {
                        if (string.IsNullOrEmpty(whereClause))
                        {
                            whereClause = "(" + keyValuePair.Value.WhereClause + ") LIKE '%" + columnValue + "%' ";
                        }
                    }
                    else if (columnName == "All")
                    {
                        if (string.IsNullOrEmpty(whereClause))
                        {
                            whereClause += "(" + keyValuePair.Value.WhereClause + ") LIKE '%" + columnValue + "%' ";
                        }
                        else
                        {
                            whereClause += " OR (" + keyValuePair.Value.WhereClause + ") LIKE '%" + columnValue + "%' ";
                        }
                    }
                }
            }
            return whereClause;
        }

        protected KeyValuePair<string, string> ParseWhereClause(string whereClause)
        {
            string keyName = String.Empty;
            string keyValue = String.Empty;

            string[] splitWhereClause = whereClause.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitWhereClause.Length; i++)
            {
                if (i == 0)
                {
                    keyName = splitWhereClause[i];
                }
                else
                {
                    if (String.IsNullOrEmpty(keyValue))
                    {
                        keyValue = splitWhereClause[i];
                    }
                    else
                    {
                        keyValue += ":" + splitWhereClause[i];
                    }
                }
            }
            KeyValuePair<string, string> parsedWhereClause = new KeyValuePair<string, string>(keyName, keyValue);
            return parsedWhereClause;
        }

        protected KeyValuePair<string, string> ParseOrderByClause(string orderByClause)
        {
            string keyName = String.Empty;
            string keyValue = String.Empty;

            string[] splitOrderByClause = orderByClause.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitOrderByClause.Length; i++)
            {
                if (i == 0)
                {
                    keyName = splitOrderByClause[i];
                }
                else
                {
                    if (String.IsNullOrEmpty(keyValue))
                    {
                        keyValue = splitOrderByClause[i];
                    }
                    else
                    {
                        keyValue += ":" + splitOrderByClause[i];
                    }
                }
            }
            KeyValuePair<string, string> parsedOrderByClause = new KeyValuePair<string, string>(keyName, keyValue);
            return parsedOrderByClause;
        }

        protected string GetBasicSearchOrderByClauseByColumn(string columnName, string sort, Dictionary<string, SearchColumn> searchColumns)
        {
            string orderByClause = string.Empty;
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in searchColumns)
            {
                //if (keyValuePair.Value.IsBasicSearchColumm)
                //{
                if (keyValuePair.Key.ToLower().Equals(columnName.ToLower()))
                {
                    if (string.IsNullOrEmpty(orderByClause))
                    {
                        orderByClause = "(" + keyValuePair.Value.WhereClause + ") " + sort;
                        break;
                    }
                }
                //}
            }
            return orderByClause;
        }

        public string GetAdvancedWhereClauseByColumn(List<SearchColumn> searchCriterias, List<SearchColumn> searchColumns)
        {
            return GetAdvanceWhereClause(searchCriterias, searchColumns, false);
        }

        public string GetAdvancedWhereClauseByColumn(List<SearchColumn> searchCriterias, List<SearchColumn> searchColumns, bool getAllRecords)
        {
            if (searchCriterias != null && searchCriterias.Count > 0)
                return GetAdvanceWhereClause(searchCriterias, searchColumns, getAllRecords);
            else
            {

                return string.Empty;
            }
        }

        private string GetAdvanceWhereClause(List<SearchColumn> searchCriterias, List<SearchColumn> searchColumns, bool getAllRecords)
        {

            string dataSecurityClause = string.Empty;
            string dataSecurityCriteria = string.Empty;
            string whereClause = string.Empty;
            searchColumns = this.GetSearchColumns();
            int index = 0;
            foreach (SearchColumn criteriaObject in searchCriterias)
            {
                index++;
                if (!String.IsNullOrEmpty(criteriaObject.Value))
                {
                    SearchColumn searchedColumn = GetSearchColumnByName(criteriaObject, searchColumns);
                    if (searchedColumn == null)
                        return string.Empty;
                    string searchColumnParameterName = searchedColumn.Name.Replace("-", "") + index;
                    criteriaObject.SQLParamaterName = searchColumnParameterName;
                    string searchValue;
                    string[] splittedValue;
                    string searchDateWhereClause;
                    string searchWithORAndCriteria = criteriaObject.Criteria;
                    //Creating Integer Types where clause
                    if (_intTypes.Contains(searchedColumn.DataType) &&
                        _intTypes.Contains(searchedColumn.DataType.Replace("?", "")))
                    {
                        if (searchedColumn.Operand.ToLower() == "in")
                        {
                            dataSecurityCriteria = searchWithORAndCriteria;
                            if (criteriaObject.IsIncludeNULLValue)
                                dataSecurityClause = " ((" + searchedColumn.WhereClause + ") " +
                                                     searchedColumn.Operand + " (select id from dbo.split(@" + searchColumnParameterName + ",',')) OR " +
                                                     searchedColumn.WhereClause + " IS NULL)";
                            else
                                dataSecurityClause = " (" + searchedColumn.WhereClause + ") " +
                                                     searchedColumn.Operand + " (select id from dbo.split(@" + searchColumnParameterName + ",','))";
                        }
                        else
                        {
                            if (searchedColumn.Operand.ToLower() == "between")
                            {
                                splittedValue = searchedColumn.Value.Split(',');
                                searchValue = " @" + searchColumnParameterName + "1 And @" + searchColumnParameterName + "2) "; //making parameterize @columnname1 AND @columnname2
                            }
                            else
                            {
                                searchValue = " @" + searchColumnParameterName + ") ";

                            }
                            if (string.IsNullOrEmpty(whereClause))
                            {
                                whereClause = "((" + searchedColumn.WhereClause + " )" + searchedColumn.Operand +
                                              " " +
                                              searchValue;
                            }
                            else
                            {
                                whereClause += " " + searchWithORAndCriteria + " ((" + searchedColumn.WhereClause +
                                               ") " + searchedColumn.Operand + " " +
                                               searchValue;
                            }
                        }
                    }

                        //Creating Numeric Types where clause
                    else if (_numericTypes.Contains(searchedColumn.DataType) &&
                             _intTypes.Contains(searchedColumn.DataType.Replace("?", "")))
                    {
                        if (searchedColumn.Operand.ToLower() == "between")
                        {
                            splittedValue = searchedColumn.Value.Split(',');
                            searchValue = " @" + searchColumnParameterName + "1 And @" + searchColumnParameterName + "2) "; //making parameterize @columnname1 AND @columnname2
                        }
                        else
                        {
                            searchValue = " @" + searchColumnParameterName + ") ";

                        }

                        if (string.IsNullOrEmpty(whereClause))
                        {
                            whereClause = "((" + searchedColumn.WhereClause + ") " + searchedColumn.Operand + " " +
                                          searchValue;
                        }
                        else
                        {
                            whereClause += " " + searchWithORAndCriteria + " ((" + searchedColumn.WhereClause + ") " +
                                           searchedColumn.Operand + " " +
                                           searchValue;
                        }
                    }

                        //Creating String Types where clause
                    else if (_stringTypes.Contains(searchedColumn.DataType) &&
                             _stringTypes.Contains(searchedColumn.DataType.Replace("?", "")))
                    {
                        if (string.IsNullOrEmpty(whereClause))
                        {
                            whereClause = "((" + searchedColumn.WhereClause + " ) " + searchedColumn.Operand;

                            if (searchedColumn.Operand.ToLower() == "like" && !criteriaObject.IsStartsWithSearching)
                                whereClause += " '%' + @" + searchColumnParameterName + @" + '%'  ESCAPE '\') ";//appending ESCAPE '\' for like query with special character ([,% etc);
                            else if (searchedColumn.Operand.ToLower() == "like" && criteriaObject.IsStartsWithSearching)
                                whereClause += " @" + searchColumnParameterName + @" + '%'  ESCAPE '\') ";
                            else //if operand is =
                                whereClause += " @" + searchColumnParameterName + ") ";
                        }
                        else
                        {
                            whereClause += " " + searchWithORAndCriteria + " ((" + searchedColumn.WhereClause + ") " +
                                           searchedColumn.Operand;

                            if (searchedColumn.Operand.ToLower() == "like" && !criteriaObject.IsStartsWithSearching)
                                whereClause += " '%' + @" + searchColumnParameterName + @"+ '%' ESCAPE '\') ";//appending ESCAPE '\' for like query with special character ([,% etc);
                            else if (searchedColumn.Operand.ToLower() == "like" && criteriaObject.IsStartsWithSearching)
                                whereClause += " @" + searchColumnParameterName + @" + '%' ESCAPE '\') ";
                            else //if operand is =
                                whereClause += " @" + searchColumnParameterName + ") ";
                        }
                    }

                        //Creating Bool Types where clause
                    else if (_boolTypes.Contains(searchedColumn.DataType) &&
                             _boolTypes.Contains(searchedColumn.DataType.Replace("?", "")))
                    {
                        if (string.IsNullOrEmpty(whereClause))
                        {
                            whereClause = " ((" + searchedColumn.WhereClause + ") = @" + searchColumnParameterName + ") ";
                        }
                        else
                        {
                            whereClause += " " + searchWithORAndCriteria + " ((" + searchedColumn.WhereClause + ") = @" + searchColumnParameterName + ") ";
                        }
                    }

                        //Creating DateTime Types where clause
                    else if (_dateTypes.Contains(searchedColumn.DataType) &&
                             _dateTypes.Contains(searchedColumn.DataType.Replace("?", "")))
                    {
                        if (searchedColumn.Operand.ToLower() == "between")
                        {
                            splittedValue = searchedColumn.Value.Split(',');
                            searchValue = "CONVERT(DATETIME,@" + searchColumnParameterName + "1,101) And CONVERT(DATETIME,@" + searchColumnParameterName + "2,101))";
                        }
                        else
                        {
                            searchValue = "CONVERT(DATETIME,@" + searchColumnParameterName + ",101)) ";
                        }
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchDateWhereClause = "CONVERT(DATETIME," + searchedColumn.WhereClause + ",101)";
                            if (string.IsNullOrEmpty(whereClause))
                            {


                                whereClause = "((" + searchDateWhereClause + ") " + searchedColumn.Operand + " " +
                                              searchValue;
                            }
                            else
                            {
                                whereClause += " " + searchWithORAndCriteria + " ((" + searchDateWhereClause + " )" +
                                               searchedColumn.Operand + " " +
                                               searchValue;
                            }
                        }
                    }

                }
            }
            if (!getAllRecords && searchCriterias.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Equals("isdeleted")).Count() == 0
                && searchColumns.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Equals("isdeleted")).Count() == 1)
            {
                whereClause = string.IsNullOrEmpty(whereClause) ? " (AllRecords.IsDeleted )= @IsDeleted " : "((" + whereClause + ") AND (AllRecords.IsDeleted )= @IsDeleted) ";
                SearchColumn sColumn = GetIsDeleteColumnToSearch();
                sColumn.SQLParamaterName = "IsDeleted";
                searchCriterias.Add(sColumn);
            }
            else
            {
                whereClause = !string.IsNullOrEmpty(whereClause) ? "(" + whereClause + ")" : string.Empty;
            }
            if (!string.IsNullOrEmpty(dataSecurityCriteria)
                && !string.IsNullOrEmpty(dataSecurityClause))
            {
                whereClause = !string.IsNullOrEmpty(whereClause) ? "(" + whereClause + " " + dataSecurityCriteria + " " + dataSecurityClause + ")" : "(" + dataSecurityClause + ")";
            }
            return whereClause;
        }

        public string GetUpdateClauseByColumn(List<SearchColumn> searchCriterias, List<SearchColumn> searchColumns, string tableName, string primaryKey, out  SqlParameter[] parameterArray)
        {
            int parameterLength = GetParameterCount(searchCriterias);
            parameterArray = new SqlParameter[parameterLength + 1];
            string updateClause = string.Empty;
            string tableColumnUpdateClause = string.Empty;
            int i = 0;
            bool isLastModifiedDateColumnExists = false;
            if (searchColumns != null && searchColumns.Count > 0 && searchColumns.Exists(x => x.Name == "LastModifiedDate"))
                isLastModifiedDateColumnExists = true;

            foreach (SearchColumn criteriaObject in searchCriterias)
            {
                //if (!String.IsNullOrEmpty(criteriaObject.Value) && IsValueValid(criteriaObject.Value))
                if (!String.IsNullOrEmpty(criteriaObject.Value) || criteriaObject.UpdateColumnIfValueIsEmpty)
                {
                    SearchColumn searchedColumn = GetSearchColumnByName(criteriaObject, searchColumns);
                    if (!String.IsNullOrEmpty(criteriaObject.Value))
                    {
                        string[] ValueArray = searchedColumn.Value.Split('|');
                        if (searchedColumn.ParameterCount > 1 && ValueArray.Length == searchedColumn.ParameterCount)
                        {
                            for (int j = 0; j < searchedColumn.ParameterCount; j++)
                            {
                                string searchName = searchedColumn.Name;
                                if (j > 0)
                                    searchName = searchedColumn.Name + "" + j;

                                parameterArray[i] = new SqlParameter("@" + searchName, ValueArray[j]);
                                if (j != searchedColumn.ParameterCount - 1)
                                    i++;
                            }
                        }
                        else
                        {
                            parameterArray[i] = new SqlParameter("@" + searchedColumn.Name, searchedColumn.Value);
                        }
                    }
                    else
                    {
                        parameterArray[i] = new SqlParameter("@" + searchedColumn.Name, DBNull.Value);
                    }
                    if (criteriaObject.IsForeignColumn)
                    {
                        if (!string.IsNullOrEmpty(updateClause))
                        {
                            updateClause += ";" + searchedColumn.BulkUpdateClause;
                        }
                        else
                        {
                            updateClause = searchedColumn.BulkUpdateClause;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(tableColumnUpdateClause))
                        {
                            tableColumnUpdateClause += " ," + searchedColumn.Name + "=@" + searchedColumn.Name;
                        }
                        else
                        {
                            tableColumnUpdateClause = searchedColumn.Name + "=@" + searchedColumn.Name;
                        }

                    }
                }
                i++;
            }
            if (isLastModifiedDateColumnExists)
            {
                if (!string.IsNullOrEmpty(tableColumnUpdateClause))
                {
                    tableColumnUpdateClause += " ,LastModifiedDate='" + DateTime.UtcNow + "'";
                }
                else
                {
                    tableColumnUpdateClause = "LastModifiedDate='" + DateTime.UtcNow + "'";
                }
            }
            if (!string.IsNullOrEmpty(tableColumnUpdateClause))
            {
                string updateQuery = "Update " + tableName + " set " + tableColumnUpdateClause + " where " + primaryKey + " in (select id from Split(@IdList,','))";
                if (!string.IsNullOrEmpty(updateClause))
                    updateClause += ";" + updateQuery;
                else
                    updateClause = updateQuery;
            }

            return updateClause;
        }

        private int GetParameterCount(List<SearchColumn> searchCriterias)
        {
            int count = 0;
            foreach (SearchColumn criteriaObject in searchCriterias)
            {
                count += criteriaObject.ParameterCount;
            }
            return count;
        }


        public string GetGroupWhereCaluse(List<SearchColumn> searchCriterias, List<SearchColumn> searchColumns)
        {
            List<SearchColumn> columnsToSearch = searchCriterias.Where(x => x.GroupId == 0).ToList();
            List<SearchColumn> columnsToGroupBy = searchCriterias.Where(x => x.GroupId > 0).ToList();
            string whereClause = GetAdvancedWhereClauseByColumn(columnsToSearch, searchColumns, false);
            string groupWhereClause = string.Empty;
            ILookup<int, SearchColumn> searchColumn = columnsToGroupBy.ToLookup(x => x.GroupId, x => x);

            foreach (IGrouping<int, SearchColumn> column in searchColumn)
            {
                groupWhereClause =
                    GetAdvancedWhereClauseByColumn(searchCriterias.Where(x => x.GroupId == (int)column.Key).ToList(),
                                                         searchColumns);
                if (!string.IsNullOrEmpty(groupWhereClause))
                {
                    if (!string.IsNullOrEmpty(whereClause))
                        whereClause += column.First().GroupCriteria + "(" + groupWhereClause + ")";
                    else
                        whereClause = groupWhereClause;
                }
                else
                    whereClause = groupWhereClause;
            }
            searchCriterias.Clear();
            searchCriterias.AddRange(columnsToSearch);
            searchCriterias.AddRange(columnsToGroupBy);
            return whereClause;
        }



        protected SearchColumn GetSearchColumnByName(SearchColumn criteriaObj, List<SearchColumn> searchColumns)
        {
            SearchColumn searchColumnObject = null;
            foreach (SearchColumn column in searchColumns)
            {
                if (criteriaObj.Name.ToLower() == column.Name.ToLower())
                {
                    searchColumnObject = column;
                    searchColumnObject.Operand = criteriaObj.Operand;
                    searchColumnObject.Value = criteriaObj.Value;
                    searchColumnObject.ParameterCount = criteriaObj.ParameterCount;
                }
            }
            return searchColumnObject;
        }

        public bool IsValueValid(string value)
        {
            Match match = Regex.Match(value, @"^[a-zA-Z0-9\-\ /@,_!$#.&:+';]*$", RegexOptions.IgnoreCase);
            return match.Success;
        }
        public static string ReplaceEscapeChars(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("'", "''");
            }
            return str;
        }
        public string CreateWhereClauseFromIDList(List<int> ListOfIds)
        {
            string whereClause = "(";
            foreach (int id in ListOfIds)
            {
                whereClause += id.ToString() + ",";
            }
            if (whereClause.Length > 1)
                whereClause = whereClause.Remove(whereClause.Length - 1);
            whereClause += ")";
            return whereClause;
        }

        public string GetCommaDelimitedStringFromArray<T>(T[] arr)
        {
            if (arr != null && arr.Length > 0)
            {
                string commaDelmitedValue = string.Empty;
                decimal id;
                if (decimal.TryParse(arr[0].ToString(), out id))
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == 0) commaDelmitedValue = arr[i].ToString();
                        else commaDelmitedValue += "," + arr[i].ToString();
                    }
                }
                else
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == 0) commaDelmitedValue = "'" + arr[i].ToString() + "'";
                        else commaDelmitedValue += ",'" + arr[i].ToString() + "'";
                    }
                }
                return commaDelmitedValue;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetCommaDelimitedStringFromList<T>(List<T> list)
        {
            return GetCommaDelimitedStringFromArray(list.ToArray());
        }

        public SqlParameter[] GetSQLParamtersBySearchColumns(List<SearchColumn> searchColumns)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (searchColumns != null)
            {
                string[] splittedValues = null;
                foreach (SearchColumn searchColumn in searchColumns)
                {
                    string searchColumnParameterName = searchColumn.SQLParamaterName;
                    if (searchColumn.Operand.ToLower().Equals("between"))
                        splittedValues = searchColumn.Value.Split(',');
                    else
                        splittedValues = null;

                    if (_boolTypes.Contains(searchColumn.DataType)
                        && _boolTypes.Contains(searchColumn.DataType.Replace("?", "")))
                    {
                        //If bool type
                        string searchValueString = searchColumn.Value.ToLower();
                        bool searchValue = (searchValueString.Equals("true") || searchValueString.Equals("1")) ? true : false;
                        sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName, searchValue));
                    }
                    else if (_dateTypes.Contains(searchColumn.DataType)
                        && _dateTypes.Contains(searchColumn.DataType.Replace("?", ""))
                        && searchColumn.Operand.ToLower() != "in")
                    {
                        //If date type & operand is not "IN" because in case of IN value would be comma delimated cant parse to datetime
                        if (splittedValues != null)
                        {
                            sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName + "1", DateTime.Parse(splittedValues[0])));
                            sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName + "2", DateTime.Parse(splittedValues[1])));
                        }
                        else
                            sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName, DateTime.Parse(searchColumn.Value)));
                    }
                    else
                    {
                        //If numeric & string type                        
                        if (splittedValues != null)
                        {
                            sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName + "1", splittedValues[0]));
                            sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName + "2", splittedValues[1]));
                        }
                        else
                        {
                            if (_stringTypes.Contains(searchColumn.DataType)
                                && _stringTypes.Contains(searchColumn.DataType.Replace("?", ""))
                                && searchColumn.Operand.ToLower() == "like")
                            {
                                //Appending '\' for wildcard characters because parametrized clause contains ESCAPE '\'
                                string modifiedValueForWildCardCharacters = searchColumn.Value;
                                modifiedValueForWildCardCharacters = modifiedValueForWildCardCharacters.Replace("[", @"\[").Replace("%", @"\%");
                                sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName, modifiedValueForWildCardCharacters));
                            }
                            else
                                sqlParameters.Add(new SqlParameter("@" + searchColumnParameterName, searchColumn.Value));
                        }

                    }

                }
            }
            return (sqlParameters != null && sqlParameters.Count != 0) ? sqlParameters.ToArray() : null;
        }

        private SearchColumn GetIsDeleteColumnToSearch()
        {
            SearchColumn isDeleted = new SearchColumn() { Name = "IsDeleted", Title = "IsDeleted", SelectClause = "IsDeleted", WhereClause = "AllRecords.IsDeleted", DataType = "System.Boolean", IsForeignColumn = false, IsAdvanceSearchColumn = false, IsBasicSearchColumm = false };
            isDeleted.Value = "False";
            isDeleted.Operand = "=";
            isDeleted.Criteria = "AND";
            return isDeleted;
        }

    }
}
