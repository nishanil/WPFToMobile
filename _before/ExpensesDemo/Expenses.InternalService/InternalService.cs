using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Expenses.InternalService
{
    [ServiceContract]
    public interface IInternalService
    {
        [OperationContract]
        void ProcessReport(int expenseReportId);
    }
}
