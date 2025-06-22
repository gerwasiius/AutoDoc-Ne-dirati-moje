namespace AutoDocService.Helpers.BindingEntities
{
    public class RBBHException : Exception
    {
        public static string _errCode;
        public string ErrCode { get { return _errCode; } }

        //    get { return primitiveEntityColumnName; }
        // set { primitiveEntityColumnName = value; }

        public static string _thrownMessage;
        public string ThrownMessage { get { return _thrownMessage; } }

        public static string _exceptionAt;
        public string ExceptionAt { get { return _exceptionAt; } }

        public static string _innerException;
        public string InnerException
        {
            get { return _exceptionAt; }
            set { _innerException = value; }
        }


        public RBBHException(string message, Exception innerException, string errCode = "0", string thrownMessage = "", string exceptionAt = "") : base(message, innerException)
        {
            //MyBase.New(message, innerException);
            //base = new Exception(message, innerException);
            RBBHException._errCode = errCode;
            RBBHException._thrownMessage = thrownMessage;
            RBBHException._exceptionAt = exceptionAt;
            if (String.IsNullOrEmpty(RBBHException._innerException))
                RBBHException._innerException = "No Inner exception provided";
            else
                RBBHException._innerException = innerException.ToString();
        }
    }
}
