using System.Collections.Generic;
using System.Linq;

namespace AET.Zigen.IPLogic {
  public class Devices {
    private readonly Dictionary<string, Device> devices = new Dictionary<string, Device>();
    private readonly Dictionary<string, Dictionary<int, Device>> devicesByIndex = new Dictionary<string, Dictionary<int, Device>>();

    public Devices() {
      Mutex = new CrestronMutex();
      InitDictionaries();
    }

    public Devices(List<Device> newDevices) : this() {
      FillDevices(newDevices);
      RebuildDicts();
    }

    internal Devices(List<Device> newDevices, IMutex mutex) {
      Mutex = mutex;
      InitDictionaries();
      FillDevices(newDevices);
      RebuildDicts();
    }

    private void FillDevices(List<Device> newDevices) {
      foreach(var device in newDevices) {
        Add(device);
      }
    }
    private void InitDictionaries() {
      devicesByIndex.Add("receiver", new Dictionary<int, Device>());
      devicesByIndex.Add("transmitter", new Dictionary<int, Device>());
    }

    internal IMutex Mutex { get; set; }

    public Device this[string deviceType, int index] {
      get {
        Dictionary<int, Device> dict;
        if(devicesByIndex.TryGetValue(deviceType, out dict))
        {
          Device device;
          if(dict.TryGetValue(index, out device)) {
            return device;
          }
        }
        if(index != 0) Config.Logger.Warn("Unable to find {0} at index {1}.", deviceType, index);
        return new Device();
      }
    }
    public Device this[string name] {
      get {
        Device device;
        if (devices.TryGetValue(name, out device)) {
            return device;
        }
        Config.Logger.Warn("Unable to find device with name {0}.", name);
        return new Device();
      }
    }
  

    public void Add(Device device) {
      if (devices.ContainsKey(device.Name)) {
        devices[device.Name] = device;
        Config.Logger.Warn("Device with name '{0}' already exists in device list. Updating.");
      } else devices.Add(device.Name, device);
    }

    public void RebuildDicts() {
      
      BuildDict("receiver");
      BuildDict("transmitter");
    }

    private void BuildDict(string deviceType) {
      Mutex.Enter();
      var deviceTypeDict = new Dictionary<int, Device>();
      devicesByIndex[deviceType] = deviceTypeDict;
      var filteredDevices = devices.Values.Where(d => d.DeviceType == deviceType);
      foreach (var device in filteredDevices) {
        Device d;
        if (deviceTypeDict.TryGetValue(device.Index, out d)) d = device;
        else deviceTypeDict.Add(device.Index, device);
      }      
      Mutex.Leave();
    }
  }
}
