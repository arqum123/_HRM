using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace HRM.Core.Entities
{
    [DataContract]
	public abstract partial class DeviceBase:EntityBase, IEquatable<DeviceBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("MachineID",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? MachineId{ get; set; }

		[FieldNameAttribute("DeviceID",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? DeviceId{ get; set; }

		[FieldNameAttribute("ConnectionTypeID",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? ConnectionTypeId{ get; set; }

		[FieldNameAttribute("DeviceModalID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? DeviceModalId{ get; set; }

		[FieldNameAttribute("IPAddress",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String IpAddress{ get; set; }

		[FieldNameAttribute("PortNumber",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? PortNumber{ get; set; }

		[FieldNameAttribute("Password",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Password{ get; set; }

		[FieldNameAttribute("ComPort",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? ComPort{ get; set; }

		[FieldNameAttribute("Baudrate",true,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int64? Baudrate{ get; set; }

		[FieldNameAttribute("Status",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Status{ get; set; }

		[FieldNameAttribute("StatusDescription",true,false,1000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String StatusDescription{ get; set; }

		[FieldNameAttribute("BranchID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? BranchId{ get; set; }

		[FieldNameAttribute("IsActive",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsActive{ get; set; }

		[FieldNameAttribute("CreationDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdateDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? UpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string UpdateDateStr
		{
			 get {if(UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdateBy",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UpdateBy{ get; set; }

		[FieldNameAttribute("UserIP",true,false,20)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String UserIp{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<DeviceBase> Members

        public virtual bool Equals(DeviceBase other)
        {
			if(this.Id==other.Id  && this.MachineId==other.MachineId  && this.DeviceId==other.DeviceId  && this.ConnectionTypeId==other.ConnectionTypeId  && this.DeviceModalId==other.DeviceModalId  && this.IpAddress==other.IpAddress  && this.PortNumber==other.PortNumber  && this.Password==other.Password  && this.ComPort==other.ComPort  && this.Baudrate==other.Baudrate  && this.Status==other.Status  && this.StatusDescription==other.StatusDescription  && this.BranchId==other.BranchId  && this.IsActive==other.IsActive  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(Device other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.MachineId=other.MachineId;
				this.DeviceId=other.DeviceId;
				this.ConnectionTypeId=other.ConnectionTypeId;
				this.DeviceModalId=other.DeviceModalId;
				this.IpAddress=other.IpAddress;
				this.PortNumber=other.PortNumber;
				this.Password=other.Password;
				this.ComPort=other.ComPort;
				this.Baudrate=other.Baudrate;
				this.Status=other.Status;
				this.StatusDescription=other.StatusDescription;
				this.BranchId=other.BranchId;
				this.IsActive=other.IsActive;
				this.CreationDate=other.CreationDate;
				this.UpdateDate=other.UpdateDate;
				this.UpdateBy=other.UpdateBy;
				this.UserIp=other.UserIp;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
