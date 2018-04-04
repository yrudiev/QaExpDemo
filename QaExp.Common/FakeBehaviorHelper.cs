using RestSharp;

namespace QaExp.Common
{
    public static class BehaviorHelper
    {
        public static void Set(string paymentId, string paymentStatus)
        {
            var restClient = new RestClient($"http://localhost:50188/Behavior/ContextBehaviorController");
            var restRequest = new RestRequest(Method.GET);
            restRequest.AddQueryParameter("paymentId", paymentId);
            restRequest.AddQueryParameter("paymentStatus", paymentStatus);
            var response = restClient.Execute(restRequest);
        }
    }
}
