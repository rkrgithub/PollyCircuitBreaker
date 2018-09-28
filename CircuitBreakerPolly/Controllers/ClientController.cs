using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CircuitBreakerPolly.Controllers
{
    public class ClientController : ApiController
    {
        string _baseAddress = "http://localhost:50324/api/products";
        static TimeSpan _durationOfBreak = TimeSpan.FromSeconds(10);
        static CircuitBreakerPolicy _circuitBreaker = CircuitBreakerPolicy
                                .Handle<Exception>()
                                .CircuitBreakerAsync(
                                    exceptionsAllowedBeforeBreaking: 1,
                                    durationOfBreak: _durationOfBreak,
                                    onBreak: (exception, timespan) =>
                                    {
                                        LogMessageToFile("On Break, time span : " + timespan.ToString());
                                    },
                                    onReset: () =>
                                    {
                                        LogMessageToFile("On Reset");
                                    },
                                    onHalfOpen: () =>
                                    {
                                        LogMessageToFile("on HalfOpen");
                                    }
                                );


        [HttpGet]
        public async Task<string> Get()
        {
            LogMessageToFile("------- Calling Get API ------");
            try
            {
                return await _circuitBreaker.ExecuteAsync(async () =>
                {
                    HttpResponseMessage res = await ServiceCall(new Uri($"{_baseAddress}/GetProducts"));
                    return await res.Content?.ReadAsStringAsync();
                });
            }
            catch (BrokenCircuitException)
            {
                return $"Circuit breaker is in OPEN state. Please try again after {_durationOfBreak.Seconds} seconds";
            }
        }

        [HttpGet]
        public async Task<string> GetwithDelay(int delay = 0)
        {
            LogMessageToFile("------- Calling GetwithDelay API with dalay " + delay + " -----");

            try
            {
                return await _circuitBreaker.ExecuteAsync(async () =>
                {
                    HttpResponseMessage res = await ServiceCall(new Uri($"{_baseAddress}/GetdelayedProducts?delay={delay}"));
                    return await res.Content?.ReadAsStringAsync();
                });
            }
            catch (BrokenCircuitException)
            {
                return $"Circuit breaker is in OPEN state. Please try again after {_durationOfBreak.Seconds} seconds";
            }
        }

        [HttpGet]
        public async Task<string> GetException()
        {
            LogMessageToFile("------- Calling GetException -----");
            try
            {
                return await _circuitBreaker.ExecuteAsync(
                    action: async (Context, CancellationToken) => await GetExceptionFromServiceCall(),
                    context: new Context("a", new Dictionary<string, object> { { "b", "c" } }),
                    cancellationToken: CancellationToken.None,
                    continueOnCapturedContext: true
                    );
            }
            catch (BrokenCircuitException)
            {
                return $"Circuit breaker is in OPEN state. Please try again after {_durationOfBreak.Seconds} seconds";
            }
            catch (Exception)
            {
                return $"My custom exception from GetException method";
            }
        }

        private async Task<string> GetExceptionFromServiceCall()
        {
            var res = await ServiceCall(new Uri($"{_baseAddress}/GetException"));
            return await res.Content?.ReadAsStringAsync();
        }

        private static async Task<HttpResponseMessage> ServiceCall(Uri uri)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(5)
                })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    LogMessageToFile("Executing Http Call");
                    response = await client.GetAsync(uri);
                    if (!response.IsSuccessStatusCode)
                    {
                        return new HttpResponseMessage(response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        /// <summary>
        /// For logging to a file - created in same project folder
        /// </summary>
        /// <param name="msg"></param>
        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                 AppDomain.CurrentDomain.BaseDirectory + "Clientlog.log");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }

    }
}
