using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {
  public class LeaveHDMIVideoAndAudioStream : APICommandObject {
    public LeaveHDMIVideoAndAudioStream() : base("/LeaveHDMIVideoandAudioStream") { }

    [JsonProperty("mac")]
    public string MacAddress { get; set; }

    protected override bool RequiredFieldsAreValid() {
      if (string.IsNullOrEmpty(MacAddress)) return FalseWithErrorMessage("MacAddress is required.");
      return true;
    }

  }
}
