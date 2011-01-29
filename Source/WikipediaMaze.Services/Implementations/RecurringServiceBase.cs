using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public abstract class RecurringServiceBase : IRecurringService
    {

        /// <summary>
        /// The unique name of the service
        /// </summary>
        public abstract string ServiceName { get; }

        /// <summary>
        /// The interval in seconds that the service performs it's actions
        /// </summary>
        public virtual int Interval { get; set; }

        /// <summary>
        /// This method is called at each interval and is used to provide the functionality of the service.
        /// </summary>
        protected abstract void DoWork();

        /// <summary>
        /// Starts the service.
        /// </summary>
        public void StartService()
        {
            ResetTimer();
        }

        /// <summary>
        /// Stops the service
        /// </summary>
        public void StopService()
        {
            HttpRuntime.Cache.Remove(ServiceName);
        }

        /// <summary>
        /// Initials the timer and sets up the callback for when the specified time has elapsed
        /// </summary>
        private void ResetTimer()
        {
            HttpRuntime.Cache.Insert(ServiceName, Interval, null, DateTime.Now.AddSeconds(Interval),
                                     System.Web.Caching.Cache.NoSlidingExpiration,
                                     CacheItemPriority.NotRemovable, OnIntervalElapsed);
        }

        /// <summary>
        /// Callback for when the specified interval has elapsed.
        /// </summary>
        private void OnIntervalElapsed(string key, object value, CacheItemRemovedReason reason)
        {
            ResetTimer();

            DoWork();
        }
    }
}
