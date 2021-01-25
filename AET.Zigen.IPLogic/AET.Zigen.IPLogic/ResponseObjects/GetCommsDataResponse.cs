using Newtonsoft.Json;
using System.Collections.Generic;

namespace AET.Zigen.IPLogic.ResponseObjects {
  internal class GetCommsDataResponse : APIResponseObject<GetCommsDataResponse> {    
    [JsonProperty("data")]
    public List<DataObject> Data { get; set; }

    internal class DataObject {
      [JsonProperty("mac")]
      public string Address { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("data")]
      public string Code { get; set; }

      [JsonProperty("event_id")]
      public int EventId { get; set; }
    }
  }
}
