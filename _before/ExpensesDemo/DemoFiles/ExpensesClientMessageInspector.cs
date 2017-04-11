using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.WPF
{
    public class ExpensesClientMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;
            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                if (string.IsNullOrEmpty(httpRequestMessage.Headers[HttpRequestHeader.Authorization]))
                {
                    var authResult = Expenses.WPF.AADSignIn.AADAuthResult;
                    if (authResult != null)
                    {
                        httpRequestMessage.Headers[HttpRequestHeader.Authorization] = Expenses.WPF.AADSignIn.AADAuthResult.CreateAuthorizationHeader();
                    }
                }
            }
            else
            {
                var authResult = Expenses.WPF.AADSignIn.AADAuthResult;
                if (authResult != null)
                {
                    httpRequestMessage = new HttpRequestMessageProperty();
                    httpRequestMessage.Headers.Add(HttpRequestHeader.Authorization, Expenses.WPF.AADSignIn.AADAuthResult.CreateAuthorizationHeader());
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
                }
            }
            return null;
        }
    }

    public class ExpensesClientMessageBehavior : IEndpointBehavior
    {

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            ExpensesClientMessageInspector inspector = new ExpensesClientMessageInspector();
            clientRuntime.ClientMessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class ExpensesClientMessageInspectorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ExpensesClientMessageBehavior();
        }

        public override Type BehaviorType
        {
            get 
            {
                return typeof(ExpensesClientMessageBehavior);
            }
        }
    }
}
