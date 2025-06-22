namespace AutoDocService.Helpers.BindingEntities
{
    /// <summary>
    /// Class corresponding to the Tracelog table
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Id
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Hostname
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Servise
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// Modification date
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Response
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// TotalDuration
        /// </summary>
        public Int64 TotalDuration { get; set; }

        /// <summary>
        /// Bool to indicate if it is an error
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// ExceptionAt
        /// </summary>
        public string ExceptionAt { get; set; }
    }
}
