using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;
using System.Text;
using System.IO;
using System.Web;

using MetricsManager.Common;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace MetricsManager.GM.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MMProService : MMPro_Contract
    {
        private static readonly ILog loggingHandler = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string sayHello(string str)
        {
            return string.Format("From Server: {0}", str);
        }

        public GMResponse GMProcess(Stream gmstream)
        {
            MMICONFIG appConfig = new MMICONFIG();
            GMConfig gconfig = new GMConfig();
            gconfig.filePath = appConfig.getString("htdocsPath");
            gconfig.backupPath = appConfig.getString("failedlogsPath");
            gconfig.dbServer = appConfig.getString("dbServer");
            gconfig.dbName = appConfig.getString("dbName");
            gconfig.dbUser = appConfig.getString("dbUser");
            gconfig.dbPwd = appConfig.getString("dbPwd");
            gconfig.dbTrust = appConfig.getBool("dbTrust");
            gconfig.debug = true;

            gconfig.appPath = HttpContext.Current.Request.MapPath(Path.Combine(HttpContext.Current.Request.ApplicationPath, "bin"));

            GMResponse respObj = MMIGMProcess.process(gmstream, gconfig, MMIMode.MMPro);
            
            WebOperationContext ctx = WebOperationContext.Current;
            switch (respObj.errorCode)
            {
                case 0:
                    loggingHandler.Info(MMIJSON.ObjectToString(respObj));
                    ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    break;
                case 1:
                    loggingHandler.Error(MMIJSON.ObjectToString(respObj));
                    ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    break;
                case 2:
                    loggingHandler.Error(MMIJSON.ObjectToString(respObj));
                    ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
                    break;
                case 3:
                    loggingHandler.Error(MMIJSON.ObjectToString(respObj));
                    ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
                    break;
                default:
                    break;
            } // switch

            return respObj;
        }
    }
}
