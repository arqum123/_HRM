using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace HRM.Core.Entities
{
	[Serializable]
    public class SearchColumn
    {
        public SearchColumn()
        {
            IsVisible = true;
            IgnoreForDefaultSelect = false;
            this.Operand = getOperandString(Operands.Equal);
            this.Criteria = Criterias.And.ToString();
        }

        public SearchColumn(string name, string value)
            : this()
        {
            this.Name = name;
            this.Value = value;
        }

        public SearchColumn(string name, string value, Operands operand)
            : this(name, value)
        {
            this.Operand = getOperandString(operand);
        }

        public SearchColumn(string name, string value, Operands operand, Criterias criteria)
            : this(name, value, operand)
        {
            this.Criteria = criteria.ToString();
        }

        public string Name { get; set; }
        public string SelectClause { get; set; }
        public string WhereClause { get; set; }
        public string BulkUpdateClause { get; set; }
        public int ParameterCount { get; set; }
        public bool IsBasicSearchColumm { get; set; }
        public bool IsAdvanceSearchColumn { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public bool IsForeignColumn { get; set; }
        public string Title { get; set; }
        public string Operand { get; set; }
        public string Criteria { get; set; }
        public int DisplayOrder { get; set; }
        public bool UseCustomStatusForBoolDataType { get; set; } public bool UseCustomDropDown { get; set; }
        public List<DropDownDataItem> BasicSearchDropDownList { get; set; }
        public List<DropDownDataItem> AdvanceSearchDropDownList { get; set; }
        public bool IsVisible { get; set; }
        public int Tag { get; set; }
        public int GroupId { get; set; }
        public string GroupCriteria { get; set; }
        public bool IsStartsWithSearching { get; set; }
        public bool IsIncludeNULLValue { get; set; }
        public bool IsScheduledDateColumn { get; set; }
        public string TableName { get; set; }
        public bool IgnoreForDefaultSelect { get; set; }
        public string EnumName { get; set; }
        public string CustomizedName { get; set; }
        public bool IgnoreForDisplay { get; set; }
        public bool UpdateColumnIfValueIsEmpty { get; set; }
        public bool HasParent { get; set; }
        public bool HideEqualSign { get; set; }
        public bool RemoveCharacters { get; set; }
        public string SQLParamaterName { get; set; }
		public string PropertyName { get; set; }

        private string getOperandString(Operands operand)
        {
            if (operand.Equals(Operands.Equal))
            {
                return "=";
            }
            else if (operand.Equals(Operands.GreaterEqual))
            {
                return ">=";
            }
            else if (operand.Equals(Operands.LesserEqual))
            {
                return "<=";
            }
            else if (operand.Equals(Operands.Greater))
            {
                return ">";
            }
            else if (operand.Equals(Operands.Lesser))
            {
                return "<";
            }
            else if (operand.Equals(Operands.Like))
            {
                return "like";
            }
            else if (operand.Equals(Operands.Between))
            {
                return "between";
            }
            else
            {
                return "=";
            }


        }
    }
    [Serializable]
    public class DropDownDataItem
    {
        public DropDownDataItem()
        {
        }
        public DropDownDataItem(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public enum Operands
    {
        Equal,
        GreaterEqual,
        LesserEqual,
        Greater,
        Lesser,
        Like,
        Between,
        In
    }
    public enum Criterias
    {
        And,
        Or
    }	
}
