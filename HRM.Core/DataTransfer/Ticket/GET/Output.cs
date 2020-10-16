using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Validation;

namespace HRM.Core.DataTransfer.Ticket
{
    public class GetOutput
    {
        [DataMember(EmitDefaultValue = false)]
        public System.Int32 ID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public System.Int32 AttendanceID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        //public System.Int32 PayrollDetailID { get; set; }
        //[DataMember(EmitDefaultValue = false)]
   
        public System.Int32 UserID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        //public System.Int32 PayrollID { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public System.String Reason { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public System.Boolean? IsApproved { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public System.Boolean? IsReject { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public System.String Comments { get; set; }

        [IgnoreDataMember]
        public System.DateTime? CreationDate { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public string CreationDateStr
        {
            get { if (CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime(); } }
        }


        [IgnoreDataMember]
        public System.DateTime? UpdateDate { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public string UpdateDateStr
        {
            get { if (UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime(); } }
        }

        [DataMember(EmitDefaultValue = false)]
        public System.Int32? UpdateBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public System.String UserIp { get; set; }

    }
}
