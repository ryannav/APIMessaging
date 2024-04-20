using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebGet(UriTemplate = "SendMsg?receiver={receiverID}&sender={senderID}&msg={msg}", ResponseFormat = WebMessageFormat.Xml)]//api address
        void sendMsg(String receiverID, String senderID, String msg);

        [OperationContract]
        [WebGet(UriTemplate = "ReceiveMsg?receiver={receiverID}&purge={purge}", ResponseFormat = WebMessageFormat.Xml)]//api address
        String[] receiveMsg(String receiverID, Boolean purge);
    }

}
