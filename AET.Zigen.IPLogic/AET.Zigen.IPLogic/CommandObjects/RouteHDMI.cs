using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {

  /// <summary> Routes HDMI from a transmitter to a receiver </summary>
  public class RouteHDMI : APICommandObject {
    public RouteHDMI() : base("/RouteHDMI") {   }

    [JsonProperty("source")]
    public string TransmitterMacAddress { get; set; }

    [JsonProperty("destination")]
    public string ReceiverMacAddress { get; set; }

    /// <summary> Can be “Genlock”, “Genlock Scaling”, or  “Fastswitch”. (case-insensitive) </summary>
    [JsonProperty("mode")]
    public string SwitcherMode { get; set; }

    /// <summary> If omitted, true by default. 
    ///           True: Route with audio.
    ///           False: Do not route with audio. </summary>
    [JsonProperty("route audio")]
    private bool? RouteAudio {
      get {
        if (Config.EnableAudioBreakaway) return false;
        else return null;
      }
    }

    /// <summary> Sets the FPS. 
    ///           It can be either an whole number (e.g. "50") or string like “60m” (59.94) or “30m” (29.97--Indicates frame rate must be divided by 1.001. i.e. multiplied by 1000⁄1001).</summary>
    public string FPS {
      set {
        if (VideoOptions == null) VideoOptions = new VideoOptions();
        VideoOptions.FPS = value;
      }
    }


    public string Width {
      set {
        if (VideoOptions == null) VideoOptions = new VideoOptions();
        if (VideoOptions.Size == null) VideoOptions.Size = new Size();
        VideoOptions.Size.Width = value;
      }
    }

    public string Height {
      set {
        if (VideoOptions == null) VideoOptions = new VideoOptions();
        if (VideoOptions.Size == null) VideoOptions.Size = new Size();
        VideoOptions.Size.Height = value;
      }
    }
   
    [JsonProperty("video options")]
    private VideoOptions VideoOptions { get; set; }

    protected override bool RequiredFieldsAreValid() {
      if (string.IsNullOrEmpty(TransmitterMacAddress)) return FalseWithErrorMessage("TransmitterMacAddress is required.");
      if (string.IsNullOrEmpty(ReceiverMacAddress)) return FalseWithErrorMessage("ReceiverMacAddress is required.");
      return true;
    }
  }
}

