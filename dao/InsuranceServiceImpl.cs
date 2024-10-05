using insurance_management.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using insurance_management.dao;
using insurance_management.Models;
using insurance_management.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace insurance_management.dao
{
    public class InsuranceServiceImpl : IPolicyService
    {
        public bool CreatePolicy(Policy policy)
        {
            using (var connection = DBConnUtil.GetConnection())
            {
                // Adjust SQL to include premiumAmount
                string query = "INSERT INTO Policies (policyName, policyDetails, premiumAmount,clientid) VALUES (@PolicyName, @PolicyDetails, @PremiumAmount,@ClientId)";
                using (var command = new SqlCommand(query, connection))
                {
                    // Add parameters including premiumAmount
                    command.Parameters.AddWithValue("@PolicyName", policy.PolicyName);
                    command.Parameters.AddWithValue("@PolicyDetails", policy.PolicyDetails);
                    command.Parameters.AddWithValue("@PremiumAmount", policy.PremiumAmount);
                    command.Parameters.AddWithValue("@ClientId", policy.ClientId);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0; // Return true if the policy was created successfully
                }
            }
        }

        public Policy GetPolicy(int policyId)
        {
            using (var connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Policies WHERE PolicyId = @PolicyId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PolicyId", policyId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Map all properties from the Policies table
                            return new Policy
                            {
                                PolicyId = reader.GetInt32(reader.GetOrdinal("PolicyId")),
                                PolicyName = reader.GetString(reader.GetOrdinal("PolicyName")),
                                PolicyDetails = reader.GetString(reader.GetOrdinal("PolicyDetails")),
                                PremiumAmount = reader.GetDecimal(reader.GetOrdinal("PremiumAmount")),
                                ClientId = reader.IsDBNull(reader.GetOrdinal("ClientId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ClientId"))
                            };
                        }
                        else
                        {
                            return null; // Return null if no policy is found
                        }
                    }
                }
            }
        }


        public IEnumerable<Policy> GetAllPolicies()
        {
            var policies = new List<Policy>();
            using (var connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Policies"; // This selects all columns
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var policy = new Policy
                            {
                                PolicyId = reader.GetInt32(reader.GetOrdinal("policyId")),
                                PolicyName = reader.GetString(reader.GetOrdinal("policyName")), 
                                PolicyDetails = reader.GetString(reader.GetOrdinal("policyDetails")), 
                                PremiumAmount = reader.GetDecimal(reader.GetOrdinal("premiumAmount")),
                                ClientId = reader.IsDBNull(reader.GetOrdinal("clientId")) ? 0 : reader.GetInt32(reader.GetOrdinal("clientId")) //handles null

                            };
                            policies.Add(policy);
                        }
                    }
                }
            }
            return policies;
        }


        public bool UpdatePolicy(Policy policy)
        {
            using (var connection = DBConnUtil.GetConnection())
            {
                
                string query = "UPDATE Policies SET PolicyName = @PolicyName, PolicyDetails = @PolicyDetails, PremiumAmount = @PremiumAmount WHERE PolicyId = @PolicyId";

                using (var command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@PolicyName", policy.PolicyName);
                    command.Parameters.AddWithValue("@PolicyDetails", policy.PolicyDetails);
                    command.Parameters.AddWithValue("@PremiumAmount", policy.PremiumAmount); 
                    command.Parameters.AddWithValue("@PolicyId", policy.PolicyId);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0; // Returns true if the policy was updated successfully
                }
            }
        }


        public bool DeletePolicy(int policyId)
        {
            using (var connection = DBConnUtil.GetConnection())
            {
                string query = "DELETE FROM Policies WHERE PolicyId = @PolicyId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PolicyId", policyId);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0; // Returns true if the policy was deleted successfully
                }
            }
        }
        public User Login(string username, string password)
        {
            using (var connection = DBConnUtil.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE username = @Username AND password = @Password";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); 

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Role = reader.GetString(reader.GetOrdinal("role"))
                            };
                        }
                        else
                        {
                            return null; // for Invalid credentials entry , returns null
                        }
                    }
                }
            }
        }

        public bool ClaimPolicy(int policyId, int clientId)
        {
            // Getting the policy details to check the clientId
            var policy = GetPolicy(policyId);
            if (policy == null)
            {
                throw new PolicyNotFoundException($"Policy with ID {policyId} not found.");
            }

            //  Check if the clientId matches the one associated with the policy
            if (policy.ClientId != clientId)
            {
                throw new InvalidOperationException("Client ID does not match the policy's client.");
            }

            // inserting the claim
            using (var connection = DBConnUtil.GetConnection())
            {
                string query = "INSERT INTO Claims (ClaimNumber, DateFiled, ClaimAmount, Status, PolicyId, ClientId) VALUES (@ClaimNumber, @DateFiled, @ClaimAmount, @Status, @PolicyId, @ClientId)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClaimNumber", Guid.NewGuid().ToString()); // to ghenerate a unique claim number
                    command.Parameters.AddWithValue("@DateFiled", DateTime.Now); // Current date
                    command.Parameters.AddWithValue("@ClaimAmount", 0); 
                    command.Parameters.AddWithValue("@Status", "Pending"); // Initial status
                    command.Parameters.AddWithValue("@PolicyId", policyId); // Policy ID
                    command.Parameters.AddWithValue("@ClientId", clientId); // Client ID

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public bool AddClient(Client client)
        {
            using (var connection = DBConnUtil.GetConnection())
            {
                string query = "INSERT INTO Clients (ClientName, ContactInfo) VALUES (@ClientName, @ContactInfo)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientName", client.ClientName);
                    command.Parameters.AddWithValue("@ContactInfo", client.ContactInfo);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0; // Return true if the client was added successfully
                }
            }
        }

    }
}



           