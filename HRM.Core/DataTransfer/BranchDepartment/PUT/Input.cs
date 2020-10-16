using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.BranchDepartment
{
    [DataContract]
	public class PutInput
	{
			
		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = false)]
		[DataMember (EmitDefaultValue=false)]
		public string Id{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string BranchId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string DepartmentId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreatedDate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string UpdatedDate{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string UpdatedBy{ get; set; }

		[FieldLength(MaxLength = 50)]
		[DataMember (EmitDefaultValue=false)]
		public string UserIp{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[DataMember (EmitDefaultValue=false)]
		public string IsActive{ get; set; }

	}	
}
