using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Expenses.InternalService
{
    public class InternalService : IInternalService
    {
        public void ProcessReport(int expenseReportId)
        {
        }
    }
}
