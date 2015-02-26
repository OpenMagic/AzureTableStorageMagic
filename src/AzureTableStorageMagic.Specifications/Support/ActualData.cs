using System;
using System.Threading.Tasks;

namespace AzureTableStorageMagic.Specifications.Support
{
    public class ActualData
    {
        public object Result;
        public Exception Exception;

        public void ExecuteWhen(Func<object> whenAction)
        {
            Result = null;
            Exception = null;

            try
            {
                Result = whenAction();
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        public void ExecuteWhen(Action whenAction)
        {
            Func<object> action = () =>
            {
                whenAction();
                return null;
            };    

            ExecuteWhen(action);
        }

        public void ExecuteWhen(Func<Task> whenAction)
        {
            Func<object> action = () =>
            {
                whenAction().Wait();
                return null;
            };

            ExecuteWhen(action);
        }
    }
}