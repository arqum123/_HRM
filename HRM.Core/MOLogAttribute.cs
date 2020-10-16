using System;

namespace HRM.Core
{

    public enum AuditOperations
    {
        Create,
        Update,
        Delete,
        Get,
        GetAll
    }
    public class MOLogAttribute:Attribute
    {
        public MOLogAttribute(AuditOperations operation,Type entityType)
        {
            this.EntityType = entityType;
            this.Operation = operation;
        }

        public Type EntityType { get; set; }
        public AuditOperations Operation { get; set; }
    }
	
	 public class FieldNameAttribute : Attribute
    {
        public string FieldName { get; private set; }
        public bool AllowNull { get; private set; }
        public bool ForeignKeyField { get; private set; }
        public int Length { get; private set; }
        public FieldNameAttribute(string fieldName, bool isAllowNull, bool isForeignField, int length)
        {
            this.FieldName = fieldName;
            this.ForeignKeyField = isForeignField;
            this.AllowNull = isAllowNull;
            this.Length = length;
        }
    }
	
}

