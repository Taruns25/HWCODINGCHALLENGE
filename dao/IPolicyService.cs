using insurance_management.Models;
using System.Collections.Generic;

namespace insurance_management.dao
{
    public interface IPolicyService
    {
        // Method to create a new policy
        bool CreatePolicy(Policy policy);

        // Method to retrieve a policy by its ID
        Policy GetPolicy(int policyId);

        // Method to retrieve all policies
        IEnumerable<Policy> GetAllPolicies();

        // Method to update an existing policy
        bool UpdatePolicy(Policy policy);

        // Method to delete a policy by its ID
        bool DeletePolicy(int policyId);

        // Method for user login
        User Login(string username, string password);

        // Method to claim a policy
        bool ClaimPolicy(int policyId, int clientId);
    }
}
