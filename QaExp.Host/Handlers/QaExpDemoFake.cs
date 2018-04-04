using Newtonsoft.Json;
using QaExp.Common;
using QaExp.Host.Behavior;
using System;
using System.Collections.Generic;
using System.Web;

namespace QaExp.Host.Handlers
{
    public class QaExpDemoFake : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = RestHelper.GetRequestFromBody(context.Request.InputStream);
            var error = string.Empty;

            var action = request.ContainsKey("action") ? request["action"] : string.Empty;
            if (string.IsNullOrEmpty(action) || !ValidationHelper.IsStringMatchRegex(action, "^[a-z]{1,8}$"))
                error += $"Param action: \"{action}\" violates contracts";

            var paymentId = request.ContainsKey("paymentId") ? request["paymentId"] : string.Empty;
            if (string.IsNullOrEmpty(paymentId) || !ValidationHelper.IsStringMatchRegex(action, "^[A-Z0-9]{1,15}$"))
                error += $"Param paymentId: \"{paymentId}\" violates contracts";

            var email = request.ContainsKey("email") ? request["email"] : string.Empty;
            if (!string.IsNullOrEmpty(email) && !ValidationHelper.IsStringMatchRegex(action, @"^[A-Za-z0-9]+@[a-zA-Z]+\.\S+$"))
                error += $"Param email: \"{email}\" violates contracts";

            if (string.IsNullOrEmpty(error))
            {

                var amount = request["amount"];
                var currency = request["currency"];
                var data = new Dictionary<string, string>
            {
                {"providerPaymentId", RandomHelper.GetRandomAlphanumericString(15)},
                {"amount", amount },
                {"currency", currency }
            };

                switch (ContextBehavior.GetStatusForPayment(paymentId))
                {
                    case Constants.PaymentStatuses.Captured:
                        data.Add("statusCode", "0");
                        data.Add("errorMessage", "Transaction is successful");
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
                    case Constants.PaymentStatuses.ManualCheck:
                        data.Add("statusCode", "0");
                        data.Add("errorMessage", "Transaction is successful");
                        break;
                }




                context.Response.Write(JsonConvert.SerializeObject(data));
            }
            else
            {
                var er = new Dictionary<string, string>
                {
                    { "statusCode", "123"},
                    { "errorMessage", error},
                };

                context.Response.Write(JsonConvert.SerializeObject(er));
            }
        }

        public bool IsReusable
        {
            // Верните значение false в том случае, если ваш управляемый обработчик не может быть повторно использован для другого запроса.
            // Обычно значение false соответствует случаю, когда некоторые данные о состоянии сохранены по запросу.
            get { return true; }
        }
    }
}
