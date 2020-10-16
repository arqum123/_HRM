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
	public abstract partial class BranchShiftBase:EntityBase, IEquatable<BranchShiftBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("BranchID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? BranchId{ get; set; }

		[FieldNameAttribute("ShiftID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? ShiftId{ get; set; }

		[FieldNameAttribute("CreatedDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreatedDateStr
		{
			 get {if(CreatedDate.HasValue) return CreatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreatedDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdatedDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? UpdatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string UpdatedDateStr
		{
			 get {if(UpdatedDate.HasValue) return UpdatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdatedDate = date.ToUniversalTime();  }  } 
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
		
		#region IEquatable<BranchShiftBase> Members

        public virtual bool Equals(BranchShiftBase other)
        {
			if(this.Id==other.Id  && this.BranchId==other.BranchId  && this.ShiftId==other.ShiftId  && this.UpdatedDate==other.UpdatedDate  && this.UpdatedBy==other.UpdatedBy  && this.UserIp==other.UserIp  && this.IsActive==other.IsActive )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(BranchShift other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.BranchId=other.BranchId;
				this.ShiftId=other.ShiftId;
				this.CreatedDate=other.CreatedDate;
				this.UpdatedDate=other.UpdatedDate;
				this.UpdatedBy=other.UpdatedBy;
				this.UserIp=other.UserIp;
				this.IsActive=other.IsActive;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
