using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AET.Zigen.IPLogic.Tests {
  internal class TestMutex : IMutex {
    private readonly Mutex mutex = new Mutex();
    public void Enter() {
      mutex.WaitOne();
    }

    public void Leave() {
      mutex.ReleaseMutex();
    }
  }
}
