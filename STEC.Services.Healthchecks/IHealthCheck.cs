using System;
using System.Threading.Tasks;

namespace STEC.Services.Healthchecks
{
    public enum Result
    {
        Success,
        Fail
    }


    public interface IHealthCheck
    {
        Task<Result> PingHealthCheck(Uri url);


        Task<Result> HttpGetHealthCheck(Uri url);

    }
}
