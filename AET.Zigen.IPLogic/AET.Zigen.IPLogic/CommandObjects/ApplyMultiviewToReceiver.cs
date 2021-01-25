using Newtonsoft.Json;
using System.Collections.Generic;

namespace AET.Zigen.IPLogic.CommandObjects {
  public class ApplyMultiviewToReceiver : APICommandObject {
    public ApplyMultiviewToReceiver() : base("/ApplyMultiviewToReceiver") {
      TransmitterMacAddresses = new List<string>();
    }

    [JsonProperty("transmitter_list")]
    public List<string> TransmitterMacAddresses { get; set; }

    public void AddTransmitterByIndex(ushort index) {
      if (index == 0) TransmitterMacAddresses.Add(null);
      else TransmitterMacAddresses.Add(Config.DeviceAddressFromIndex("transmitter", index));
    }

    protected override bool RequiredFieldsAreValid() {
      if (TransmitterMacAddresses==null || TransmitterMacAddresses.Count == 0) return FalseWithErrorMessage("Transmitter address(es) are required");
      if (string.IsNullOrEmpty(ReceiverMacAddress)) return FalseWithErrorMessage("Receiver address is required");
      if (LayoutNumber < 0) return FalseWithErrorMessage("Layout number is required");
      return true;
    }

    [JsonProperty("receiver_mac")]
    public string ReceiverMacAddress { get; set; }


    /// <summary> Allowed values: 0-Picture in Picture, 1-Picture and Picture, 2-2x2, 3-3x3, 4-4x4, 
    ///           5-2x2 and middle, 6-4x4 and middle, 7-1big+3small left, 8-1big+3small right, 
    ///           9-1big+5small bottom left, 10-1big+5small bottom right, 11-1big+5small top left, 12-1big+5small top right, 
    ///           13-1big+7small bottom left, 14-1big+7small bottom right, 15-1big+7small top left, 16-1big+7small top right, 
    ///           17-32small, 18-2big+8small IShaped </summary>
    [JsonProperty("layout_number")]
    public int LayoutNumber { get; set; }

    /// <summary> A transmitter can only transmit a single scaled stream. 
    ///           False returns an error if it's currently streaming to another multiview at a different resolution. 
    ///           True forcibly disconnects it from the other multiview layout(s).</summary>
    [JsonProperty("force")]
    public bool Force { get; set; }

    public ushort Force_u {     
      set { Force = value != 0; }
    }

    /// <summary> Sets the FPS. 
    ///           It can be either an whole number (e.g. "50") or string like “60m” (59.94) or “30m” (29.97--Indicates frame rate must be divided by 1.001. i.e. multiplied by 1000⁄1001).
    ///           Note: By default, uses receiver device EDID framerate.</summary>
    [JsonProperty("fps")]
    public string FPS { get; set; }
  }
}
