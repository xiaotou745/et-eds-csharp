using System;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;

namespace ETS.Util
{
    public static class AssertUtils
    {
        #region ArgumentNotNull

        /// <summary>
        /// Checks the value of the supplied <paramref name="argument"/> and throws an
        /// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
        /// </summary>
        /// <param name="argument">The object to check.</param>
        /// <param name="name">The argument name.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If the supplied <paramref name="argument"/> is <see langword="null"/>.
        /// </exception>
        public static void ArgumentNotNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(
                    name,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Argument '{0}' cannot be null.", name));
            }
        }

        /// <summary>
        /// Checks the value of the supplied <paramref name="argument"/> and throws an
        /// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
        /// </summary>
        /// <param name="argument">The object to check.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="message">
        /// An arbitrary message that will be passed to any thrown
        /// <see cref="System.ArgumentNullException"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// If the supplied <paramref name="argument"/> is <see langword="null"/>.
        /// </exception>
        public static void ArgumentNotNull(object argument, string name, string message)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        #endregion

        #region StringNotNullOrEmpty

        public static void StringNotNullOrEmpty(string argument, string name)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(
                    name,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "ArgumentString '{0}' cannot be null or empty.", name));
            }
        }

        public static void StringNotNullOrEmpty(string argument, string name, string message)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(name, message);
            }
        }

        #endregion

        #region IsTrue

        /// <summary>
        ///  AssertUtils a boolean expression, throwing <code>ArgumentException</code>
        ///  if the test result is <code>false</code>.
        /// </summary>
        /// <param name="expression">a boolean expression.</param>
        /// <param name="message">The exception message to use if the assertion fails.</param>
        /// <exception cref="ArgumentException">
        /// if expression is <code>false</code>
        /// </exception>
        public static void IsTrue(bool expression, string message)
        {
            if (!expression)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        ///  AssertUtils a boolean expression, throwing <code>ArgumentException</code>
        ///  if the test result is <code>false</code>.
        /// </summary>
        /// <param name="expression">a boolean expression.</param>
        /// <exception cref="ArgumentException">
        /// if expression is <code>false</code>
        /// </exception>
        public static void IsTrue(bool expression)
        {
            IsTrue(expression, "[Assertion failed] - this expression must be true");
        }

        #endregion

        /// <summary>
        /// AssertUtils a bool expression, throwing <code>InvalidOperationException</code>
        /// if the expression is <code>false</code>.
        /// </summary>
        /// <param name="expression">a boolean expression.</param>
        /// <param name="message">The exception message to use if the assertion fails</param>
        /// <exception cref="InvalidOperationException">if expression is <code>false</code></exception>
        public static void State(bool expression, string message)
        {
            if (!expression)
            {
                throw new InvalidOperationException(message);
            }
        }

        #region Greater

        /// <summary>
        /// 判定参数1的值大于参数2的值，如果参数1值小于等于参数2，则抛出异常
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Greater(int arg1, int arg2)
        {
            Greater(arg1, arg2, string.Format("[AsserUtils-Greater] failed with arg1:{0} arg2:{1}, {0}<={1}", arg1, arg2));
        }

        /// <summary>
        /// 判定参数1的值大于参数2的值，如果参数1值小于等于参数2，则抛出异常
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Greater(long arg1, long arg2)
        {
            Greater(arg1, arg2, string.Format("[AsserUtils-Greater] failed with arg1:{0} arg2:{1}, {0}<={1}", arg1, arg2));
        }

        /// <summary>
        /// 判定参数1的值大于参数2的值，如果参数1值小于等于参数2，则抛出异常
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="message"> </param>
        public static void Greater(long arg1, long arg2, string message)
        {
            if (arg1 <= arg2)
            {
                throw new InvalidDataException(message);
            }
        }

        #endregion

       
        public static string GetMacString()
        {
            string strMac = "";
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    strMac += ni.GetPhysicalAddress().ToString() + "|";
                }
            }
            //return strMac.Split('|');
            return strMac;
        }
    }
}