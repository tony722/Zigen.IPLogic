using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {
  /// <summary> Sends data to rs232, infrared, or cec port. </summary>
  public class SendCommsData : APICommandObject {
    public SendCommsData() : base("/SendCommsData") { }

    [JsonProperty("destination")]
    public string MacAddress { get; set; }

    /// <summary> Type of port to send data to. 
    ///           Allowed Values: "rs232", "infrared", or "cec". </summary>
    [JsonProperty("type")]
    public string Type { get; set; }

    /// <summary> Data to send to the port: 
    ///           RS-232 string,
    ///           IR Hex Code (CCF/Pronto),
    ///           or CEC string.</summary>
    [JsonProperty("code")]
    public string Code { get; set; }

    protected override bool RequiredFieldsAreValid() {
      if (string.IsNullOrEmpty(MacAddress)) return FalseWithErrorMessage("MacAddress is required.");
      if (!Type.Matches(new[] { "rs232", "infrared", "cec" })) return FalseWithErrorMessage("Type is {0}. Must be 'rs232', 'infrared', or 'cec'",Type);
      if (string.IsNullOrEmpty(Code)) return FalseWithErrorMessage("Code is required.");
      return true;
    }
  }
}
