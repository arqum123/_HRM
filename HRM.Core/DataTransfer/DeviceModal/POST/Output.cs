using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.DeviceModal
{
    [DataContract]
	public class PostOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String DeviceModal{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreatedDateStr
		{
			 get {if(CreatedDate.HasValue) return CreatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreatedDate = date.ToUniversalTime();  }  } 
		}

		[IgnoreDataMember]
		public System.DateTime? UpdationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string UpdationDateStr
		{
			 get {if(UpdationDate.HasValue) return UpdationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdationDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UpdatedBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String UserIp{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsActive{ get; set; }

	}	
}
