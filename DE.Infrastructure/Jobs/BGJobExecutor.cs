using Autofac;
using DE.Infrastructure.Concept;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DE.Infrastructure.Jobs
{
    public class BGJobExecutor
    {
        private readonly IComponentContext _context;
        public BGJobExecutor(IComponentContext context)
        {
            _context = context;
        }

        public async Task ExecuteJob(BGJobData jobData)
        {
            Type jobType = LoadType(jobData.Type);
            Type argumentType = LoadType(jobData.ArgumentType);
            dynamic argumentObject = JsonConvert.DeserializeObject(jobData.ArgumentData, argumentType);
            dynamic bgJob = _context.Resolve(jobType);

            using (var _unitOfWork = _context.Resolve<IUnitOfWork>())
            {
                //execute job
                await bgJob.Execute(argumentObject);

                //commit changes
                _unitOfWork.Commit();
            }
        }

        private static Type LoadType(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
                type = Assembly.Load("DPWVessel.Model").GetType(typeName);
            if (type == null)
                throw new ArgumentException("Cant resolve type for " + typeName);
            return type;
        }
    }
}