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
            await Get(new Uri(url), username, password).ConfigureAwait(false);
        }

        public async Task Get(Uri url, string username, string password)
        {
            using (var client = HttpClientCreator.GetClient(username, password))
            {
                await ExecuteGetRequest(client, url).ConfigureAwait(false);
            };
        }

        public async Task Get(string url, string token)
        {
            await Get(new Uri(url), token).ConfigureAwait(false);
        }

        public async Task Get(Uri url, string token)
        {
            using (var client = HttpClientCreator.GetClient(token))
            {
                await ExecuteGetRequest(client, url).ConfigureAwait(false);
            };
        }

        public async Task Get(Uri url)
        {
            using (var client = HttpClientCreator.GetClient())
            {
                await ExecuteGetRequest(client, url).ConfigureAwait(false);
            };
        }

        public async Task Get(string url)
        {
            await Get(new Uri(url)).ConfigureAwait(false);
        }

        private async Task ExecuteGetRequest(HttpClient client, Uri url)
        {
            try
            {
                client.BaseAddress = url;

                var response = await client.GetAsync("").ConfigureAwait(false);

                StatusCode = response.StatusCode;
                Headers = response.Headers;
                Content = response.Content;
            }
            catch (HttpRequestException e)
            {
                Success = false;
                _logger.LogError("Request exception: {Message}", e.Message);
                _logger.LogError("{Message}", e.InnerException.Message);
            }
        }


        public async Task Post(string url, Dictionary<string, string> content, string authentication = null)
        {
            using var client = new HttpClient();

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new FormUrlEncodedContent(content);

                var response = await client.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                StatusCode = response.StatusCode;
                Headers = response.Headers;
                Content = response.Content;
            }
            catch (HttpRequestException e)
            {
                Success = false;
                _logger.LogError("Request exception: {Message}", e.Message);
                if (e.InnerException != null)
                {
                    _logger.LogError("{Message}", e.InnerException.Message);
                }
            }
        }
    }

}