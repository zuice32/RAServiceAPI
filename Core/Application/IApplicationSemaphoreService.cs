using System;

namespace Core.Application
{
    public interface IApplicationSemaphoreService
    {
        void SynchronizeThread(Action synchronizedAction);
    }
}