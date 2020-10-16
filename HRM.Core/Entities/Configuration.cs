
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Configuration : ConfigurationBase 
	{

        public Boolean BoolValue
        {
            get
            {
                if (!String.IsNullOrEmpty(Value))
                    return Convert.ToBoolean(Convert.ToInt32(Value));
                return false;
            }
            set
            {
                Value = Convert.ToInt32(value).ToString();
            }
        }
		
	}
}
