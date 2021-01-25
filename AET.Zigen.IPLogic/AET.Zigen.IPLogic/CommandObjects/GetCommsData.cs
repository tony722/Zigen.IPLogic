using Newtonsoft.Json;
using System.Collections.Generic;

namespace AET.Zigen.IPLogic.CommandObjects {
  public class GetCommsData : APICommandObject {
    public GetCommsData() : base("/GetCommsData") {
      MacAddresses = new List<string>();
    }
    [JsonProperty("mac")]
    public List<string> MacAddresses { get; set; }

    [JsonProperty("events")]
    public int MaxEventsToRetrieve{ get; set; }

    /// <summary>Required. Type of port to get data from. Allowed values: "rs232", "infrared", or "cec" </summary>
    [JsonProperty("type")]
    public string Type { get; set; }

    /// <summary>First</summary>
    [JsonProperty("event_id")]
    public long FirstEventIdToRetrieve { get; set; }

    protected override bool RequiredFieldsAreValid() {
      if (MacAddresses.Count == 0) return FalseWithErrorMessage("MacAddresses is required.");
      if (!Type.Matches(new[] { "rs232", "infrared", "cec" })) return FalseWithErrorMessage("Type is {0}. Must be 'rs232', 'infrared', or 'cec'", Type);
      return true;
    }
  }
}
