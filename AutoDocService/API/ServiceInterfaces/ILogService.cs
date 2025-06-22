using AutoDocService.Helpers.BindingEntities;

namespace AutoDocService.API.ServiceInterfaces
{
    /// <summary>
    /// Interface of Log service
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Interface of InsertLog method
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        Task<Int64> InsertLog(Log logger);
        /// <summary>
        /// Interface og CreateErrorLog method
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
        Task<Int64> CreateErrorLog(string Service, string UserName, string Action, string ExceptionAt, string Response, bool IsError, string ErrorCode = "", string Parameters = "", string HostName = "");

        /// <summary>
        /// Interface of LogException method
        /// </summary>
        /// <param name="exceptionAt"></param>
        /// <param name="ex"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<Int64> LogException(string exceptionAt, Exception ex, string parameter = "");
    }
}
