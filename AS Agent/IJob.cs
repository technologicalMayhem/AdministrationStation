using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AS_Agent
{
    public interface IJob
    {
        /// <summary>
        /// The name given to the task.
        ///
        /// This should not be changed by the job itself as its
        /// the name given to the job from the user to identify it among others.
        /// </summary>
        int JobId { get; set; }

        DateTime RunNext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="JobFailedException"></exception>
        /// <returns></returns>
        Task<JobResult> RunJob();
    }
    
    public class JobFailedException : Exception
    {
        public JobFailedException()
        {
        }

        public JobFailedException(string message) : base(message)
        {
        }

        public JobFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected JobFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}