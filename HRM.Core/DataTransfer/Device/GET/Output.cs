using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Device
{
    [DataContract]
	public class GetOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? MachineId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? DeviceId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? ConnectionTypeId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? DeviceModalId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String IpAddress{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? PortNumber{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Password{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? ComPort{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int64? Baudrate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Status{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String StatusDescription{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? BranchId{ get; set; }

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
