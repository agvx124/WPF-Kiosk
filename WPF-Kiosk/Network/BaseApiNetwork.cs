using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WPF_Kiosk.Model;

namespace WPF_Kiosk.Network
{
    public class BaseApiNetwork
    {
        readonly string HOST = "http://10.80.163.88:3000/v1/";
        RestClient client;
        RestRequest request;

        public BaseApiNetwork()
        {
            client = new RestClient(HOST);
        }

        public string PostLogin(string id, string pw)
        {
            request = new RestRequest("auth/login", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();

            User user = new User();
            user.id = id;
            user.pw = pw;

            request.AddJsonBody(user);

            var queryResult = client.Execute(request);
            var response = queryResult.Content;

            return response;
        }
    }
}
