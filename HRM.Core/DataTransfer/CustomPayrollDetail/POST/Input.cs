using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Validation;

namespace HRM.Core.DataTransfer.CustomPayrollDetail.POST
{
    [DataContract]
    public class PostInput
    {

        [DataMember(EmitDefaultValue = false)]
        public string Id { get; set; }

        [FieldTypeValidation(DataType = DataTypes.Integer)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string PayrollId { get; set; }

        [FieldTypeValidation(DataType = DataTypes.Integer)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string PayrollPolicyId { get; set; }

        [FieldTypeValidation(DataType = DataTypes.Decimal)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string Amount { get; set; }

        [FieldTypeValidation(DataType = DataTypes.Boolean)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CreationDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UpdateDate { get; set; }

        [FieldTypeValidation(DataType = DataTypes.Integer)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string UpdateBy { get; set; }

        [FieldLength(MaxLength = 20)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string UserIp { get; set; }

    }
}
