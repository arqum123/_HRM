﻿using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.UserContact
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

		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string ContactTypeId{ get; set; }

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
