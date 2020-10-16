using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Leave
{
    [DataContract]
	public class PostInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string Id{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string UserId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Date{ get; set; }

		[FieldLength(MaxLength = 500)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Reason{ get; set; }
        [FieldLength(MaxLength = 500)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string AdminReason { get; set; }

        [FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string LeaveTypeId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsActive{ get; set; }
        [FieldTypeValidation(DataType = DataTypes.Boolean)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string IsApproved { get; set; }
        [FieldTypeValidation(DataType = DataTypes.Boolean)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string IsReject { get; set; }

        [DataMember (EmitDefaultValue=false)]
		public string UpdateDate{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string UpdatedBy{ get; set; }

		[FieldLength(MaxLength = 20)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string UserIp{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreationDate{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LeaveDateEnd { get; set; }

    }	
}
