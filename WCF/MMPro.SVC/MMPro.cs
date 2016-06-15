using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace MetricsManager.GM.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface MMPro_Contract
    {
        [OperationContract]
        [WebGet(UriTemplate = "sayHello(message={str})", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        string sayHello(string str);

        [OperationContract]
        [WebInvoke(UriTemplate = "GMProcess", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        GMResponse GMProcess(Stream gmstream);
    }
}
