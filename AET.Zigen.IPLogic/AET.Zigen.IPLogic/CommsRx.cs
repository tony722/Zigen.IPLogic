using Crestron.SimplSharp;

namespace AET.Zigen.IPLogic {
  public class CommsRx {
    public CommsRx() {
      RxCode = delegate { };
    }

    public string MacAddress { get; set; }

    public void RegisterForAutomaticPolling() {
      CommsPoller.RegisterForAutomaticPolling(MacAddress, ReceiveData);
    }


    public void Poll() {
      CommsPoller.ManualPollMac(MacAddress);
    }

    private void ReceiveData(string data) {
      RxCode(data);
    }

    public SPlusStringOutputDelegate RxCode { get; set; }    

  }

  public delegate void SPlusStringOutputDelegate(SimplSharpString value);
}
