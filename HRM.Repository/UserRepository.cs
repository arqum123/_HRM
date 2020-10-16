using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using System.Data.SqlClient;
using System.Data;

namespace HRM.Repository
{
    public partial class UserRepository: UserRepositoryBase, IUserRepository
	{
        public User GetUser(string LoginId, string SelectClause = null)
        {

            string sql = string.IsNullOrEmpty(SelectClause) ? GetUserSelectClause() : (string.Format("Select {0} ", SelectClause));
            sql += "from [User] with (nolock)  where LoginID=@LoginID and IsActive = 1";
            SqlParameter parameter = new SqlParameter("@LoginID", LoginId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return UserFromDataRow(ds.Tables[0].Rows[0]);
        }
	}
	
	
}
