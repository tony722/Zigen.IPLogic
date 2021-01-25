using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {
  public class LeaveMultiviewStreams : APICommandObject {
    public LeaveMultiviewStreams() : base("/LeaveMultiviewStreams") { }
    [JsonProperty("mac")]
    public string MacAddress { get; set; }

    protected override bool RequiredFieldsAreValid() {
      if (string.IsNullOrEmpty(MacAddress)) return FalseWithErrorMessage("MacAddress is required.");
      return true;
    }
  }
}
