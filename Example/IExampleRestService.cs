using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Example
{
    [ServiceContract(Name = "ExampleRestServices")]
    public interface IExampleRestService
    {
        [OperationContract]
        [WebGet(UriTemplate = UriTemplates.Authenticate, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Authenticate(string userid, string password, string env);

        [OperationContract]
        [WebGet(UriTemplate = UriTemplates.GetReport, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetReport(string reportId, string asofdate, string sessionId);
    }
}