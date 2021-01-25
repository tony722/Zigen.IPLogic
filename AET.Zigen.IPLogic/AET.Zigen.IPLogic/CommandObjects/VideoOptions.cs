using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {
  internal class VideoOptions {
    [JsonProperty("size")]
    public Size Size { get; set; }

    [JsonProperty("fps")]
    public string FPS { get; set; }
  }

  internal class Size {
    [JsonProperty("width")]
    public string Width { get; set; }

    [JsonProperty("height")]
    public string Height { get; set; }

  }

}
