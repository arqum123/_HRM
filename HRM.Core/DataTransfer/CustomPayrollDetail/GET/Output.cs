using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HRM.Core.DataTransfer.CustomPayrollDetail.GET
{
    public class Output
    {
        [DataContract]
        public class GetOutput
        {

            [DataMember(EmitDefaultValue = false)]
            public System.Int32 Id { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public System.Int32? PayrollId { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public System.Int32? PayrollPolicyId { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public System.Decimal? Amount { get; set; }

            [DataMember(EmitDefaultValue = false)]
            public System.Boolean? IsActive { get; set; }

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
}
