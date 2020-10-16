
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Configuration;
using System.Linq;

namespace HRM.Core.Entities
{
		
	public class EntityHelper
    {
        public static bool IsTransient(object entity)
        {

            PropertyInfo[] propertiesArray = entity.GetType().GetProperties();
            PropertyInfo prop = null;
            foreach (PropertyInfo info in propertiesArray)
            {
                object obj = info.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).FirstOrDefault();
                if (obj != null)
                {
                    prop = info;
                    break;
                }
            }

            if (prop == null)
            {
                return true;
            }
            else
            {
                object value = prop.GetValue(entity, null);
                if (Convert.ToInt32(value) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
    }
	
	public class PrimaryKeyAttribute : Attribute
    {

    }
	
	
}
