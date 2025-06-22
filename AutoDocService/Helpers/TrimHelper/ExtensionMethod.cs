using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;

namespace AutoDocService.Helpers.TrimHelper
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// Trims all string parameters in a list with classes
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>

        public static List<TSelf> StringTrimer5000<TSelf>(this List<TSelf> input)
        {
            List<TSelf> TrimObj = new List<TSelf>();
            foreach (var obj in input)
            {
                var stringProperties = obj.GetType().GetProperties();

                foreach (var stringProperty in stringProperties)
                {
                    if (stringProperty.PropertyType == typeof(string))
                    {
                        string currentValue = (string)stringProperty.GetValue(obj, null);
                        if (currentValue != null)
                        {
                            string val = currentValue.Trim();

                            stringProperty.SetValue(obj, Regex.Replace(val, " {2,}", " "), null);
                        }
                    }
                }
                TrimObj.Add(obj);
            }
            return TrimObj;
        }

        /// <summary>
        /// Checks if boject type i of type list
        /// </summary>
        /// <param name="o"></param>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        /// <summary>
        /// Trims all string within a clas with classes in it
        /// </summary>
        /// <param name="obj"></param>
        ///  <remarks>
        /// </remarks>
        /// <returns></returns>
        public static TSelf StringTrimer5000<TSelf>(this TSelf obj)
        {
            if (IsList(obj))
            {
                return obj;
            };

            //var stringProperties = obj.GetType().GetProperties();
            if (obj == null)
                return obj;

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            foreach (PropertyInfo p in obj.GetType().GetProperties(flags))
            {
                Type currentNodeType = p.PropertyType;
                if (currentNodeType == typeof(String))
                {
                    string currentValue = (string)p.GetValue(obj, null);
                    if (currentValue != null)
                    {
                        string val = currentValue.Trim();
                        p.SetValue(obj, Regex.Replace(val, " {2,}", " "), null);
                    }
                }
                else if (currentNodeType != typeof(object) && Type.GetTypeCode(currentNodeType) == TypeCode.Object)
                {
                    p.GetValue(obj, null).StringTrimer5000();
                }
            }

            return obj;
        }
    }
}
