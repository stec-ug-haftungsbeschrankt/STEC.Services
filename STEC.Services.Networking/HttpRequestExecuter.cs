using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace STEC.Services.Networking
{
    public class HttpRequestExecuter
    {
        private readonly ILogger _logger;

        public HttpStatusCode StatusCode { get; private set; }
        public HttpResponseHeaders Headers { get; private set; }
        public HttpContent Content { get; private set; }

        public bool Success { get; private set;} = true;

                public HttpRequestExecuter(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Get(string url, string username, string password)
        {
            using (var client = HttpClientCreator.GetClient(username, password))
            {
                await ExecuteGetRequest(client, url);
            };
        }

        public async Task Get(string url, string token)
        {
            using (var client = HttpClientCreator.GetClient(token))
            {
                await ExecuteGetRequest(client, url);
            };
        }

        public async Task Get(string url)
        {
            using (var client = HttpClientCreator.GetClient())
            {
                await ExecuteGetRequest(client, url);
            };
        }

        private async Task ExecuteGetRequest(HttpClient client, string url)
        {
            try
            {
                client.BaseAddress = new Uri(url);

                var response = await client.GetAsync("").ConfigureAwait(false);

                StatusCode = response.StatusCode;
                Headers = response.Headers;
                Content = response.Content;
            }
            catch (HttpRequestException e)
            {
                Success = false;
                _logger.LogError($"Request exception: {e.Message}");
                _logger.LogError(e.InnerException.Message);
            }
        }


        public async Task Post(string url, Dictionary<string, string> content, string authentication = null)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                    {
                        request.Content = new FormUrlEncodedContent(content);

                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();

                        StatusCode = response.StatusCode;
                        Headers = response.Headers;
                        Content = response.Content;
                    }
                }
                catch (HttpRequestException e)
                {
                    Success = false;
                    _logger.LogError($"Request exception: {e.Message}");
                    if (e.InnerException != null)
                    {
                        _logger.LogError(e.InnerException.Message);
                    }
                }
            }
        }
    }

}