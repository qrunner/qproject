using System;
using System.Text.RegularExpressions;

namespace Provider.Messaging.Tibco
{
    [Obsolete("Вынести базовый функционал в отдельную сборку")]
    public class TibcoConnectionString
    {
        private const string nServerUrl = "server";
        private const string nQueue = "queue";
        private const string nUser = "user";
        private const string nPassword = "password";
        private const string nAllStringValidation = @"^([^=;]+=[^=;]+)(;[^=;]+=[^=;]+)*\;?$";
        private const string nSearchParamPattern = @"({0}=[^=;]+)";

        protected string connectionString;

        public TibcoConnectionString(string cs)
        {
            connectionString = cs;
        }

        public void CheckCSFormat()
        {
            Regex regex = new Regex(nAllStringValidation);
            if (!regex.IsMatch(connectionString)) throw new FormatException("Строка подключения имеет неверный формат.");
        }

        protected string GetParamValue(string paramName)
        {
            Regex regex = new Regex(string.Format(nSearchParamPattern, paramName));
            foreach (Match match in regex.Matches(connectionString))
                return match.Value.Substring(match.Value.IndexOf("=") + 1);
            return null;
        }

        protected void SetParamValue(string paramName, string value)
        {
            Regex regex = new Regex(string.Format(nSearchParamPattern, paramName));
            if (regex.IsMatch(connectionString))
                connectionString = regex.Replace(connectionString, (x) => string.Format("{0}={1}", paramName, value));
            else
                connectionString += (string.Format("{2}{0}={1}", paramName, value, connectionString.EndsWith(";") ? null : ";"));
        }

        public string ServerUrl { get { return GetParamValue(nServerUrl); } }
        public string QueueName { get { return GetParamValue(nQueue); } }
        public string User { get { return GetParamValue(nUser); } }
        public string Password { get { return GetParamValue(nPassword); } }
    }
}