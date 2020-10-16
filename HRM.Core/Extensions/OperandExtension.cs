using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRM.Core.Entities;

namespace HRM.Core.Extensions
{
	public static class OperandExtension        
    {
        public static string ToOperandString(this Operands operand)
        {
            return  getOperandString(operand);
        }

        private static string getOperandString(Operands operand)
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
}