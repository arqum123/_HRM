using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HRM.Core.Entities
{
    [DataContract]
    public abstract partial class CustomPayrollDetailBase : EntityBase, IEquatable<CustomPayrollDetailBase>
    {

        [PrimaryKey]
        [FieldNameAttribute("ID", false, false, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32 Id { get; set; }

        [FieldNameAttribute("PayrollID", true, true, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? PayrollId { get; set; }

        [FieldNameAttribute("PayrollPolicyID", true, true, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? PayrollPolicyId { get; set; }

        [FieldNameAttribute("Amount", true, false, 5)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Decimal? Amount { get; set; }

        [FieldNameAttribute("IsActive", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsActive { get; set; }

        [FieldNameAttribute("CreationDate", true, false, 8)]
        [IgnoreDataMember]
        public virtual System.DateTime? CreationDate { get; set; }



        [DataMember(EmitDefaultValue = false)]
        public virtual string CreationDateStr
        {
            get { if (CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime(); } }
        }

        [FieldNameAttribute("UpdateDate", true, false, 8)]
        [IgnoreDataMember]
        public virtual System.DateTime? UpdateDate { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public virtual string UpdateDateStr
        {
            get { if (UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime(); } }
        }

        [FieldNameAttribute("UpdateBy", true, false, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? UpdateBy { get; set; }

        [FieldNameAttribute("UserIP", true, false, 20)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.String UserIp { get; set; }

        [FieldNameAttribute("PayrollPolicyName", true, false, 20)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.String PayrollPolicyName { get; set; }


        public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }

        #region IEquatable<PayrollDetailBase> Members

        public virtual bool Equals(CustomPayrollDetailBase other)
        {
            if (this.Id == other.Id && this.PayrollId == other.PayrollId && this.PayrollPolicyId == other.PayrollPolicyId && this.Amount == other.Amount && this.IsActive == other.IsActive && this.CreationDate == other.CreationDate && this.UpdateDate == other.UpdateDate && this.UpdateBy == other.UpdateBy && this.UserIp == other.UserIp)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public virtual void CopyFrom(CustomPayrollDetail other)
        {
            if (other != null)
            {
                this.Id = other.Id;
                this.PayrollId = other.PayrollId;
                this.PayrollPolicyId = other.PayrollPolicyId;
                this.Amount = other.Amount;
                this.IsActive = other.IsActive;
                this.CreationDate = other.CreationDate;
                this.UpdateDate = other.UpdateDate;
                this.UpdateBy = other.UpdateBy;
                this.UserIp = other.UserIp;
            }


        }

        #endregion



    }
}
