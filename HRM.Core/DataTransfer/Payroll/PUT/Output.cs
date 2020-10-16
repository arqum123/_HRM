using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Payroll
{
    [DataContract]
	public class PutOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? PayrollCycleId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UserId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Decimal? Salary{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Decimal? Addition{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Decimal? Deduction{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Decimal? NetSalary{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsActive{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[IgnoreDataMember]
		public System.DateTime? UpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string UpdateDateStr
		{
			 get {if(UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UpdateBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String UserIp{ get; set; }

	}	
}
