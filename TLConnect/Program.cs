using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace TLConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            string tlSender = "PCE-01";
            string tlReceiver = "PCE";
            string tlRequestId = "201807180900";
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
            

            try
            {
                tlClient.Open();
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
                Console.WriteLine(tlSNResponse.RandomizedNumberList);
                Console.ReadLine();
            }
        }    
    }
}
