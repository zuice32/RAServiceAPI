using System;

namespace Core.Services
{
    public interface IApplicationService : IDisposable
    {
        string ThisClassName { get; }

        IApplicationServiceBus ServiceBus { set; }

        string Title { get; }

        bool IsDisposed { get; }
    }
}