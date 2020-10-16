using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Branch
{
    [DataContract]
	public class PutInput
	{
			
		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = false)]
		[DataMember (EmitDefaultValue=false)]
		public string Id{ get; set; }

		[FieldLength(MaxLength = 500)]
		[DataMember (EmitDefaultValue=false)]
		public string Name{ get; set; }

		[FieldLength(MaxLength = 1000)]
		[DataMember (EmitDefaultValue=false)]
		public string AddressLine{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string CityId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string StateId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[DataMember (EmitDefaultValue=false)]
		public string CountryId{ get; set; }

		[FieldLength(MaxLength = 20)]
		[DataMember (EmitDefaultValue=false)]
		public string ZipCode{ get; set; }

		[FieldLength(MaxLength = 50)]
		[DataMember (EmitDefaultValue=false)]
		public string PhoneNumber{ get; set; }

		[FieldLength(MaxLength = 500)]
		[DataMember (EmitDefaultValue=false)]
		public string Guid{ get; set; }

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
