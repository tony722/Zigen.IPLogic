using Newtonsoft.Json;
using System;

namespace AET.Zigen.IPLogic.ResponseObjects {
  internal abstract class APIResponseObject<T> {
    public static T Deserialize(string json) {
      try {
        return JsonConvert.DeserializeObject<T>(json);
      } catch (Exception ex) {
        Config.Logger.Error("Unable to deserialize: ({0})\r\n{1}", ex.Message, json);
      }
      return default(T);
    }


    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }
  }
}
