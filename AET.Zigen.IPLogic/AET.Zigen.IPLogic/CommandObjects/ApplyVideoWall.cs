using Newtonsoft.Json;
using System.Collections.Generic;

namespace AET.Zigen.IPLogic.CommandObjects {
  public class ApplyVideoWall : APICommandObject {
    private List<VideoWallReceiver> receiverList;

    public ApplyVideoWall() : base("/ApplyVideoWall") { }

    private string transmitterMacAddress;
    [JsonProperty("transmitter_mac", NullValueHandling=NullValueHandling.Include)]
    public string TransmitterMacAddress {
      get { return transmitterMacAddress; }
      set { transmitterMacAddress = value == string.Empty ? null : value; } 
    }

    /// <summary> Number of Columns </summary>
    public int Width { get; set; }

    /// <summary> Number of Rows </summary>
    public int Height { get; set; }

    [JsonProperty("wall_size")]
    private int[] WallSize {
      get { return new[] { Width, Height }; }
    }

    /// <summary> Can be “genlock” or  “fastswitch”.</summary>
    [JsonProperty("mode")]
    public string SwitcherMode { get; set; }

    /// <summary>Allowed values:  
    ///          “keep” (Keep aspect ratio by adding black bars to wall) 
    ///          “stretch” (Stretch to cover entire wall with no black bars added) </summary>
    [JsonProperty("aspect_ratio")]
    public string AspectRatio { get; set; }

    [JsonProperty("receiver_list")]
    private List<VideoWallReceiver> ReceiverList {
      get { return receiverList; }
    }

    /// <summary>Receivers are added left to right, top to bottom</summary>
    public void AddReceiver(string macAddress) {
      if (receiverList == null) receiverList = new List<VideoWallReceiver>();
      ReceiverList.Add(new VideoWallReceiver { MacAddress = macAddress });
    }

    public void AddReceiver(VideoWallReceiver receiver) {
      if (receiverList == null) receiverList = new List<VideoWallReceiver>();
      ReceiverList.Add(receiver);
    }

    protected override bool RequiredFieldsAreValid() {
      if (Width == 0 || Height == 0) return FalseWithErrorMessage("Width and Height must both be > 0. Width = {0}. Height = {1}", Width, Height);
      if (SwitcherMode != "fastswitch" && SwitcherMode != "genlock") return FalseWithErrorMessage("SwitcherMode = '{0}'. Must be 'fastswitch' or 'genlock'", SwitcherMode);
      if (AspectRatio != "keep" && AspectRatio != "stretch") return FalseWithErrorMessage("AspectRatio = '{0'}. Must be 'keep' or 'stretch'.", AspectRatio);
      if (ReceiverList.Count < Width * Height) return FalseWithErrorMessage("ReceiverList Must contain {0} elements but only contains {1}.", Width * Height, ReceiverList.Count);
      return true;
    }
  }

  public class VideoWallReceiver {

    [JsonProperty("mac")]
    public string MacAddress { get; set; }

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

  }
}
