using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.PayrollPolicy
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
		public string PayrollVariableId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[DataMember (EmitDefaultValue=false)]
		public string IsUnit{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[DataMember (EmitDefaultValue=false)]
		public string IsPercentage{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Decimal)]
		[DataMember (EmitDefaultValue=false)]
		public string Value{ get; set; }

		[FieldLength(MaxLength = 1000)]
		[DataMember (EmitDefaultValue=false)]
		public string Description{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string EffectiveDate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string RetiredDate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreationDate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string UpdateDate{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string UpdateBy{ get; set; }

		[FieldLength(MaxLength = 20)]
		[DataMember (EmitDefaultValue=false)]
		public string UserIp{ get; set; }

	}	
}
