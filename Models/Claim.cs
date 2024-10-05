using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insurance_management.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public DateTime DateFiled { get; set; }
        public double ClaimAmount { get; set; }
        public string Status { get; set; }
        public string Policy { get; set; }
        public Client Client { get; set; }

        public Claim() { }

        public Claim(int claimId, string claimNumber, DateTime dateFiled, double claimAmount, string status, string policy, Client client)
        {
            ClaimId = claimId;
            ClaimNumber = claimNumber;
            DateFiled = dateFiled;
            ClaimAmount = claimAmount;
            Status = status;
            Policy = policy;
            Client = client;
        }

        public override string ToString()
        {
            return $"Claim [ClaimId={ClaimId}, ClaimNumber={ClaimNumber}, ClaimAmount={ClaimAmount}, Status={Status}]";
        }
    }
}

