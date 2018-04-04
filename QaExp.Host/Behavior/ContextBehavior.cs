using QaExp.Common;
using System.Collections.Concurrent;

namespace QaExp.Host.Behavior
{
    public static class ContextBehavior
    {
        private static ConcurrentDictionary<string, string> _storage;

        static ContextBehavior()
        {
            _storage = new ConcurrentDictionary<string, string>();
        }

        public static string GetStatusForPayment(string paymentId)
        {
            string status;
            if (!_storage.TryGetValue(paymentId, out status))
                return Constants.PaymentStatuses.Captured;

            return status;
        }

        public static void SetStatusForPayment(string paymentId, string paymentStatus)
        {
            _storage.TryAdd(paymentId, paymentStatus);
        }
    }
}