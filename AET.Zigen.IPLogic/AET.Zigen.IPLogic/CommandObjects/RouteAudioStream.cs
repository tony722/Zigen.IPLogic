using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {
  public class RouteAudioStream : APICommandObject {
    public RouteAudioStream() : base("/RouteAudioStream") { }

    [JsonProperty("source")]
    public string TransmitterMacAddress { get; set; }

    [JsonProperty("destination")]
    public string ReceiverMacAddress { get; set; }

    /// <summary> Only used for receiver->transmitter routing.
    ///           Allowed values: "TOSLINK", "ARC", or "ANALOG".</summary>
    public string ReceiverSource {
      get {
        if (Args == null) return string.Empty;
        else return Args.ReceiverSource;
      }
      set {
        Args = new _Args { ReceiverSource = value };
      }
    }

    [JsonProperty("args")]
    private _Args Args { get; set; }

    protected override bool RequiredFieldsAreValid() {
      if (string.IsNullOrEmpty(TransmitterMacAddress)) return FalseWithErrorMessage("TransmitterMacAddress is required.");
      if (string.IsNullOrEmpty(ReceiverMacAddress)) return FalseWithErrorMessage("ReceiverMacAddress is required.");
      return true;
    }

    private class _Args {
      [JsonProperty("receiver_source")]
      public string ReceiverSource { get; set; }
    }
  }
}
