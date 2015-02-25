using System;
using System.Threading.Tasks;

namespace AzureTableStorageMagic.Specifications.Support
{
    public class ActualData
    {
        public object Result;
        public Exception Exception;

        public void ExecuteWhen(Func<Task> whenAction)
        {
            Result = null;
            Exception = null;

            try
            {
                whenAction().Wait();
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }
    }
}