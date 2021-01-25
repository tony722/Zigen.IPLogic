using Newtonsoft.Json;

namespace AET.Zigen.IPLogic {
  public class Device {
    public int Index { get; set; }

    [JsonProperty("type")]
    public string DeviceType { get; set; }

    [JsonProperty("mac")]
    public string MacAddress { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
  }  
}
