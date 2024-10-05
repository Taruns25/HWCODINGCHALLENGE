using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insurance_management.Models
{
    public class Policy
    {
        // Properties
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string PolicyDetails { get; set; }
        public decimal PremiumAmount { get; set; }
        public int ClientId { get; set; } // Foreign key linking to Clients

        // Default constructor
        public Policy()
        {
        }

        // Parameterized constructor
        public Policy(int policyId, string policyName, string policyDetails, decimal premiumAmount, int clientId)
        {
            PolicyId = policyId;
            PolicyName = policyName;
            PolicyDetails = policyDetails;
            PremiumAmount = premiumAmount;
            ClientId = clientId;
        }

        // Method to display policy information
        public override string ToString()
        {
            return $"Policy ID: {PolicyId}, Name: {PolicyName}, Details: {PolicyDetails}, " +
                   $"Premium Amount: {PremiumAmount}, Client ID: {ClientId}";
        }
    }
}
