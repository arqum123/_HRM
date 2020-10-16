using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Validation;

namespace HRM.Core.DataTransfer.Ticket
{
    public class PostInput
    {
        [DataMember(EmitDefaultValue = false)]
        public string ID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string AttendanceID { get; set; }
        //[DataMember(EmitDefaultValue = false)]
        //public string PayrollDetailID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string UserID { get; set; }
        //[DataMember(EmitDefaultValue = false)]
        //public string PayrollID { get; set; }


        [FieldLength(MaxLength = 100)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string Reason { get; set; }

        [FieldTypeValidation(DataType = DataTypes.Boolean)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string IsApproved { get; set; }
        [FieldTypeValidation(DataType = DataTypes.Boolean)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string IsReject { get; set; }
        [FieldLength(MaxLength = 100)]
        [FieldNullable(IsNullable = true)]
        [DataMember(EmitDefaultValue = false)]
        public string Comments { get; set; }
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
