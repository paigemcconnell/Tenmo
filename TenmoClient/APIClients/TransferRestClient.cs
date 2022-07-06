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
                client.Authenticator = new JwtAuthenticator(jwt);
            }
            
        }
        /*
        public AccountBalance DisplayBalance(string username)
        {
            RestRequest request = new RestRequest("balance/" + )
        } 
        */
    }
}
