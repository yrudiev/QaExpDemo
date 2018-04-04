using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace QaExp.Common
{
    public static class RestHelper
    {
        public static Dictionary<string, string> GetRequestFromBody(Stream inputStream)
        {
            Dictionary<string, string> request;
            using (var reader = new StreamReader(inputStream))
            {
                request = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
            }

            return request;
        }
    }
}
