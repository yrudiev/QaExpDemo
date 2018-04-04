using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace QaExp.Common
{
    public static class ValidationHelper
    {
        public static bool IsValidJson(string json)
        {
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }

        public static bool IsValidInt(string intValue)
        {
            return int.TryParse(intValue, out _);
        }

        public static bool IsValidMail(string email, int maxLength = -1)
        {
            if (!new Regex(@"^[A-Za-z0-9]+@[a-z]+\.[a-z]+").Match(email).Success)
                return false;
            if (maxLength > -1)
                return email.Length < maxLength;

            return true;
        }

        public static bool IsValidCurrencyCode(string currencyCode)
        {
            return new Regex(@"^[A-Z]{3}").Match(currencyCode).Success;
        }

        public static bool DoesStringHaveSpaces(string stringValue)
        {
            return stringValue.Replace(" ", "").Length != stringValue.Length;
        }

        public static bool IsStringMatchRegex(string value, string regexPattern)
        {
            return new Regex(regexPattern).Match(value).Success;
        }
    }
}
