using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using System.Data.SqlClient;
using System.Data;
using HRM.Core.Model;

namespace HRM.Repository
{
		
	public partial class PayrollRepository: PayrollRepositoryBase, IPayrollRepository
	{
        public void RunPayroll(int PayrollCycleID, int IsFinal)
        {
            string sql = "GeneratePayroll";
            SqlParameter[] parameterArray = new SqlParameter[]{
					 new SqlParameter("@PayrollCycleID",PayrollCycleID)
					, new SqlParameter("@IsFinal",IsFinal)};

            SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.StoredProcedure, sql, parameterArray);
        }

        public virtual List<VMModifyPayrollEdit> GetModifyPayrollByPayrollCycleId(System.Int32? PayrollCycleId, System.Int32? DepartmentId, System.String UserName, string SelectClause = null)
        {
            string sql = "";
            sql += " SELECT P.ID AS PayrollId,P.Salary,P.Addition,P.Deduction,P.NetSalary, D.ID AS DepartmentId,D.Name AS DepartmentName, U.ID AS UserId,U.FirstName AS UserName,U.MiddleName,U.LastName,U.Designation,U.NICNo, S.ID AS SalaryTypeId, S.Name AS SalaryTypeName, PC.ID AS PayrollCycleId FROM [Payroll] P INNER JOIN[User] U ON P.UserID = U.ID INNER JOIN PayrollCycle PC ON P.PayrollCycleID = PC.ID INNER JOIN SalaryType S ON U.SalaryTypeID = S.ID INNER JOIN UserDepartment UD on U.ID = UD.UserId INNER JOIN[Department] d ON UD.DepartmentID = D.Id  Where @PayrollCycleId IS NULL OR  PC.ID=@PayrollCycleId AND(@DepartmentId IS NULL OR  UD.DepartmentID = @DepartmentId)  ORDER BY P.ID ";

            SqlParameter[] parameterArray = new SqlParameter[]
            {
                new SqlParameter("@PayrollCycleId", PayrollCycleId) ,
                new SqlParameter("@DepartmentId", DepartmentId) 
                //new SqlParameter("@UserName", UserName)
            };
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, parameterArray);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<VMModifyPayrollEdit>(ds, VMModifyPayrollEditFromDataRow);
        }

        public virtual List<VMModifyPayrollEdit> GetModifyPayslipEdit(System.Int32? PayrollId, System.Int32? DepartmentId, System.Int32? UserId)
        {

            string sql = "";
            sql += " SELECT P.ID AS PayrollId,P.Salary,P.Addition,P.Deduction,P.NetSalary, PD.ID AS PayrollDetailId,PD.PayrollPolicyName AS PayrollDetailName, PV.ID AS PayrollVariableId,PV.Name AS PayrollVariableName, D.ID AS DepartmentId,D.Name AS DepartmentName, U.ID AS UserId,U.FirstName AS UserFName,U.MiddleName AS UserMName,U.LastName AS UserLName,U.Designation,U.NICNo, S.ID AS SalaryTypeId, S.Name AS SalaryTypeName, PC.ID AS PayrollCycleId , PC.Name AS PayrollCycleName,PC.Month AS PayrollCycleMonth,PC.Year AS PayrollCycleYear FROM [Payroll] P  INNER JOIN [User] U ON P.UserID = U.ID INNER JOIN PayrollCycle PC ON P.PayrollCycleID = PC.ID INNER JOIN PayrollDetail PD on P.ID=PD.PayrollID INNER JOIN PayrollPolicy PP on PD.PayrollPolicyID=PP.ID INNER JOIN PayrollVariable PV on PP.PayrollVariableID=PV.ID INNER JOIN SalaryType S ON U.SalaryTypeID = S.ID  INNER JOIN UserDepartment UD on U.ID=UD.UserId  INNER JOIN [Department] d ON UD.DepartmentID=D.Id Where  (@DepartmentId IS NULL OR  UD.DepartmentID=@DepartmentId) AND (@UserId IS NULL OR  P.UserID=@UserId) AND (@PayrollId IS NULL OR  P.Id=@PayrollId) ORDER BY P.ID  ";
            SqlParameter[] parameterArray = new SqlParameter[]
            {

                new SqlParameter("@PayrollId", PayrollId) ,
                new SqlParameter("@DepartmentId", DepartmentId),
                new SqlParameter("@UserId", UserId)
            };

            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, parameterArray);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<VMModifyPayrollEdit>(ds, VMModifyPayrollSlipEditFromDataRow);
        }

        //New
        public virtual VMModifyPayrollEdit VMModifyPayrollSlipEditFromDataRow(DataRow dr)
        {
            if (dr == null) return null;
            VMModifyPayrollEdit entity = new VMModifyPayrollEdit();
            //if (dr.Table.Columns.Contains("ID"))
            //{
            //    entity.Id = (System.Int32)dr["ID"];
            //}
            if (dr.Table.Columns.Contains("PayrollCycleID"))
            {
                entity.PayrollCycleId = dr["PayrollCycleID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollCycleID"];
            }
            if (dr.Table.Columns.Contains("PayrollDetailId"))
            {
                entity.PayrollDetailId = dr["PayrollDetailId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollDetailId"];
            }
            if (dr.Table.Columns.Contains("PayrollVariableId"))
            {
                entity.PayrollVariableId = dr["PayrollVariableId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollVariableId"];
            }


            if (dr.Table.Columns.Contains("PayrollId"))
            {
                entity.PayrollId = dr["PayrollId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollId"];
            }
            if (dr.Table.Columns.Contains("UserID"))
            {
                entity.UserId = dr["UserID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["UserID"];
            }
            if (dr.Table.Columns.Contains("SalaryTypeId"))
            {
                entity.SalaryTypeId = dr["SalaryTypeId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["SalaryTypeId"];
            }
            if (dr.Table.Columns.Contains("DepartmentId"))
            {
                entity.DepartmentId = dr["DepartmentId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["DepartmentId"];
            }
            if (dr.Table.Columns.Contains("Salary"))
            {
                entity.Salary = dr["Salary"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Salary"];
            }
            if (dr.Table.Columns.Contains("Addition"))
            {
                entity.Addition = dr["Addition"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Addition"];
            }
            if (dr.Table.Columns.Contains("Deduction"))
            {
                entity.Deduction = dr["Deduction"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Deduction"];
            }
            if (dr.Table.Columns.Contains("NetSalary"))
            {
                entity.NetSalary = dr["NetSalary"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["NetSalary"];
            }



            if (dr.Table.Columns.Contains("DepartmentName"))
            {
                entity.DepartmentName = Convert.ToString(dr["DepartmentName"]);
            }
            if (dr.Table.Columns.Contains("Designation"))
            {
                entity.Designation = Convert.ToString(dr["Designation"]);
            }
            if (dr.Table.Columns.Contains("NICNo"))
            {
                entity.NICNo = Convert.ToString(dr["NICNo"]);
            }
            if (dr.Table.Columns.Contains("SalaryTypeName"))
            {
                entity.SalaryTypeName = Convert.ToString(dr["SalaryTypeName"]);
            }
            //if (dr.Table.Columns.Contains("PayrollDetailName"))
            //{
            //    entity.PayrollDetailName = Convert.ToString(dr["PayrollDetailName"]);
            //}
            if (dr.Table.Columns.Contains("PayrollCycleName"))
            {
                entity.PayrollCycleName = Convert.ToString(dr["PayrollCycleName"]);
            }
            if (dr.Table.Columns.Contains("PayrollVariableName"))
            {
                entity.PayrollVariableName = Convert.ToString(dr["PayrollVariableName"]);
            }
            if (dr.Table.Columns.Contains("UserFName"))
            {
                entity.UserFName = Convert.ToString(dr["UserFName"]);
            }
            //if (dr.Table.Columns.Contains("UserMName"))
            //{
            //    entity.UserMName = Convert.ToString(dr["UserMName"]);
            //}
            //if (dr.Table.Columns.Contains("UserLName"))
            //{
            //    entity.UserLName = Convert.ToString(dr["UserLName"]);
            //}

            return entity;
        }
        //New
        public virtual VMModifyPayrollEdit VMModifyPayrollEditFromDataRow(DataRow dr)
        {
            if (dr == null) return null;
            VMModifyPayrollEdit entity = new VMModifyPayrollEdit();
            //if (dr.Table.Columns.Contains("ID"))
            //{
            //    entity.Id = (System.Int32)dr["ID"];
            //}
            if (dr.Table.Columns.Contains("PayrollCycleID"))
            {
                entity.PayrollCycleId = dr["PayrollCycleID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollCycleID"];
            }
            if (dr.Table.Columns.Contains("PayrollDetailId"))
            {
                entity.PayrollDetailId = dr["PayrollDetailId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollDetailId"];
            }
            if (dr.Table.Columns.Contains("PayrollVariableId"))
            {
                entity.PayrollVariableId = dr["PayrollVariableId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollVariableId"];
            }
            if (dr.Table.Columns.Contains("PayrollDetailName"))
            {
                entity.PayrollDetailName = (System.String)dr["PayrollDetailName"];
            }
            if (dr.Table.Columns.Contains("UserName"))
            {
                entity.UserName = (System.String)dr["UserName"];
            }
            if (dr.Table.Columns.Contains("PayrollId"))
            {
                entity.PayrollId = dr["PayrollId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["PayrollId"];
            }
            if (dr.Table.Columns.Contains("UserID"))
            {
                entity.UserId = dr["UserID"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["UserID"];
            }
            if (dr.Table.Columns.Contains("SalaryTypeId"))
            {
                entity.SalaryTypeId = dr["SalaryTypeId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["SalaryTypeId"];
            }
            if (dr.Table.Columns.Contains("DepartmentId"))
            {
                entity.DepartmentId = dr["DepartmentId"] == DBNull.Value ? (System.Int32?)null : (System.Int32?)dr["DepartmentId"];
            }

            if (dr.Table.Columns.Contains("DepartmentName"))
            {
                entity.DepartmentName = (System.String)dr["DepartmentName"];
            }
            if (dr.Table.Columns.Contains("Salary"))
            {
                entity.Salary = dr["Salary"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Salary"];
            }
            if (dr.Table.Columns.Contains("Addition"))
            {
                entity.Addition = dr["Addition"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Addition"];
            }
            if (dr.Table.Columns.Contains("Deduction"))
            {
                entity.Deduction = dr["Deduction"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["Deduction"];
            }
            if (dr.Table.Columns.Contains("NetSalary"))
            {
                entity.NetSalary = dr["NetSalary"] == DBNull.Value ? (System.Decimal?)null : (System.Decimal?)dr["NetSalary"];
            }

            return entity;
        }
    }
	
	
}
