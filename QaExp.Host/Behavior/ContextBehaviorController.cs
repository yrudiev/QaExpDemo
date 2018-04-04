using System.Web;

namespace QaExp.Host.Behavior
{
    public class ContextBehaviorController : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var paymentId = context.Request.QueryString["paymentId"];
            var paymentStatus = context.Request.QueryString["paymentStatus"];

            if (!string.IsNullOrEmpty(paymentId) && !string.IsNullOrEmpty(paymentStatus))
            {
                ContextBehavior.SetStatusForPayment(paymentId, paymentStatus);
                context.Response.Write("OK");
            }
            else
            {
                context.Response.Write("NOK");
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
