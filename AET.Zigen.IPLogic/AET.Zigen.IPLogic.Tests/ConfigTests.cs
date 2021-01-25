using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Zigen.IPLogic.Tests {
  [TestClass]
  public class ConfigTests {

    [TestMethod]
    public void MatchValuesExtension_Works() {
      "Hello".Matches(new[] { "This", "Hello", "World" }).Should().BeTrue();
      "Oops".Matches(new[] { "This", "Hello", "World" }).Should().BeFalse();
      string nullString = null;
      nullString.Matches(new[] { "This", "Hello" }).Should().BeFalse();
    }

    #region Address Methods
    [TestMethod]
    public void ReceiverAddress_stringName_ReturnsAddress() {
      Config.DeviceAddressFromName("1R").Should().Be("80F000000002");
    }

    [TestMethod]
    public void ReceiverAddress_ushortIndex_ReturnsAddress() {
      Config.ReceiverAddressFromIndex(1).Should().Be("80F000000002");
    }
    [TestMethod]
    public void TransmitterAddress_stringName_ReturnsAddress() {
      Config.DeviceAddressFromName("1T").Should().Be("80F000000001");
    }
    [TestMethod]
    public void TransmitterAddress_ushortIndex_ReturnsAddress() {
      Config.TransmitterAddressFromIndex(1).Should().Be("80F000000001");
    }

    [TestMethod]
    public void TransmitterAddress_IndexNotFound_ReturnsEmptyString() {
      Config.TransmitterAddressFromIndex(999).Should().BeEmpty();
    }

    [TestMethod]
    public void DeviceAddress_stringName_ReturnsAddress() {
      Config.DeviceAddressFromName("1T").Should().Be("80F000000001");
    }

    [TestMethod]
    public void DeviceAddress_ushortIndex_ReturnsAddress() {
      Config.DeviceAddressFromIndex("receiver", 1).Should().Be("80F000000002");
    }

    #endregion 
    [TestMethod]
    public void SetDeviceAddress_Index50_SetsIndex() {
      Config.SetDeviceIndex("1R", 50);
      Config.Devices.RebuildDicts();
      Config.Devices["1R"].Index.Should().Be(50);
      Config.Devices["receiver", 50].Name.Should().Be("1R");
      Config.SetDeviceIndex("1R", 1); //Reset to default
      Config.Devices.RebuildDicts();
    }


    [TestMethod]
    public void GetTransmittersAndReceivers_ValidData_PopulatesDevices() {
      TestHttpClient.ResponseContents = @"{""error"":"""",""messages"":[],""data"":{""devices"":[{""videoconfig"":{""width"":3840,""mode"":""GENLOCK_MODE"",""height"":2160,""fps"":60,""autoscale"":true,""routeAudio"":true},""serialnumber"":""IPLBCDAECE217EDA"",""color_id"":""#E1DF13"",""zigen_mac"":""70b3d57393e8"",""type"":""receiver"",""name"":""1R_LG"",""mac"":""70b3d57393e7"",""url"":""/static/images/tv_icon_zigen.png"",""usb_mac"":""70b3d57393e9""},{""videoconfig"":{""width"":3840,""mode"":""GENLOCK_MODE"",""height"":2160,""fps"":60,""autoscale"":true,""routeAudio"":true},""serialnumber"":""IPLAA767AC308EDA"",""color_id"":""#5FEE68"",""zigen_mac"":""70b3d573943a"",""type"":""receiver"",""name"":""3R_TCL"",""mac"":""70b3d5739439"",""url"":""/static/images/tv_icon_zigen.png"",""usb_mac"":""70b3d573943b""},{""videoconfig"":{""width"":3840,""mode"":""GENLOCK_SCALING"",""height"":2160,""fps"":30,""autoscale"":true,""routeAudio"":true},""serialnumber"":""IPL00DDC16F16EDA"",""color_id"":""#BFB988"",""zigen_mac"":""70b3d57394e5"",""type"":""receiver"",""name"":""2R_Visio"",""mac"":""70b3d57393e4"",""url"":""/static/images/tv_icon_zigen.png"",""usb_mac"":""70b3d57393e6""},{""aes67_mac"":""70b3d5739451"",""videoconfig"":{""height"":2160,""width"":3840,""fps"":60},""serialnumber"":""IPLA7CF711809EDA"",""color_id"":""#B3A6D0"",""zigen_mac"":""70b3d5739450"",""type"":""transmitter"",""name"":""2T_BluRay"",""mac"":""70b3d573944f"",""url"":""/static/images/source_blue.png"",""usb_mac"":""70b3d5739452""},{""aes67_mac"":""70b3d573942b"",""videoconfig"":{""height"":2160,""width"":3840,""fps"":60},""serialnumber"":""IPL9B2B208A04EDA"",""color_id"":""#DABC9E"",""zigen_mac"":""70b3d573942a"",""type"":""transmitter"",""name"":""1T_Roku"",""mac"":""70b3d5739429"",""url"":""/static/images/source_blue.png"",""usb_mac"":""70b3d573942c""}]},""status"":""success""}";
      Config.GetTransmittersAndReceivers();
      Config.DeviceAddressFromName("1T_Roku").Should().Be("70b3d5739429");
      Config.SetDeviceIndex("1T_Roku", 1);
      Config.Devices.RebuildDicts();
      Config.TransmitterAddressFromIndex(1).Should().Be("70b3d5739429");
    }
  }
}
