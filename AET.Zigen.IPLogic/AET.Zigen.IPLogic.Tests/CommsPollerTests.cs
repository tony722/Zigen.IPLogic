using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Zigen.IPLogic.Tests {
  [TestClass]
  public class CommsPollerTests {
    [TestInitialize]
    public void TestInit() {
      TestHttpClient.Url = "";
      TestHttpClient.ResponseContents = "";
    }

    [TestMethod]
    public void PollADevice_PollsCorrectly() {
      string result = string.Empty;
      CommsPoller.RegisterForAutomaticPolling("70b3d5739438", (value) => result = value);
      TestHttpClient.ResponseContents = @"{""status"":""success"",""data"":[{""mac"":""70b3d5739438"",""timestamp"":""02:57:02"",""data"":""442244"",""type"":""rs232"",""event_id"":0}],""error"":"""",""messages"":[]}";
      CommsPoller.ManualPollMac("70b3d5739438");
      TestHttpClient.Url.Should().Be("http://test/GetCommsData");
      TestHttpClient.RequestContents.Should().Be(@"{""mac"":[""70b3d5739438""],""events"":50,""type"":""rs232"",""event_id"":0}");
      result.Should().Be("442244");
    }
    [TestMethod]
    public void ManualPollMac_InvalidResponse_DoesNotThrowError() {
      string result = string.Empty;
      TestHttpClient.ResponseContents = @"error";
      CommsPoller.ManualPollMac("70b3d5739438");
      result.Should().Be("");
    }
    
    [TestMethod]
    public void AutoPollRS232_ShouldWork() {
      CommsPoller.RegisterForAutomaticPolling("70b3d5739438", (x) => { });
      CommsPoller.StartPolling();
      TestHttpClient.Url.Should().Be("http://test/GetCommsData");
      TestHttpClient.RequestContents.Should().Be(@"{""mac"":[""70b3d5739438""],""events"":50,""type"":""rs232"",""event_id"":0}");
    }
  }
}
