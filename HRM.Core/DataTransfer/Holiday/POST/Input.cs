using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Holiday
{
    [DataContract]
	public class PostInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string Id{ get; set; }

		[FieldLength(MaxLength = 100)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Name{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Date{ get; set; }

		[FieldLength(MaxLength = 200)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Detail{ get; set; }

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
