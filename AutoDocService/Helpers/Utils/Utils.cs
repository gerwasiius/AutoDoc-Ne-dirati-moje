using Newtonsoft.Json.Linq;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoDocService.Helpers.Utils
{
    public static class Utils
    {
        private static IHttpClientFactory? _httpClientFactory;
       
        public static string GetMethodAndClassName(MethodBase methodbase)
        {
            MethodBase mb = GetRealMethodFromAsyncMethod(methodbase);
            var fullName = string.Format("{0}.{1}", mb.ReflectedType.FullName, mb.Name);
            //,string.Join(",", mb.GetParameters().Select(s => string.Format("{0} {1}", s.ParameterType, s.Name)).ToArray()));
            return fullName;
        }

        private static MethodBase GetRealMethodFromAsyncMethod(MethodBase asyncMethod)
        {
            var generatedType = asyncMethod.DeclaringType;
            var originalType = generatedType.DeclaringType;
            var matchingMethods =
                from methodInfo in originalType.GetMethods()
                let attr = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>()
                where attr != null && attr.StateMachineType == generatedType
                select methodInfo;

            // If this throws, the async method scanning failed.
            var foundMethod = matchingMethods.Single();
            return foundMethod;
        }

        // '' <summary>
        // '' File Extensions
        // '' </summary>
        // '' <remarks></remarks>
        public class Extensions
        {
            public const string XLS = ".xls";

            public const string XLSX = ".xlsx";

            public const string TXT = ".txt";

            public const string OLD = ".OLD";

            public const string XML = ".xml";

            public const string L_L = ".l_l";

            public const string DAT = ".dat";
        }


        public static List<string> ParseSingleColumnDataReader(DbDataReader dr)
        {
            List<string> retVal = new List<string>();
            int i = 0;
            while (dr.Read())
            {
                retVal.Add(dr.GetValue(i++).ToString());
            }
            dr.Close();
            return retVal;
        }


        internal static void InitEmptyObject(object obj)
        {
            foreach (var prop in obj.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanWrite))
            {
                var type = prop.PropertyType;
                var constr = type.GetConstructor(Type.EmptyTypes); //find constructor without parameters
                if (type.IsClass && constr != null)
                {
                    var propInst = Activator.CreateInstance(type);  //activator nece da kreira listu objekata treba hendlat u iznad ako je se oceku li ta objekata
                    prop.SetValue(obj, propInst, null);
                    if (!type.IsGenericType)
                    {
                        InitEmptyObject(propInst);
                    }
                }
            }
        }

        internal static string SpaceIfNothing(object InPar)
        {
            if (InPar == null)
                return "";
            if (InPar.GetType() == typeof(string))
                InPar = InPar.ToString().Replace("'", "''");
            return InPar.ToString();
        }

        public static bool Language(HttpContext Request)
        {
            var acceptLanguages = Request.Request.Headers["Accept-Language"];
            foreach (var acceptLanguageSet in acceptLanguages)
            {
                foreach (var acceptLanguage in acceptLanguageSet.Split(';'))
                {
                    if (acceptLanguage.Contains("en"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static JObject GetJObjectErr(string Key, string Description, string DescriptionEN, bool language)
        {
            if (language)
            {
                return new JObject() {
                 new JProperty("Key",Key),
                        new JProperty("Description",DescriptionEN),
                     //   new JProperty("DescriptionEN",DescriptionEN)
                };
            }
            else
            {
                return new JObject() {
                 new JProperty("Key",Key),
                        new JProperty("Description",Description),
                      //  new JProperty("DescriptionEN",DescriptionEN)
                };
            }
        }

        public static object DBNullIfNothing(object InPar)
        {
            if (InPar == null)
                InPar = DBNull.Value;
            return InPar;
        }

        public static AsyncRetryPolicy<HttpResponseMessage> SetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                });
        }

        public static AsyncCircuitBreakerPolicy<HttpResponseMessage> SetBreakCircutPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30), OnBreak, OnReset, OnHalfOpen);
        }

        private static void OnHalfOpen()
        {
            Console.WriteLine("Circuit in test mode, one request will be allowed.");
        }

        private static void OnReset()
        {
            Console.WriteLine("Circuit closed, requests flow normally.");
        }

        private static void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan ts)
        {
            Console.WriteLine("Circuit cut, requests will not flow.");
        }

        public static string GetMethodAndClassName1(MethodBase methodbase)
        {
            //MethodBase mb = GetRealMethodFromAsyncMethod(methodbase);
            //var fullName = string.Format("{0}.{1}", mb.ReflectedType.FullName, mb.Name);
            //,string.Join(",", mb.GetParameters().Select(s => string.Format("{0} {1}", s.ParameterType, s.Name)).ToArray()));
            var fullName = GetMethodContextName(methodbase);
            return fullName;
        }

        static string GetMethodContextName(this MethodBase method)
        {
            if (method.DeclaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
            {
                var generatedType = method.DeclaringType;
                var originalType = generatedType.DeclaringType;
                var foundMethod = originalType.GetMethods()
                    .Single(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
                return foundMethod.DeclaringType.Name + "." + foundMethod.Name;
            }
            else
            {
                return method.DeclaringType.Name + "." + method.Name;
            }
        }
    }
}
