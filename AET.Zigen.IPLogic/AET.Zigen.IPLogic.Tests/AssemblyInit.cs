using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Zigen.IPLogic.Tests {
  [TestClass]
  public static class AssemblyInit {

    [AssemblyInitialize]
    public static void Init(TestContext context) {
      APIClient.HttpClient = new TestHttpClient();
      APIClient.HostName = "http://testhost";
      CommsPoller.HttpClient = new TestHttpClient();
      CommsPoller.Timer = new TestTimer();
      Config.Logger = new TestLogger();
      Config.Mutex = new TestMutex();
      Config.Devices = new Devices(new List<Device> { new Device {DeviceType = "transmitter", Name =  "1T", MacAddress  = "80F000000001", Index = 1}, new Device { DeviceType = "transmitter", Name = "2T", MacAddress = "80F000000010", Index = 2 }, new Device { DeviceType = "receiver", Name = "1R", MacAddress = "80F000000002", Index = 1 }, new Device { DeviceType = "receiver", Name = "2R", MacAddress = "80F000000002", Index = 2 } }, new TestMutex());      
    }
  }
}
