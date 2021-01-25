using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AET.Zigen.IPLogic.Tests {
  class TestTimer : ITimer {
    public Action TimerElapsed { get;  set; }

    public void Start(long repeatTime) {
      TimerElapsed();      
    }

    public void Stop() { }
  }
}
