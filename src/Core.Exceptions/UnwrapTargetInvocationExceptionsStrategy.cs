using System;
using System.Reflection;

namespace Core.Exceptions
{
    public class UnwrapTargetInvocationExceptionsStrategy : IExceptionStrategy
    {
        public Exception ProcessException(Exception e)
        {
            TargetInvocationException invokeException = e as TargetInvocationException;

            return invokeException == null ? e : invokeException.InnerException;
        }
    }
}
