﻿using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Payroll
{
    [DataContract]
	public class PostInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string Id{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string PayrollCycleId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string UserId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Decimal)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Salary{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Decimal)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Addition{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Decimal)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Deduction{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Decimal)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string NetSalary{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsActive{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreationDate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string UpdateDate{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string UpdateBy{ get; set; }

		[FieldLength(MaxLength = 20)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string UserIp{ get; set; }

	}	
}
