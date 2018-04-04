using Newtonsoft.Json;
using QaExp.Common;
using QaExp.Host.Behavior;
using System.Collections.Generic;
using System.Web;

namespace QaExp.Host.Handlers
{
    public class ProviderFake : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = RestHelper.GetRequestFromBody(context.Request.InputStream);
            
            if (request == null)
                context.Response.Write("Dictionary is empty");

            var error = string.Empty;

            var action = string.Empty;
            if (request.ContainsKey("action")) action = request["action"];
            if (string.IsNullOrEmpty(action) || !ValidationHelper.IsStringMatchRegex(action, "^[a-z]{1,8}$"))
                error += $"action param: {action} violates contract. ";

            string paymentId = string.Empty;
            if (request.ContainsKey("paymentId")) paymentId = request["paymentId"];
            if (string.IsNullOrEmpty(paymentId) || !ValidationHelper.IsStringMatchRegex(paymentId, "^[a-zA-Z0-9]{1,15}$"))
                error += $"paymentId param: '{paymentId}' violates contract. ";

            string amount = string.Empty;
            if (request.ContainsKey("amount")) amount = request["amount"];
            if (string.IsNullOrEmpty(amount) || !ValidationHelper.IsStringMatchRegex(amount, @"^[0-9]+\.[0-9]{0,2}$"))
                error += $"amount param: '{amount}' violates contract. ";

            string currency = string.Empty;
            if (request.ContainsKey("currency")) currency = request["currency"];
            if (string.IsNullOrEmpty(currency) || !ValidationHelper.IsValidCurrencyCode(currency))
                error += $"currency param: '{currency}' violates contract. ";

            string accountId = string.Empty;
            if (request.ContainsKey("accountId")) accountId = request["accountId"];
            if (string.IsNullOrEmpty(accountId) || !ValidationHelper.IsStringMatchRegex(accountId, @"^[0-9]{1,10}$"))
                error += $"accountId param: '{accountId}' violates contract. ";

            string merchantId = string.Empty;
            if (request.ContainsKey("merchantId")) merchantId = request["merchantId"];
            if (string.IsNullOrEmpty(merchantId) || !ValidationHelper.IsStringMatchRegex(merchantId, @"^[0-9]{1,20}$"))
                error += $"merchantId param: {merchantId ?? "null"} violates contract. ";

            string email = string.Empty;
            if (request.ContainsKey("email")) email = request["email"];
            if (!string.IsNullOrEmpty(email) && !ValidationHelper.IsValidMail(email))
                error += $"email param: {email ?? "null"} violates contract. ";

            string fullName = string.Empty;
            if (request.ContainsKey("fullName")) fullName = request["fullName"];
            if (!string.IsNullOrEmpty(fullName) && fullName.Length > 20)
                error += $"fullName param: {fullName ?? "null"} violates contract. ";

            string address = string.Empty;
            if (request.ContainsKey("address")) address = request["address"];
            if (!string.IsNullOrEmpty(address) && address.Length > 35)
                error += $"address param: {address ?? "null"} violates contract. ";

            string age = string.Empty;
            if (request.ContainsKey("age")) age = request["age"];
            if (!string.IsNullOrEmpty(age) && !ValidationHelper.IsStringMatchRegex(age, @"^[0-9]{1,3}$"))
                error += $"age param: {age ?? "null"} violates contract. ";
                
            var data = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(error))
            {
                data.Add("statusCode", "123");
                data.Add("errorMessage", error);

                context.Response.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            else
            {
                data.Add("providerPaymentId", RandomHelper.GetRandomAlphanumericString(12));
                data.Add("amount", "99.99");
                data.Add("currency", "USD");

                switch (ContextBehavior.GetStatusForPayment(request["paymentId"]))
                {
                    case Constants.PaymentStatuses.Captured:
                        data.Add("statusCode", "0");
                        data.Add("errorMessage", "OK. Transaction is successful");
                        break;
                    case Constants.PaymentStatuses.Refused:
                        data.Add("statusCode", "30");
                        data.Add("errorMessage", "Transaction is refused. Not enough money");
                        break;
                    case Constants.PaymentStatuses.Error:
                        data.Add("statusCode", "50");
                        data.Add("errorMessage", "Technical error");
                        break;
                    case Constants.PaymentStatuses.Cancelled:
                        data.Add("statusCode", "70");
                        data.Add("errorMessage", "Transaction is cancelled by user");
                        break;
                }

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                context.Response.Write(json);
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
