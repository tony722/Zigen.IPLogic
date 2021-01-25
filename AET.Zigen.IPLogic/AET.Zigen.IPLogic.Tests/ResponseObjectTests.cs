using System;
using System.Collections.Generic;
using AET.Zigen.IPLogic.ResponseObjects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Zigen.IPLogic.Tests {
  [TestClass]
  public class ResponseObjectTests {
    [TestMethod]
    public void GetCommsData_MultipleEvents_DeserializesObjectCorrectly() {
      var response = GetCommsDataResponse.Deserialize(@"{""status"":""success"",""data"":[{""mac"":""70b3d5739439"",""timestamp"":""02:17:40"",""data"":""12345"",""type"":""rs232"",""event_id"":9},{""mac"":""70b3d5730000"",""timestamp"":""02:57:02"",""data"":""442244"",""type"":""rs232"",""event_id"":10}],""error"":"""",""messages"":[]}");
      response.Should().BeEquivalentTo(new GetCommsDataResponse {
        Status = "success",
        Error = "",
        Data = new List<GetCommsDataResponse.DataObject> {
          new GetCommsDataResponse.DataObject { Address = "70b3d5739439", Code = "12345", EventId = 9, Type = "rs232"  },
          new GetCommsDataResponse.DataObject { Address = "70b3d5730000", Code = "442244", EventId = 10, Type = "rs232"  }
        }
      });
    }

    [TestMethod]
    public void GetDeviceList_SampleData_DeserializesCorrectly() {
      var response = GetDeviceListResponse.Deserialize(@"{""error"":"""",""messages"":[],""data"":{""devices"":[{""videoconfig"":{""width"":3840,""mode"":""GENLOCK_MODE"",""height"":2160,""fps"":60,""autoscale"":true,""routeAudio"":true},""serialnumber"":""IPLBCDAECE217EDA"",""color_id"":""#E1DF13"",""zigen_mac"":""70b3d57393e8"",""type"":""receiver"",""name"":""1R_LG"",""mac"":""70b3d57393e7"",""url"":""/static/images/tv_icon_zigen.png"",""usb_mac"":""70b3d57393e9""},{""videoconfig"":{""width"":3840,""mode"":""GENLOCK_MODE"",""height"":2160,""fps"":60,""autoscale"":true,""routeAudio"":true},""serialnumber"":""IPLAA767AC308EDA"",""color_id"":""#5FEE68"",""zigen_mac"":""70b3d573943a"",""type"":""receiver"",""name"":""3R_TCL"",""mac"":""70b3d5739439"",""url"":""/static/images/tv_icon_zigen.png"",""usb_mac"":""70b3d573943b""},{""videoconfig"":{""width"":3840,""mode"":""GENLOCK_SCALING"",""height"":2160,""fps"":30,""autoscale"":true,""routeAudio"":true},""serialnumber"":""IPL00DDC16F16EDA"",""color_id"":""#BFB988"",""zigen_mac"":""70b3d57394e5"",""type"":""receiver"",""name"":""2R_Visio"",""mac"":""70b3d57393e4"",""url"":""/static/images/tv_icon_zigen.png"",""usb_mac"":""70b3d57393e6""},{""aes67_mac"":""70b3d5739451"",""videoconfig"":{""height"":2160,""width"":3840,""fps"":60},""serialnumber"":""IPLA7CF711809EDA"",""color_id"":""#B3A6D0"",""zigen_mac"":""70b3d5739450"",""type"":""transmitter"",""name"":""2T_BluRay"",""mac"":""70b3d573944f"",""url"":""/static/images/source_blue.png"",""usb_mac"":""70b3d5739452""},{""aes67_mac"":""70b3d573942b"",""videoconfig"":{""height"":2160,""width"":3840,""fps"":60},""serialnumber"":""IPL9B2B208A04EDA"",""color_id"":""#DABC9E"",""zigen_mac"":""70b3d573942a"",""type"":""transmitter"",""name"":""1T_Roku"",""mac"":""70b3d5739429"",""url"":""/static/images/source_blue.png"",""usb_mac"":""70b3d573942c""}]},""status"":""success""}");
      response.Should().BeEquivalentTo(new GetDeviceListResponse {
        Status = "success",
        Error = "",
        Devices = new List<Device> {
            new Device { DeviceType = "receiver", MacAddress = "70b3d57393e7", Name = "1R_LG" },
            new Device { DeviceType = "receiver", MacAddress = "70b3d5739439", Name = "3R_TCL" },
            new Device { DeviceType = "receiver", MacAddress = "70b3d57393e4", Name = "2R_Visio"},
            new Device { DeviceType = "transmitter", MacAddress = "70b3d573944f", Name = "2T_BluRay" },
            new Device { DeviceType = "transmitter", MacAddress = "70b3d5739429", Name = "1T_Roku" }
        }
      });
    }
  }
}
