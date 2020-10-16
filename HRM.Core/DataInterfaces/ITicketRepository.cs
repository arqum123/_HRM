using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
    public interface ITicketRepository: ITicketRepositoryBase
    {
        System.Data.DataSet GetPendingTicketsDetail(); //NewModifyPayroll
        System.Data.DataSet GetTicketByUserIdAndDateRangeSP(System.Int32? UserId,System.DateTime StartDate,System.DateTime EndDate);
        
    }

}

