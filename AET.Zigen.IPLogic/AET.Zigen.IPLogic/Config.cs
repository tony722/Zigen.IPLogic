using AET.Zigen.IPLogic.CommandObjects;
using AET.Zigen.IPLogic.ResponseObjects;

using System;

namespace AET.Zigen.IPLogic {
  public static class Config {
    static Config() {
      Logger = new CrestronLogger();
      Mutex = new CrestronMutex();
      Devices = new Devices();
      PollInterval = 5000;
    }

    internal static event Action LoadComplete;

    public static Devices Devices { get; set; }

    internal static ILogger Logger { get; set; }

    internal static IMutex Mutex { get; set; }

    public static int PollInterval { get; set; }

    public static string ReceiverAddressFromIndex(ushort index) { return Devices["receiver",index].MacAddress ?? string.Empty; }
    public static string TransmitterAddressFromIndex(ushort index) { return Devices["transmitter", index].MacAddress ?? string.Empty; }

    public static string DeviceAddressFromIndex(string deviceType, ushort index) { return Devices[deviceType, index].MacAddress ?? string.Empty; }
    public static string DeviceAddressFromName(string name) { return Devices[name].MacAddress ?? string.Empty; }

    public static void SetDeviceIndex(string name, ushort index) { 
      Devices[name].Index = index;
    }

    internal static bool EnableAudioBreakaway { get; set; }
    
    /// <summary> For use by SIMPL+: Makes the EnableAudioBreakaway bool property accessible. </summary>
    public static ushort EnableAudioBreakaway_u {
      get {
        return (ushort)(EnableAudioBreakaway ? 1 : 0);
      }
      set {
        EnableAudioBreakaway = value > 0;
      }
    }

    public static void Start () {
      GetTransmittersAndReceivers();
    }

    internal static void GetTransmittersAndReceivers() {
      var request = new GetDeviceList();
      APIClient.HttpClient.Post(APIClient.BuildFullUrl(request.Url), request.GetJson(), OnTransferResponse);
    }

    private static void OnTransferResponse(string response) {
      var deviceList = GetDeviceListResponse.Deserialize(response);
      Devices = new Devices(deviceList.Devices, Mutex);
      if(LoadComplete != null) LoadComplete();
    }
  }
}
