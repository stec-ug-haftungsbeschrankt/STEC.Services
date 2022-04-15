using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace STEC.Services.Healthchecks
{
    public class HealthCheck : IHealthCheck
    {
        private HealthCheckOptions _options;

        private ILogger _logger;

        public HealthCheck(IOptions<HealthCheckOptions> options, ILogger logger)
        {
            if (options != null)
            {
                _options = options.Value;
            }
            _logger = logger;
        }

        public async Task<Result> PingHealthCheck(Uri url)
        {
            try
            {
                if (url == null)
                {
                    throw new ArgumentNullException(nameof(url));
                }

                using (Ping pingSender = new Ping())
                {
                    var response = await pingSender.SendPingAsync(url.ToString()).ConfigureAwait(false);

                    if (response.Status == IPStatus.Success)
                    {
                        return Result.Success;
                    }
                    return Result.Fail;
                }
            }
            catch (PingException exception)
            {
                _logger.LogWarning("{Method}: {Url} - Message :{Message}", MethodBase.GetCurrentMethod().Name, url, exception.Message);
                return Result.Fail;
            }
            catch (SocketException exception)
            {
                _logger.LogWarning("{Method}: {Url} - Message :{Message}", MethodBase.GetCurrentMethod().Name, url, exception.Message);
                return Result.Fail;
            }
        }


        public async Task<Result> HttpGetHealthCheck(Uri url)
        {
            try
            {
                if (url == null)
                {
                    throw new ArgumentNullException(nameof(url));
                }

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return Result.Success;
                    }
                    return Result.Fail;
                }
            }
            catch(HttpRequestException exception)
            {
                _logger.LogWarning("{Method}: {Url} - Message: {Message}", MethodInfo.GetCurrentMethod().Name, url, exception.Message);
                return Result.Fail;
            }
        }
    }
}