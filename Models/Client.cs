using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insurance_management.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ContactInfo { get; set; }
       

        public Client() { }

        public Client(int clientId, string clientName, string contactInfo)
        {
            ClientId = clientId;
            ClientName = clientName;
            ContactInfo = contactInfo;
            
        }

        public override string ToString()
        {
            return $"Client [ClientId={ClientId}, ClientName={ClientName}]";
        }
    }
}
