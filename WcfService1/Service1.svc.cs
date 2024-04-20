using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string XMLLocale = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data\Messages.xml");        //locates XML file
        public void sendMsg(String receiverID, String senderID, String msg)     //code is primarily based off of lecture 3-3
        {
            XDocument xmlDocMsgs = new XDocument();
            XNamespace nameSpace = "http://example.com/Messages";
            try
            {
                xmlDocMsgs = XDocument.Load(XMLLocale);
                XElement xmlElement = new XElement(nameSpace + "Message",       //takes arguments given and creates XML elements
                new XElement(nameSpace + "SenderID", senderID),
                new XElement(nameSpace + "ReceiverID", receiverID),
                new XElement(nameSpace + "TS", System.DateTime.Now.ToString()),
                new XElement(nameSpace + "MessageContents", msg));
                xmlDocMsgs.Element(nameSpace + "Messages").Add(xmlElement);
                xmlDocMsgs.Save(XMLLocale);

            }
            catch (XmlException ex)
            {
                if (!(ex.Message.ToLower().Contains("root") && ex.Message.ToLower().Contains("element") && ex.Message.ToLower().Contains("not") && ex.Message.ToLower().Contains("found")))
                {
                    xmlDocMsgs = new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                    new XComment("CSE446 Messaging System Example"),
                    new XElement(nameSpace + "Messages",
                    new XElement(nameSpace + "Message",
                    new XElement(nameSpace + "SenderID", senderID),
                    new XElement(nameSpace + "ReceiverID", receiverID),
                    new XElement(nameSpace + "TS", System.DateTime.Now.ToString()),
                    new XElement(nameSpace + "MessageContents", msg))));
                    xmlDocMsgs.Save(XMLLocale);
                }
            }
        }

        public String[] receiveMsg(String receiverID, Boolean purge)        //code is primarily based off of lecture 3-3
        {
            string[] returnMsg;
            XDocument xmlDocMsgs = XDocument.Load(XMLLocale);
            XNamespace nameSpace = "http://example.com/Messages";
            IEnumerable<XElement> queryElementItems =
            from item in xmlDocMsgs.Root.Descendants(nameSpace + "Message")
            where item.Element(nameSpace + "ReceiverID").Value == receiverID
            orderby (DateTime)item.Element(nameSpace + "TS") descending
            select item;
            returnMsg = new string[queryElementItems.Count() * 3];
            int iter = 0;
            foreach (XElement item in queryElementItems)
            {                                                                       //loads values for each XML element
                returnMsg[iter++] = item.Element(nameSpace + "SenderID").Value;
                returnMsg[iter++] = item.Element(nameSpace + "TS").Value;
                returnMsg[iter++] = item.Element(nameSpace + "MessageContents").Value;
            }
            if (purge == true)                                          //removes all messages with the users id
            {
                xmlDocMsgs.Root.Elements(nameSpace + "Message")
                    .Where(msg => msg.Element(nameSpace + "ReceiverID").Value == receiverID)
                    .Remove();
                xmlDocMsgs.Save(XMLLocale);
            }
            return returnMsg;
        }


    }
}
