using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChasBWare.Spotlight.Test.Infrastructure.System
{
    public class MockDispatcher : IDispatcher
    {
        public bool IsDispatchRequired => true;

        public IDispatcherTimer CreateTimer()
        {
            throw new NotImplementedException();
        }

        public bool Dispatch(Action action)
        {
            action();
            return true;
        }

        public bool DispatchDelayed(TimeSpan delay, Action action)
        {
            throw new NotImplementedException();
        }
    }
}
