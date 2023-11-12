using DE.Infrastructure.Concept;
using DE.Infrastructure.Jobs;
using Newtonsoft.Json;
using System;

namespace DPWVessel.Web.Core.Jobs
{
    public static class BGJobScheduler
    {
        public static void Enqueue<T1, T2>(T2 args) where T1 : IBackgroundJob<T2>
        {
            BGJobData data = new BGJobData
            {
                Type = typeof(T1).FullName,
                ArgumentType = typeof(T2).FullName,
                ArgumentData = JsonConvert.SerializeObject(args)
            };
            Hangfire.BackgroundJob.Enqueue<BGJobExecutor>(x => x.ExecuteJob(data));
        }

        public static void Schedule<T1, T2>(T2 args, Func<string> cron) where T1 : IBackgroundJob<T2>
        {
            BGJobData data = new BGJobData
            {
                Type = typeof(T1).FullName,
                ArgumentType = typeof(T2).FullName,
                ArgumentData = JsonConvert.SerializeObject(args)
            };

            Hangfire.RecurringJob.AddOrUpdate<BGJobExecutor>(typeof(T1).Name.ToString(), x => x.ExecuteJob(data), cron);
        }
    }
}