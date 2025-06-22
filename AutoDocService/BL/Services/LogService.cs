using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using AutoDocService.API.ServiceInterfaces;
using AutoDocService.Helpers.BindingEntities;

namespace AutoDocService.BL.Services
{
    /// <summary>
    /// The log service is used to call the log service in charge of writing to the Tracelog table
    /// </summary>
    public class LogService : ILogService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// Inicijalizacija Log servisa
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public LogService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;


        }
        /// <summary>
        /// Insert Data into TraceLog
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task<Int64> InsertLog(Log logger)
        {
            if (String.IsNullOrEmpty(logger.HostName))
                logger.HostName = Dns.GetHostName();
            if (String.IsNullOrEmpty(logger.UserName))
                logger.UserName = "UNKNOWN";
            if (String.IsNullOrEmpty(logger.Service))
                logger.Service = "UNKNOWN";
            if (String.IsNullOrEmpty(logger.Action))
                logger.Action = "UNKNOWN";
            if (String.IsNullOrEmpty(logger.Parameters))
                logger.Parameters = "UNKNOWN";
            if (logger.ModifyDate == default(DateTime))
                logger.ModifyDate = DateTime.Now;
            if (String.IsNullOrEmpty(logger.Response))
                logger.Response = "UNKNOWN";
            if (String.IsNullOrEmpty(logger.ErrorCode))
                logger.ErrorCode = "0";
            if (String.IsNullOrEmpty(logger.ExceptionAt))
                logger.ExceptionAt = "UNKNOWN";

            if (logger.Parameters?.Length >= 3000)
                logger.Parameters = logger.Parameters.Substring(0, 3000);

            HttpClient client1 = _httpClientFactory.CreateClient("Log");

            dynamic jsonObject = new JObject();
            jsonObject.Add("logger", JObject.Parse(JsonConvert.SerializeObject(logger)));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client1.BaseAddress.ToString() + "api/Payment/Log/");
            string data = JsonConvert.SerializeObject(jsonObject);

            request.Content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            StringContent theContent = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseID = client1.PostAsync(client1.BaseAddress.ToString() + "api/Payment/Log/", theContent).Result;

            Int64 ID = 0;
            if (responseID.IsSuccessStatusCode)
            {
                string dataObjects = responseID.Content.ReadAsStringAsync().Result;
                ID = JsonConvert.DeserializeObject<Int64>(dataObjects);
            }

            return ID;
        }

        /// <summary>
        /// CreateErrorLog
        /// </summary>
        /// <param name="Service"></param>
        /// <param name="UserName"></param>
        /// <param name="Action"></param>
        /// <param name="ExceptionAt"></param>
        /// <param name="Response"></param>
        /// <param name="IsError"></param>
        /// <param name="ErrorCode"></param>
        /// <param name="Parameters"></param>
        /// <param name="HostName"></param>
        /// <returns></returns>
        public async Task<Int64> CreateErrorLog(string Service, string UserName, string Action, string ExceptionAt, string Response, bool IsError, string ErrorCode = "", string Parameters = "", string HostName = "")
        {
            Log log = new Log
            {
                Service = Service,
                UserName = UserName,
                ExceptionAt = ExceptionAt,
                Action = Action,
                Response = Response,
                IsError = IsError,
                ErrorCode = ErrorCode,
                Parameters = Parameters,
                HostName = HostName
            };
            return await InsertLog(log);
        }

        /// <summary>
        /// Prepares data for CreateErrorLog
        /// </summary>
        /// <param name="exceptionAt"></param>
        /// <param name="ex"></param>
        /// <param name="parameter"></param>
        /// <returns>Tracelog ID</returns>
        public async Task<Int64> LogException(string exceptionAt, Exception ex, string parameter = "")
        {
            string additionalInfo = ex.InnerException != null ? " " + parameter + " " + " Inner Exception: " + ex.InnerException.Message : " " + parameter + " " + "";
            return await CreateErrorLog(Assembly.GetExecutingAssembly().GetName().Name, "OCP", exceptionAt.Split('.').LastOrDefault(), exceptionAt, ex.Message + additionalInfo, false, "OCP", ex.StackTrace);
        }
    }
}