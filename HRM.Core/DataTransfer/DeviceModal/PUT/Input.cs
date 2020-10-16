using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.DeviceModal
{
    [DataContract]
	public class PutInput
	{
			
		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string Id{ get; set; }

		[FieldLength(MaxLength = 100)]
		[DataMember (EmitDefaultValue=false)]
		public string DeviceModal{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreatedDate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string UpdationDate{ get; set; }

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
