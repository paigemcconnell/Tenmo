using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using RestSharp;
using RestSharp.Authenticators;
using TenmoClient;

namespace TenmoClient.APIClients
{
    public class TransferRestClient
    {
        private readonly RestClient client;

        public TransferRestClient()
        {
            this.client = new RestClient("https://localhost:44315/");
        }
        private string token;
        public void UpdateToken(string jwt)
        {
            token = jwt;
            if (jwt == null)
            {
                client.Authenticator = null;
            }
            else
            {
                client.Authenticator = new JwtAuthenticator(jwt);  // This line ensures that any request to this client includes this jwt
            }
            
        }
        
        public AccountBalance DisplayBalance()
        {
            RestRequest request = new RestRequest("transfer/balance");
            //request.AddHeader("Authorization", "Bearer " + token); - we don't need this line because line 29 already do this
            IRestResponse<AccountBalance> response = client.Get<AccountBalance>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not connect to the database");

                return null;
            }

            if (!response.IsSuccessful)
            {
                Console.WriteLine("Problem getting balance: " + response.StatusDescription);
                Console.WriteLine(response.Content);

                return null;
            }

            return response.Data;

        } 
        public List<API_User> DisplayUsers()
        {
            RestRequest request = new RestRequest("transfer/users");

            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not connect to the database");

                return null;
            }

            if (!response.IsSuccessful)
            {
                Console.WriteLine("Problem getting users: " + response.StatusDescription);
                Console.WriteLine(response.Content);

                return null;
            }

            return response.Data;

        }
        
        public Transfer SendFunds(Transfer transfer)
        {
            RestRequest request = new RestRequest("transfer/sendfunds");

            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Post<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not connect to the database");

                return null;
            }

            if (!response.IsSuccessful)
            {
                Console.WriteLine("Problem getting transfer: " + response.StatusDescription);
                //Console.WriteLine(response.Content);

                return null;
            }

            return response.Data;

        }

    }
}
