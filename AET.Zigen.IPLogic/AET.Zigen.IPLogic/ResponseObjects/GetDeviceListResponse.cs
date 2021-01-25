using Newtonsoft.Json;
using System.Collections.Generic;

namespace AET.Zigen.IPLogic.ResponseObjects {
  internal class GetDeviceListResponse : APIResponseObject<GetDeviceListResponse> {
    public List<Device> Devices {
      get { return Data.Devices; }
      set {
        Data = new DataObject {
          Devices = value
        };
      }
    }
    
    [JsonProperty("data")]
    private DataObject Data { get; set; }
    private class DataObject {
      [JsonProperty("devices")]
      public List<Device> Devices { get; set; }
    }
  }
}
