using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using QaExp.Common;
using RestSharp;

namespace QaExp.Testing
{
    [TestFixture]
    public class FakeTests
    {
        [Test]
        public void Test()
        {
            var paymentId = RandomHelper.GetRandomAlphanumericString(12);

            BehaviorHelper.Set(paymentId, Constants.PaymentStatuses.ManualCheck);

            var restClient = new RestClient("http://localhost:50188/handlers/QaExpDemoFake");
            var restRequest = new RestRequest(Method.POST);

            var data = new Dictionary<string, string>
            {
                { "action" , "withdraw123" },
                { "paymentId" , paymentId },
                { "amount" , "12.74" },
                { "currency" , "USD" },
                { "accountId" , "1234567890" },
                { "merchantId" , "123456778990" },
                { "email" , "johnDoe@mail.com" },
                { "fullName" , "John Doe" }
            };
           
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            restRequest.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = restClient.Execute(restRequest).Content;
        }
    }
}
