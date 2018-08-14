using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;


namespace TLConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            string tlSender = "PCE-01";
            string tlReceiver = "PCE";
            string tlRequestId = "201808131848";
            string tlGtin = "02768000000014";
            string tlUser = "holger.prohl@mt.com";
            string tlPassword = "xxx";
            int tlAmount = 100;

            Console.WriteLine("Tracelink Connect");
            Console.WriteLine("\n\n");

            TraceLink.objectKey tlObjectKey = new TraceLink.objectKey
            {
                Name = TraceLink.objectIdentifierType.GTIN,
                Value = tlGtin
            };

            tlObjectKey.NameSpecified = true;

            TraceLink.SNResponse tlSNResponse = new TraceLink.SNResponse();
            TraceLink.ReferenceDocuments referenceDocuments = new TraceLink.ReferenceDocuments();

            TraceLink.snrequestClient tlClient = new TraceLink.snrequestClient();
            tlClient.ClientCredentials.UserName.UserName = tlUser;
            tlClient.ClientCredentials.UserName.Password = tlPassword;

            using (OperationContextScope scope = new OperationContextScope(tlClient.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();

                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic "
                    + Convert.ToBase64String(Encoding.ASCII.GetBytes(tlClient.ClientCredentials.UserName.UserName + ":"
                    + tlClient.ClientCredentials.UserName.Password));

                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
               
                try
                {
                    //tlClient.Open();
                    Console.WriteLine("Client state: " + tlClient.State.ToString());

                    tlSNResponse = tlClient.serialNumbersRequest(tlSender, tlReceiver,
                        TraceLink.idType.GS1_SER, TraceLink.encodingType.SGTIN,
                        tlAmount, tlObjectKey,
                        tlRequestId, referenceDocuments);
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error: " + error.Message.ToString());
                }
                finally
                {
                    tlClient.Close();
                    Console.WriteLine("List length: " + tlSNResponse.RandomizedNumberList.Length.ToString());
                    if (tlSNResponse.RandomizedNumberList.Length > 0)
                    {
                        foreach(string sn in tlSNResponse.RandomizedNumberList)
                        {
                            Console.WriteLine(sn);
                        }
                    }
                    Console.WriteLine("--Finish--");
                    Console.ReadLine();
                }


            }




        }    
    }
}
