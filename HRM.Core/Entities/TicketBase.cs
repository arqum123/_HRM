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
    public abstract partial class TicketBase : EntityBase, IEquatable<TicketBase>
    {
        [PrimaryKey]
        [FieldNameAttribute("ID", false, false, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32 ID { get; set; }
        [FieldNameAttribute("AttendanceID", true, true, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? AttendanceID { get; set; }

        [FieldNameAttribute("UserID", true, true, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? UserID { get; set; }
        [FieldNameAttribute("Reason", false, false, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.String Reason { get; set; }
        [FieldNameAttribute("IsApproved", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsApproved { get; set; }
        [FieldNameAttribute("IsReject", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsReject { get; set; }
        [FieldNameAttribute("Comments", false, false, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.String Comments { get; set; }

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


        public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }

        #region IEquatable<TicketBase> Members

        public virtual bool Equals(Ticket other)
        {
            if (this.ID == other.ID && this.AttendanceID == other.AttendanceID && this.UserID == other.UserID  && this.Reason == other.Reason && this.IsApproved == other.IsApproved && this.IsReject == other.IsReject && this.Comments == other.Comments && this.UpdateDate == other.UpdateDate && this.UpdateBy == other.UpdateBy && this.UserIp == other.UserIp)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public virtual void CopyFrom(Ticket other)
        {
            if (other != null)
            {
                this.ID = other.ID;
                this.AttendanceID = other.AttendanceID;
                this.UserID = other.UserID;
                this.Reason = other.Reason;
                this.IsApproved = other.IsApproved;
                this.IsReject = other.IsReject;
                this.Comments = other.Comments;
                this.CreationDate = other.CreationDate;
                this.UpdateDate = other.UpdateDate;
                this.UpdateBy = other.UpdateBy;
                this.UserIp = other.UserIp;
            }


        }

        public bool Equals(TicketBase other)
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
