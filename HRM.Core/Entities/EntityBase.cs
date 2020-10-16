using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HRM.Core.Entities
{
	[DataContract]
    public abstract class EntityBase
    {
        [IgnoreDataMember]
        public bool IsUpdated { get; set; }
    }
}