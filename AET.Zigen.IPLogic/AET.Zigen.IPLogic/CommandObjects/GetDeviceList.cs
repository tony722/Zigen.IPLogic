using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {
  internal class GetDeviceList : APICommandObject {
    public GetDeviceList() : base("/GetDeviceList") { }

    /// <summary>DeviceType (optional): "transmitter", "receiver", "cage", or "all" (default)</summary>
    [JsonProperty("type")]
    public string DeviceType { get; set; }

    /// <summary>Connection (optional): "online", "offline", or "all" (default)</summary>
    [JsonProperty("connection")]
    public string Connection{ get; set; }

    protected override bool RequiredFieldsAreValid() {
      return true;
    }
  }
}
