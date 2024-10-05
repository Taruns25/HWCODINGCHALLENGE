using insurance_management.dao;
using insurance_management.Models;
using insurance_management.Exceptions;
using System;

namespace insurance_management.main
{
    public class MainModule
    {
        public void Run()
        {
            var policyService = new InsuranceServiceImpl();
            User loggedInUser = null;

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Insurance Management System");
                if (loggedInUser == null)
                {
                    Console.WriteLine("1. User Login");
                    Console.WriteLine("2. Exit");
                }
                else
                {
                    Console.WriteLine("1. Create Client");
                    Console.WriteLine("2. Create Policy");
                    Console.WriteLine("3. Get Policy");
                    Console.WriteLine("4. Get All Policies");
                    Console.WriteLine("5. Update Policy");
                    Console.WriteLine("6. Delete Policy");
                    Console.WriteLine("7. Claim for a Policy");
                    Console.WriteLine("8. Logout");
                }

                Console.Write("Enter choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                try
                {
                    if (loggedInUser == null)
                    {
                        switch (choice)
                        {
                            case 1:
                                Console.Write("Enter username: ");
                                string username = Console.ReadLine();
                                Console.Write("Enter password: ");
                                string password = Console.ReadLine();
                                loggedInUser = policyService.Login(username, password);

                                if (loggedInUser != null)
                                {
                                    Console.WriteLine($"Welcome, {loggedInUser.Username}! You are logged in as {loggedInUser.Role}.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid username or password.");
                                }
                                break;

                            case 2:
                                exit = true; // Exit
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    else
                    {
                        switch (choice)
                        {
                            case 1: // Add Client
                                Console.Write("Enter client name: ");
                                string clientName = Console.ReadLine();
                                Console.Write("Enter client contact details: ");
                                string contactInfo = Console.ReadLine();

                                var newClient = new Client
                                {
                                    ClientName = clientName,
                                    ContactInfo = contactInfo
                                };

                                bool isClientAdded = policyService.AddClient(newClient);
                                if (isClientAdded)
                                {
                                    Console.WriteLine("Client added successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to add the client.");
                                }
                                break;


                            case 2: // Create Policy
                                Console.Write("Enter policy name: ");
                                string policyName = Console.ReadLine();

                                Console.Write("Enter policy details: "); // Prompt for policy details
                                string policyDetails = Console.ReadLine();

                                Console.Write("Enter premium amount: "); // Prompt for premium amount
                                decimal premiumAmount;
                                while (!decimal.TryParse(Console.ReadLine(), out premiumAmount)) 
                                {
                                    Console.Write("Invalid input. Please enter a valid premium amount: ");
                                }

                                Console.Write("Enter client ID: "); 
                                int clientId;
                                while (!int.TryParse(Console.ReadLine(), out clientId))
                                {
                                    Console.Write("Invalid input. Please enter a valid Client ID: ");
                                }

                                // Create a policy object with all required fields
                                var newPolicy = new Policy
                                {
                                    PolicyName = policyName,
                                    PolicyDetails = policyDetails,
                                    PremiumAmount = premiumAmount,
                                    ClientId = clientId 
                                };

                                bool isCreated = policyService.CreatePolicy(newPolicy); 
                                if (isCreated)
                                {
                                    Console.WriteLine("Policy created successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to create the policy.");
                                }
                                break;



                            case 3:
                                Console.Write("Enter policy ID: ");
                                int policyId = Convert.ToInt32(Console.ReadLine());
                                var policy = policyService.GetPolicy(policyId);
                                if (policy == null)
                                {
                                    throw new PolicyNotFoundException("Policy not found");
                                }
                                Console.WriteLine($"Policy: {policy}");
                                break;

                            case 4:
                                var policies = policyService.GetAllPolicies();
                                foreach (var p in policies)
                                {
                                    Console.WriteLine(p);
                                }
                                break;

                            case 5:
                                Console.Write("Enter policy ID to update: ");
                                int updatePolicyId = Convert.ToInt32(Console.ReadLine());
                                var existingPolicy = policyService.GetPolicy(updatePolicyId);

                                if (existingPolicy == null)
                                {
                                    Console.WriteLine($"Policy with ID {updatePolicyId} not found.");
                                    break;
                                }

                                // Update policy name
                                Console.Write($"Enter new policy name");
                                existingPolicy.PolicyName = Console.ReadLine();

                                // Update policy details
                                Console.Write($"Enter new policy details ");
                                existingPolicy.PolicyDetails = Console.ReadLine();

                                // Update premium amount
                                Console.Write($"Enter new premium amount ");
                                existingPolicy.PremiumAmount = Convert.ToDecimal(Console.ReadLine());

                                bool isUpdated = policyService.UpdatePolicy(existingPolicy);
                                if (isUpdated)
                                {
                                    Console.WriteLine("Policy updated successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to update the policy.");
                                }
                                break;


                            case 6:
                                Console.Write("Enter policy ID to delete: ");
                                int delPolicyId = Convert.ToInt32(Console.ReadLine());
                                var policyToDelete = policyService.GetPolicy(delPolicyId);
                                if (policyToDelete == null)
                                {
                                    throw new PolicyNotFoundException($"Policy with ID {delPolicyId} not found.");
                                }
                                policyService.DeletePolicy(delPolicyId);
                                Console.WriteLine("Policy deleted successfully!");
                                break;

                            case 7:
                                Console.Write("Enter policy ID to claim: ");
                                int claimPolicyId = Convert.ToInt32(Console.ReadLine());

                                Console.Write("Enter client ID: ");
                                int clientIdd = Convert.ToInt32(Console.ReadLine());

                                Console.Write("Enter claim amount: ");
                                decimal claimAmount = Convert.ToDecimal(Console.ReadLine());

                                bool isClaimed = policyService.ClaimPolicy(claimPolicyId, clientIdd);
                                if (isClaimed)
                                {
                                    Console.WriteLine("Claim submitted successfully with status: Registered!");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to submit the claim.");
                                }
                                break;

                            case 8:
                                loggedInUser = null; // Logout
                                Console.WriteLine("You have logged out.");
                                break;

                            default:
                                Console.WriteLine("Invalid choice.");
                                break;
                        }
                    }
                }
                catch (PolicyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
