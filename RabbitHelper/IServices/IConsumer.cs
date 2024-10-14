using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitHelper.IServices
{
    public interface IConsumer
    {
        public void StartConsuming(Action<string> messageHandler, ushort prefetchCount = 1);
        public void Dispose();

    }
}
