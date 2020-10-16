using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.PayrollPolicy
{
    [DataContract]
	public class GetOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? PayrollVariableId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsUnit{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsPercentage{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Decimal? Value{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Description{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? EffectiveDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string EffectiveDateStr
		{
			 get {if(EffectiveDate.HasValue) return EffectiveDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { EffectiveDate = date.ToUniversalTime();  }  } 
		}

		[IgnoreDataMember]
		public System.DateTime? RetiredDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string RetiredDateStr
		{
			 get {if(RetiredDate.HasValue) return RetiredDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { RetiredDate = date.ToUniversalTime();  }  } 
		}

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
