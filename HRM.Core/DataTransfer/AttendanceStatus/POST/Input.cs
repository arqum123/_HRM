using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.AttendanceStatus
{
    [DataContract]
	public class PostInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string Id{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string AttendanceId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsShiftOffDay{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsHoliday{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsLeaveDay{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsQuarterDay{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsHalfDay{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsFullDay{ get; set; }

		[FieldLength(MaxLength = 2000)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Reason{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsApproved{ get; set; }
        [FieldTypeValidation(DataType = DataTypes.Boolean)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string IsReject { get; set; }
        [FieldLength(MaxLength = 2000)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string AdminReason { get; set; }

        [FieldLength(MaxLength = 2000)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Remarks{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string ActionBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string ActionDate{ get; set; }

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

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsLate{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Boolean)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IsEarly{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string LateMinutes{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string EarlyMinutes{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string WorkingMinutes{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string TotalMinutes{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string OverTimeMinutes{ get; set; }

		[FieldLength(MaxLength = 50)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string BreakType{ get; set; }

	}	
}
