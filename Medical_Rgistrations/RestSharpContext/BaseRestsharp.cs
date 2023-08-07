using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading;

namespace Medical_Rgistrations.RestSharpContext
{
    public class RestsharpClient : IDisposable
    {
        private string _baseAddres { get; set; }
        public RestClientOptions _restClientOptions { get; set; }
        public HttpBasicAuthenticator _authenticator { get; set; }
        public RestRequest _request { get; set; }
        public bool isAuthorized { get; set; } = false;
        public string token { get; set; }

        public string username { get; set; }
        public string password { get; set; }


        public RestsharpClient(string baseAddress)
        {
            this._baseAddres = baseAddress;
            _restClientOptions = new RestClientOptions(_baseAddres);


        }


        public void SetBasicAuthenticator(string username, string password)
        {
            try
            {
                this.username = username;
                this.password = password;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> GetToken()
        {
            if (!this.isAuthorized)
            {

                var options = new RestClientOptions(this._baseAddres)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(this._baseAddres+"/Authentication/login", Method.Post);
                var body = JsonConvert.SerializeObject(new { username = username, password = password });

                request.AddHeader("Content-Type", "application/json");

                request.AddStringBody(body, DataFormat.Json);
                request.RequestFormat = DataFormat.Json;
                RestResponse response = await client.PostAsync(request);


                if (response.IsSuccessStatusCode)
                {
                    JwtToken token = JsonConvert.DeserializeObject<JwtToken>(response.Content);

                    if (token != null)
                    {
                        isAuthorized = true;
                        this.token = token.token;
                    }

                }

            }
            return this.token;
        }


        public async Task<RestClient> GetClientInstance(string resourceUrl)
        {
            if (!this.isAuthorized)
            {
                _request = new RestRequest(resourceUrl);
                this._request.AddHeader("Authorization", $"Bearer {await GetToken()}");
                _request.AddHeader("Content-Type", "application/json");

            }



            var client = new RestClient(_restClientOptions);

            return client;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    class JwtToken
    {
        public string? token { get; set; }
    }
}
