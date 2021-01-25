using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {
  public class SaveRS232Configuration : APICommandObject {
    public SaveRS232Configuration() : base("/SaveRS232Configuration") { }

    [JsonProperty("mac")]
    public string  MacAddress { get; set; }

    /// <summary> Allowed Values: 2400, 4800, 9600, 19200, 38400, 57600, or 115200 </summary>
    [JsonProperty("baud_rate")]
    public int BaudRate { get; set; }

    /// <summary> Allowed Values: 6, 7, or 8 </summary>
    [JsonProperty("data_bits")]
    public ushort DataBits { get; set; }
    
    /// <summary> Allowed Values: 1 or 2 </summary>
    [JsonProperty("stop_bits")]
    public ushort StopBits { get; set; }

    /// <summary> Allowed Values: "NONE","ODD", or "EVEN" </summary>
    [JsonProperty("parity")]
    public string Parity { get; set; }

    protected override bool RequiredFieldsAreValid() {
      if (string.IsNullOrEmpty(MacAddress)) return FalseWithErrorMessage("MacAddress is required.");
      if (BaudRate < 2400 || BaudRate > 115200) return FalseWithErrorMessage("BaudRate = {0}. Must be 2400, 4800, 9600, 19200, 38400, 57600, or 115200", BaudRate);
      if (DataBits < 6 || DataBits > 8) return FalseWithErrorMessage("DataBits = {0}. Must be 6, 7, or 8.", DataBits);
      if (StopBits < 1 || StopBits > 2) return FalseWithErrorMessage("StopBits = {0}. Must be 1 or 2.", StopBits);
      if (!Parity.Matches(new[] { "NONE", "ODD", "EVEN" })) return FalseWithErrorMessage("Parity = '{0}'. Must be 'NONE','ODD', or 'EVEN'.", Parity);
      return true;
    }
  }
}
