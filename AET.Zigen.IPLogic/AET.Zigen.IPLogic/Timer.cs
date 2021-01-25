using System;

namespace AET.Zigen.IPLogic {
  internal interface ITimer {
    void Start(long repeatTime);
    void Stop();
    Action TimerElapsed { set; }
  }

  internal class CrestronTimer : ITimer, IDisposable {
    private Crestron.SimplSharp.CTimer timer;

    public Action TimerElapsed { private get; set; }

    public void Start(long repeatTime) {
      timer = new Crestron.SimplSharp.CTimer(TimerElapsedCallback, null, 0, repeatTime);
    }

    private void TimerElapsedCallback(object unused) {
      TimerElapsed();
    }

    public void Stop() {
      timer.Stop();
    }

    public void Dispose()
    {
      if (timer != null) timer.Dispose();
    }
  }
}
