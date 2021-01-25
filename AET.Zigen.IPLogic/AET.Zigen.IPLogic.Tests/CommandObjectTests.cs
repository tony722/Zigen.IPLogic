using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AET.Zigen.IPLogic.CommandObjects;
using FluentAssertions;

namespace AET.Zigen.IPLogic.Tests {
  [TestClass]
  public class CommandObjectTests {
    [TestInitialize]
    public void TestInit() {
      TestHttpClient.Url = "";
      TestHttpClient.ResponseContents = "";
      TestHttpClient.RequestContents = "";
    }
    #region RouteHDMI tests
    [TestMethod]
    public void RouteHDMI_OnlyRequiredParams_SerializesCorrectly() {
      var o = new RouteHDMI {
        TransmitterMacAddress = Config.DeviceAddressFromName("1T"),
        ReceiverMacAddress = Config.DeviceAddressFromName("1R")
      };
      o.GetJson().Should().Be(@"{""source"":""80F000000001"",""destination"":""80F000000002""}");
    }

    [TestMethod]
    public void RouteHDMI_OnlyRequiredParams_SendsCorrectHttpPost() {
      var r = new RouteHDMI {
        TransmitterMacAddress = Config.DeviceAddressFromName("1T"), //See AssemblyInit where these indexes are added to the config
        ReceiverMacAddress = Config.DeviceAddressFromName("1R")
      };
      r.Execute();
      TestHttpClient.RequestContents.Should().Be(@"{""source"":""80F000000001"",""destination"":""80F000000002""}");
      TestHttpClient.Url.Should().Be("http://testhost/RouteHDMI");
    }

    [TestMethod]
    public void RouteHDMI_AllOptionalParams_SerializesCorrectly() {
      var o = new RouteHDMI {
        TransmitterMacAddress = "80F000000001",
        ReceiverMacAddress = "80F000000002",
        SwitcherMode = "Genlock Scaling",        
        FPS = "29.97",
        Width = "1920",
        Height = "1080"
      };
      Config.EnableAudioBreakaway = true;
      o.GetJson().Should().Be(@"{""source"":""80F000000001"",""destination"":""80F000000002"",""mode"":""Genlock Scaling"",""route audio"":false,""video options"":{""size"":{""width"":""1920"",""height"":""1080""},""fps"":""29.97""}}");      
      Config.EnableAudioBreakaway = false;
    }
    #endregion

    #region RouteAudio Tests
    [TestMethod]
    public void RouteAudio_OnlyRequiredParams_SendsCorrectHttpPost() {
      var a = new RouteAudioStream {        
        ReceiverMacAddress = Config.DeviceAddressFromName("1R"),
        TransmitterMacAddress = Config.DeviceAddressFromName("1T")
      };
      a.Execute();
      TestHttpClient.RequestContents.Should().Be(@"{""source"":""80F000000001"",""destination"":""80F000000002""}");
      TestHttpClient.Url.Should().Be("http://testhost/RouteAudioStream");
    }

    [TestMethod]
    public void RouteAudio_AllParams_SerializesCorrectly() {
      var a = new RouteAudioStream {
        ReceiverMacAddress = Config.DeviceAddressFromName("1R"),
        TransmitterMacAddress = Config.DeviceAddressFromName("1T"),
        ReceiverSource = "TOSLINK"
      };
      a.GetJson().Should().Be(@"{""source"":""80F000000001"",""destination"":""80F000000002"",""args"":{""receiver_source"":""TOSLINK""}}");
    }
    #endregion


    [TestMethod]
    public void SendCommsData_SendsCorrectHTTPPost() {
      var rs232 = new SendCommsData {
        MacAddress = Config.DeviceAddressFromName("1R"),
        Type = "rs232",
        Code = "test"
      };            
      rs232.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/SendCommsData");
      TestHttpClient.RequestContents.Should().Be(@"{""destination"":""80F000000002"",""type"":""rs232"",""code"":""test""}");
    }

    [TestMethod]
    public void SaveRS232Configuration_SendsCorrectHTTPPost() {
      var rs232 = new SaveRS232Configuration {
        MacAddress = Config.DeviceAddressFromName("1R"),
        BaudRate = 9600,
        DataBits = 8,
        StopBits = 1,
        Parity = "NONE"
      };
      rs232.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/SaveRS232Configuration");
      TestHttpClient.RequestContents.Should().Be(@"{""mac"":""80F000000002"",""baud_rate"":9600,""data_bits"":8,""stop_bits"":1,""parity"":""NONE""}");
    }

    [TestMethod]
    public void LeaveHDMIVideoAndAudioStream_SendsCorrectHTTPPost() {
      var hdmi = new LeaveHDMIVideoAndAudioStream {
        MacAddress = Config.DeviceAddressFromName("1R")
      };
      hdmi.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/LeaveHDMIVideoandAudioStream");
      TestHttpClient.RequestContents.Should().Be(@"{""mac"":""80F000000002""}");
    }
    [TestMethod]
    public void LeaveHDMIVideoStream_SendsCorrectHTTPPost() {
      var hdmi = new LeaveHDMIVideoStream {
        MacAddress = Config.DeviceAddressFromName("1R")
      };
      hdmi.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/LeaveHDMIVideoStream");
      TestHttpClient.RequestContents.Should().Be(@"{""mac"":""80F000000002""}");
    }

    [TestMethod]
    public void LeaveHDMIAudioStream_SendsCorrectHTTPPost() {
      var hdmi = new LeaveHDMIAudioStream {
        MacAddress = Config.DeviceAddressFromName("1R")
      };
      hdmi.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/LeaveHDMIAudioStream");
      TestHttpClient.RequestContents.Should().Be(@"{""mac"":""80F000000002""}");
    }

    [TestMethod]
    public void ApplyMultiviewToReceiver_SendsCorrectHTTPPost() {
      var mv = new ApplyMultiviewToReceiver {
        ReceiverMacAddress = Config.DeviceAddressFromName("1R"),
        Force = true,
        LayoutNumber = 5,
        TransmitterMacAddresses = { "ABCD", "EFGH" },
        FPS = "30m"
      };
      mv.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/ApplyMultiviewToReceiver");
      TestHttpClient.RequestContents.Should().Be(@"{""transmitter_list"":[""ABCD"",""EFGH""],""receiver_mac"":""80F000000002"",""layout_number"":5,""force"":true,""fps"":""30m""}");

    }

    [TestMethod]
    public void ApplyMultiviewToReceiver_AddTransmitterIndex0_AddsNullToJson() {
      var mv = new ApplyMultiviewToReceiver();
      mv.AddTransmitterByIndex(0);
      mv.AddTransmitterByIndex(1);
      mv.GetJson().Should().Be(@"{""transmitter_list"":[null,""80F000000001""],""layout_number"":0,""force"":false}");
    }

    [TestMethod]
    public void LeaveMultiviewStreams_SendsCorrectHTTPPost() {
      var mv = new LeaveMultiviewStreams {
        MacAddress = Config.DeviceAddressFromName("1R")
      };
      mv.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/LeaveMultiviewStreams");
      TestHttpClient.RequestContents.Should().Be(@"{""mac"":""80F000000002""}");
    }

    [TestMethod]
    public void ApplyVideoWall_SendsCorrectHTTPPost() {
      var vw = new ApplyVideoWall {
        TransmitterMacAddress = Config.DeviceAddressFromName("1R"),
        Height = 2,
        Width = 2,
        AspectRatio = "keep",
        SwitcherMode = "fastswitch"
      };
      vw.AddReceiver("ABCDE");
      vw.AddReceiver("12345");
      vw.AddReceiver("FEDBC");
      vw.AddReceiver("54321");

      vw.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/ApplyVideoWall");
      TestHttpClient.RequestContents.Should().Be(@"{""transmitter_mac"":""80F000000002"",""wall_size"":[2,2],""mode"":""fastswitch"",""aspect_ratio"":""keep"",""receiver_list"":[{""mac"":""ABCDE""},{""mac"":""12345""},{""mac"":""FEDBC""},{""mac"":""54321""}]}");

    }

    [TestMethod]
    public void ApplyVideoWall_TransmitterIndexIs0_SendsNullInHTTPPost() {
      var vw = new ApplyVideoWall {
        TransmitterMacAddress = Config.Devices["transmitter", 0].MacAddress,
        Height = 2,
        Width = 2,
        AspectRatio = "keep",
        SwitcherMode = "fastswitch",        
      };
      vw.AddReceiver("ABCDE");
      vw.AddReceiver("12345");
      vw.AddReceiver("FEDBC");
      vw.AddReceiver("54321");
      vw.Execute();
      TestLogger.ErrorMessage.Should().BeNull();
      TestHttpClient.Url.Should().Be("http://testhost/ApplyVideoWall");
      TestHttpClient.RequestContents.Should().Be(@"{""transmitter_mac"":null,""wall_size"":[2,2],""mode"":""fastswitch"",""aspect_ratio"":""keep"",""receiver_list"":[{""mac"":""ABCDE""},{""mac"":""12345""},{""mac"":""FEDBC""},{""mac"":""54321""}]}");
    }

    [TestMethod]
    public void GetDeviceList_NoParams_SendsCorrectHTTPPost() {
      var devList = new GetDeviceList();
      devList.Execute();
      TestHttpClient.Url.Should().Be("http://testhost/GetDeviceList");
      TestHttpClient.RequestContents.Should().Be(@"{}");
    }

    [TestMethod]
    public void GetDeviceList_AllParms_GeneratesCorrectJson() {
      var devList = new GetDeviceList {
        Connection = "online",
        DeviceType = "all"
      };
      devList.GetJson().Should().Be(@"{""type"":""all"",""connection"":""online""}");
    }
  }
}
