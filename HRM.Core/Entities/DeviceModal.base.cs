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
	public abstract partial class DeviceModalBase:EntityBase, IEquatable<DeviceModalBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("DeviceModal",true,false,100)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String DeviceModal{ get; set; }

		[FieldNameAttribute("CreatedDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreatedDateStr
		{
			 get {if(CreatedDate.HasValue) return CreatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreatedDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdationDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? UpdationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string UpdationDateStr
		{
			 get {if(UpdationDate.HasValue) return UpdationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdationDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdatedBy",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UpdatedBy{ get; set; }

		[FieldNameAttribute("UserIP",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String UserIp{ get; set; }

		[FieldNameAttribute("IsActive",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsActive{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<DeviceModalBase> Members

        public virtual bool Equals(DeviceModalBase other)
        {
			if(this.Id==other.Id  && this.DeviceModal==other.DeviceModal  && this.UpdationDate==other.UpdationDate  && this.UpdatedBy==other.UpdatedBy  && this.UserIp==other.UserIp  && this.IsActive==other.IsActive )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(DeviceModal other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.DeviceModal=other.DeviceModal;
				this.CreatedDate=other.CreatedDate;
				this.UpdationDate=other.UpdationDate;
				this.UpdatedBy=other.UpdatedBy;
				this.UserIp=other.UserIp;
				this.IsActive=other.IsActive;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
