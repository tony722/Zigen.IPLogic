using System;
using Crestron.SimplSharp;

namespace AET.Zigen.IPLogic {
  internal interface IMutex {
    void Enter();
    void Leave();
  }

  internal class CrestronMutex : IMutex, IDisposable {
    private CCriticalSection mutex;
    public void Enter() {
      if (mutex == null) mutex = new CCriticalSection();
      mutex.Enter();
    }

    public void Leave() {
      mutex.Leave();
    }

    public void Dispose()
    {
      if (mutex != null) mutex.Dispose();
    }
  }
}
